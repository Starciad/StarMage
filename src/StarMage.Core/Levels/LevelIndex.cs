using System;
using System.Runtime.Serialization;

namespace StarMage.Core.Levels
{
    [DataContract]
    internal struct LevelIndex : IEquatable<LevelIndex>, IComparable<LevelIndex>, IFormattable
    {
        [DataMember]
        public int Index
        {
            readonly get => this.index;

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Total must be at least 1.");
                }

                this.index = value;
            }
        }

        [DataMember]
        public readonly int GroupSize => this.groupSize;

        [IgnoreDataMember]
        public int World
        {
            readonly get => ((this.index - 1) / this.groupSize) + 1;

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "World must be at least 1.");
                }

                int currentLevelInWorld = this.LevelInWorld;
                int newTotal = ((value - 1) * this.groupSize) + currentLevelInWorld;
                this.index = newTotal;
            }
        }

        [IgnoreDataMember]
        public int LevelInWorld
        {
            readonly get => ((this.index - 1) % this.groupSize) + 1;
            set
            {
                if (value < 1 || value > this.groupSize)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"LevelInWorld must be between 1 and {this.groupSize}.");
                }

                int currentWorld = this.World;
                int newTotal = ((currentWorld - 1) * this.groupSize) + value;
                this.index = newTotal;
            }
        }

        private int index;
        private readonly int groupSize;

        public LevelIndex()
        {
            this.index = 1;
            this.groupSize = 4;
        }

        public LevelIndex(int total)
        {
            if (total < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(total), "Total must be at least 1.");
            }

            this.index = total;
            this.groupSize = 4;
        }

        public LevelIndex(int total, int groupSize)
        {
            if (total < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(total), "Total must be at least 1.");
            }

            if (groupSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(groupSize), "GroupSize must be at least 1.");
            }

            this.index = total;
            this.groupSize = groupSize;
        }

        public static LevelIndex operator +(LevelIndex left, LevelIndex right)
        {
            if (left.groupSize != right.groupSize)
            {
                throw new InvalidOperationException("Cannot add two LevelIndex instances with different group sizes.");
            }

            int sum = left.index + right.index;
            return new LevelIndex(sum, left.groupSize);
        }

        public static LevelIndex operator +(LevelIndex left, int value)
        {
            int result = left.index + value;
            return new LevelIndex(result, left.groupSize);
        }

        public static LevelIndex operator -(LevelIndex left, LevelIndex right)
        {
            if (left.groupSize != right.groupSize)
            {
                throw new InvalidOperationException("Cannot subtract two LevelIndex instances with different group sizes.");
            }

            int difference = left.index - right.index;
            return new LevelIndex(difference, left.groupSize);
        }

        public static LevelIndex operator -(LevelIndex left, int value)
        {
            int result = left.index - value;
            return new LevelIndex(result, left.groupSize);
        }

        public static LevelIndex operator ++(LevelIndex value)
        {
            return new LevelIndex(value.index + 1, value.groupSize);
        }

        public static LevelIndex operator --(LevelIndex value)
        {
            return value.index == 1
                ? throw new InvalidOperationException("Cannot decrement LevelIndex below 1.")
                : new LevelIndex(value.index - 1, value.groupSize);
        }

        public static bool operator ==(LevelIndex left, LevelIndex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LevelIndex left, LevelIndex right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(LevelIndex left, LevelIndex right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(LevelIndex left, LevelIndex right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(LevelIndex left, LevelIndex right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(LevelIndex left, LevelIndex right)
        {
            return left.CompareTo(right) >= 0;
        }

        public override readonly bool Equals(object obj)
        {
            return obj is LevelIndex other && Equals(other);
        }

        public readonly bool Equals(LevelIndex other)
        {
            return this.index == other.index && this.groupSize == other.groupSize;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.index, this.groupSize);
        }

        public override readonly string ToString()
        {
            return string.Format("{0}-{1}", this.World, this.LevelInWorld);
        }

        public readonly string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format) || format == "F")
            {
                return $"{this.World}-{this.LevelInWorld}";
            }
            else if (format == "W")
            {
                return this.World.ToString(formatProvider);
            }
            else if (format == "L")
            {
                return this.LevelInWorld.ToString(formatProvider);
            }

            throw new FormatException($"Unknown format '{format}'.");
        }

        public static implicit operator int(LevelIndex source)
        {
            return source.index;
        }

        public static explicit operator LevelIndex(int total)
        {
            const int defaultGroupSize = 4;
            return new LevelIndex(total, defaultGroupSize);
        }

        public readonly int CompareTo(LevelIndex other)
        {
            return this.groupSize != other.groupSize
                ? throw new InvalidOperationException("Cannot compare levels with different group sizes.")
                : this.index.CompareTo(other.index);
        }

        public readonly LevelIndex Next()
        {
            return new LevelIndex(this.index + 1, this.groupSize);
        }

        public readonly LevelIndex Previous()
        {
            return this.index == 1
                ? throw new InvalidOperationException("Cannot go below level 1.")
                : new LevelIndex(this.index - 1, this.groupSize);
        }

        public readonly bool IsFirstInWorld()
        {
            return this.LevelInWorld == 1;
        }

        public readonly bool IsLastInWorld()
        {
            return this.LevelInWorld == this.groupSize;
        }

        public static int Distance(LevelIndex a, LevelIndex b)
        {
            return a.groupSize != b.groupSize
                ? throw new InvalidOperationException("Cannot measure distance between different group sizes.")
                : Math.Abs(a.index - b.index);
        }

        public readonly void Deconstruct(out int total, out int groupSize)
        {
            total = this.index;
            groupSize = this.groupSize;
        }
    }
}
