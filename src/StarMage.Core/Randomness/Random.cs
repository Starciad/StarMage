namespace StarMage.Core.Randomness
{
    internal static class Random
    {
        private static readonly System.Random _random = new();

        internal static byte Range(byte min, byte max)
        {
            return (byte)_random.Next(min, max + 1);
        }

        internal static sbyte Range(sbyte min, sbyte max)
        {
            return (sbyte)_random.Next(min, max + 1);
        }

        internal static short Range(short min, short max)
        {
            return (short)_random.Next(min, max + 1);
        }

        internal static ushort Range(ushort min, ushort max)
        {
            return (ushort)_random.Next(min, max + 1);
        }

        internal static int Range(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        internal static uint Range(uint min, uint max)
        {
            return (uint)_random.Next((int)min, (int)(max + 1));
        }

        internal static long Range(long min, long max)
        {
            return min + (long)(_random.NextDouble() * (max - min + 1));
        }

        internal static ulong Range(ulong min, ulong max)
        {
            return min + (ulong)(_random.NextDouble() * (long)(max - min + 1));
        }

        internal static float Range(float min, float max)
        {
            return (float)((_random.NextDouble() * (max - min)) + min);
        }

        internal static double Range(double min, double max)
        {
            return (_random.NextDouble() * (max - min)) + min;
        }

        internal static decimal Range(decimal min, decimal max)
        {
            return ((decimal)_random.NextDouble() * (max - min)) + min;
        }

        internal static byte Range(byte max)
        {
            return Range((byte)0, max);
        }

        internal static sbyte Range(sbyte max)
        {
            return Range((sbyte)0, max);
        }

        internal static short Range(short max)
        {
            return Range((short)0, max);
        }

        internal static ushort Range(ushort max)
        {
            return Range((ushort)0, max);
        }

        internal static int Range(int max)
        {
            return Range(0, max);
        }

        internal static uint Range(uint max)
        {
            return Range(0u, max);
        }

        internal static long Range(long max)
        {
            return Range(0L, max);
        }

        internal static ulong Range(ulong max)
        {
            return Range(0UL, max);
        }

        internal static float Range(float max)
        {
            return Range(0f, max);
        }

        internal static double Range(double max)
        {
            return Range(0.0, max);
        }

        internal static decimal Range(decimal max)
        {
            return Range(0m, max);
        }

        internal static bool GetBool()
        {
            return _random.Next(2) == 0;
        }

        internal static bool Chance(int chance)
        {
            return Chance(chance, 100);
        }

        internal static bool Chance(int chance, int total)
        {
            return Range(0, total) < chance;
        }
    }
}
