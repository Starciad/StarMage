using StarMage.Core.Enums;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Chunks
{
    internal sealed class Chunk
    {
        internal ChunkType Type { get; }

        internal int Width { get; }
        internal int Height { get; }

        private readonly TileType[,] tiles;

        internal Chunk(ChunkType type, TileType[,] tiles)
        {
            this.Type = type;
            this.tiles = tiles;

            this.Height = tiles.GetLength(0);
            this.Width = tiles.GetLength(1);

            if (this.Width != Constants.CHUNK_WIDTH_TILES || this.Height != Constants.CHUNK_HEIGHT_TILES)
            {
                throw new ArgumentException($"Chunk must be {Constants.CHUNK_WIDTH_TILES} tiles wide and {Constants.CHUNK_HEIGHT_TILES} tiles high.");
            }
        }

        internal void ApplyToTileMap(TileMap tileMap, int offsetX, int offsetY)
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    tileMap.SetTile(this.tiles[y, x], offsetX + x, offsetY + y);
                }
            }
        }
    }
}
