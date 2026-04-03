using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Enums;
using StarMage.Core.Handlers;

using System;

namespace StarMage.Core.Tiles
{
    internal sealed class TileMap
    {
        internal Rectangle Bounds => new(0, 0, this.Width * Constants.TILE_SIZE, this.Height * Constants.TILE_SIZE);

        internal int Width { get; private set; }
        internal int Height { get; private set; }

        private TileType[,] tiles;

        internal TileType this[int x, int y]
        {
            get => GetTile(x, y);
            set => SetTile(value, x, y);
        }

        internal TileType this[Point position]
        {
            get => GetTile(position.X, position.Y);
            set => SetTile(value, position.X, position.Y);
        }

        internal static Point WorldToTile(Point position)
        {
            return new(
                position.X / Constants.TILE_SIZE,
                position.Y / Constants.TILE_SIZE
            );
        }

        internal static Point TileToWorld(Point tilePosition)
        {
            return new(
                tilePosition.X * Constants.TILE_SIZE,
                tilePosition.Y * Constants.TILE_SIZE
            );
        }

        internal static Rectangle GetTileRectangle(int x, int y)
        {
            return new(
                x * Constants.TILE_SIZE,
                y * Constants.TILE_SIZE,
                Constants.TILE_SIZE,
                Constants.TILE_SIZE
            );
        }

        internal bool IsWithinBounds(Point position)
        {
            return IsWithinBounds(position.X, position.Y);
        }

        internal bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < this.Width && y < this.Height;
        }

        internal TileType GetTile(int x, int y)
        {
            return this.tiles[y, x];
        }

        internal void SetTile(TileType type, int x, int y)
        {
            if (IsWithinBounds(x, y))
            {
                this.tiles[y, x] = type;
            }
        }

        internal void Resize(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this.tiles = new TileType[height, width];
        }

        private static bool IsOneWayTileCollision(ref TileType tileType, ref Rectangle tileRect, ref Rectangle nextRect, ref Rectangle prevRect, ref int motionX, ref int motionY)
        {
            // Quick reject: if nextRect does not intersect tile at all, it's not colliding.
            if (!nextRect.Intersects(tileRect))
            {
                return false;
            }

            // Helper overlap checks
            bool horizontalOverlap = nextRect.Right > tileRect.Left && nextRect.Left < tileRect.Right;
            bool verticalOverlap = nextRect.Bottom > tileRect.Top && nextRect.Top < tileRect.Bottom;

            if (TileProperty.IsOneWayTopTile(tileType) &&
                verticalOverlap && horizontalOverlap &&
                motionY > 0 &&
                prevRect.Bottom <= tileRect.Top && nextRect.Bottom > tileRect.Top)
            {
                return true;
            }

            if (TileProperty.IsOneWayBottomTile(tileType) &&
                verticalOverlap && horizontalOverlap &&
                motionY < 0 &&
                prevRect.Top >= tileRect.Bottom && nextRect.Top < tileRect.Bottom)
            {
                return true;
            }

            // One-Way Left: blocks only when moving right into the left face and actor was left before move.
            if (TileProperty.IsOneWayLeftTile(tileType) &&
                horizontalOverlap && verticalOverlap &&
                motionX > 0 &&
                prevRect.Right <= tileRect.Left && nextRect.Right > tileRect.Left)
            {
                return true;
            }

            // One-Way Right: blocks only when moving left into the right face and actor was right before move.
            if (TileProperty.IsOneWayRightTile(tileType) &&
                horizontalOverlap && verticalOverlap &&
                motionX < 0 &&
                prevRect.Left >= tileRect.Right && nextRect.Left < tileRect.Right)
            {
                return true;
            }

            // If tile has none of the above, it doesn't block.
            return false;
        }

        internal bool IsCollideAt(Rectangle collisionRect, int motionX, int motionY)
        {
            if (collisionRect.IsEmpty)
            {
                return false;
            }

            const double size = Constants.TILE_SIZE;

            int startX = (int)Math.Floor(collisionRect.Left / size);
            int endX = (int)Math.Floor((collisionRect.Right - 1) / size);
            int startY = (int)Math.Floor(collisionRect.Top / size);
            int endY = (int)Math.Floor((collisionRect.Bottom - 1) / size);

            if (endX < 0 || endY < 0 || startX > this.Width - 1 || startY > this.Height - 1)
            {
                return false;
            }

            startX = Math.Max(0, startX);
            startY = Math.Max(0, startY);
            endX = Convert.ToInt32(Math.Min(this.Width - 1, endX));
            endY = Convert.ToInt32(Math.Min(this.Height - 1, endY));

            Rectangle prevRect = collisionRect;
            prevRect.Offset(motionX * -1, motionY * -1);

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    TileType tileType = this[x, y];

                    if (tileType is TileType.NNN)
                    {
                        continue;
                    }

                    Rectangle tileRect = GetTileRectangle(x, y);

                    if (TileProperty.IsSolidTile(tileType) || IsOneWayTileCollision(ref tileType, ref tileRect, ref collisionRect, ref prevRect, ref motionX, ref motionY))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Rectangle viewBounds = CameraHandler.GetViewBounds();

            int minTileX = (int)Math.Clamp(MathF.Floor(viewBounds.Left / Constants.TILE_SIZE), 0, this.Width);
            int minTileY = (int)Math.Clamp(MathF.Floor(viewBounds.Top / Constants.TILE_SIZE), 0, this.Height);
            int maxTileX = (int)Math.Clamp(MathF.Ceiling(viewBounds.Right / Constants.TILE_SIZE), 0, this.Width) + 1;
            int maxTileY = (int)Math.Clamp(MathF.Ceiling(viewBounds.Bottom / Constants.TILE_SIZE), 0, this.Height);

            for (int y = minTileY; y < maxTileY; y++)
            {
                for (int x = minTileX; x < maxTileX; x++)
                {
                    if (!IsWithinBounds(x, y))
                    {
                        continue;
                    }

                    TileType tileType = this[x, y];

                    if (tileType is TileType.NNN || TileProperty.IsInternalTile(tileType))
                    {
                        continue;
                    }

                    TileRenderer.Draw(spriteBatch, tileType, new(x, y));
                }
            }
        }
    }
}
