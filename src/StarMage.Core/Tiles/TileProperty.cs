using Microsoft.Xna.Framework;

using StarMage.Core.Enums;

namespace StarMage.Core.Tiles
{
    internal static class TileProperty
    {
        private static readonly TileCharacteristics[] flagsTable = new TileCharacteristics[byte.MaxValue];

        private static void SetFlags(TileType type, TileCharacteristics flags)
        {
            flagsTable[(int)type] = flags;
        }

        internal static void Initialize()
        {
            SetFlags(TileType.NNN, TileCharacteristics.None);

            SetFlags(TileType.B01, TileCharacteristics.Decorative);
            SetFlags(TileType.B02, TileCharacteristics.Decorative);
            SetFlags(TileType.B03, TileCharacteristics.Decorative);
            SetFlags(TileType.B04, TileCharacteristics.Decorative);
            SetFlags(TileType.B05, TileCharacteristics.Decorative);
            SetFlags(TileType.B06, TileCharacteristics.Decorative);
            SetFlags(TileType.B07, TileCharacteristics.Decorative);
            SetFlags(TileType.B08, TileCharacteristics.Decorative);
            SetFlags(TileType.B09, TileCharacteristics.Decorative);
            SetFlags(TileType.B10, TileCharacteristics.Decorative);
            SetFlags(TileType.B11, TileCharacteristics.Decorative);
            SetFlags(TileType.B12, TileCharacteristics.Decorative);
            SetFlags(TileType.B13, TileCharacteristics.Decorative);
            SetFlags(TileType.B14, TileCharacteristics.Decorative);
            SetFlags(TileType.B15, TileCharacteristics.Decorative);
            SetFlags(TileType.B16, TileCharacteristics.Decorative);

            SetFlags(TileType.C01, TileCharacteristics.Internal);
            SetFlags(TileType.C02, TileCharacteristics.Internal);
            SetFlags(TileType.C03, TileCharacteristics.Internal);
            SetFlags(TileType.C04, TileCharacteristics.Internal);

            SetFlags(TileType.E01, TileCharacteristics.Internal);
            SetFlags(TileType.E02, TileCharacteristics.Internal);
            SetFlags(TileType.E03, TileCharacteristics.Internal);
            SetFlags(TileType.E04, TileCharacteristics.Internal);
            SetFlags(TileType.E05, TileCharacteristics.Internal);
            SetFlags(TileType.E06, TileCharacteristics.Internal);

            SetFlags(TileType.T01, TileCharacteristics.Solid);
            SetFlags(TileType.T02, TileCharacteristics.Solid);
            SetFlags(TileType.T03, TileCharacteristics.Solid);
            SetFlags(TileType.T04, TileCharacteristics.One_Way_Top);
            SetFlags(TileType.T05, TileCharacteristics.One_Way_Right);
            SetFlags(TileType.T06, TileCharacteristics.One_Way_Bottom);
            SetFlags(TileType.T07, TileCharacteristics.One_Way_Left);
            SetFlags(TileType.T08, TileCharacteristics.Solid);
            SetFlags(TileType.T09, TileCharacteristics.Solid);
            SetFlags(TileType.T10, TileCharacteristics.Solid);
            SetFlags(TileType.T11, TileCharacteristics.Solid);
            SetFlags(TileType.T12, TileCharacteristics.Solid);
            SetFlags(TileType.T13, TileCharacteristics.Solid);
            SetFlags(TileType.T14, TileCharacteristics.Solid);
            SetFlags(TileType.T15, TileCharacteristics.Solid);
            SetFlags(TileType.T16, TileCharacteristics.Solid);
            SetFlags(TileType.T17, TileCharacteristics.Solid);
            SetFlags(TileType.T18, TileCharacteristics.Solid);
            SetFlags(TileType.T19, TileCharacteristics.Solid);
            SetFlags(TileType.T20, TileCharacteristics.Solid);
            SetFlags(TileType.T21, TileCharacteristics.One_Way_Top);
            SetFlags(TileType.T22, TileCharacteristics.One_Way_Top);
            SetFlags(TileType.T23, TileCharacteristics.One_Way_Top);
            SetFlags(TileType.T24, TileCharacteristics.Decorative);
            SetFlags(TileType.T25, TileCharacteristics.Decorative);
            SetFlags(TileType.T26, TileCharacteristics.Decorative);
            SetFlags(TileType.T27, TileCharacteristics.Decorative);
            SetFlags(TileType.T28, TileCharacteristics.Decorative);
            SetFlags(TileType.T29, TileCharacteristics.Decorative);
        }

        internal static TileCharacteristics GetFlags(TileType type)
        {
            return flagsTable[(int)type];
        }

        internal static bool IsSolidTile(TileType type)
        {
            return (GetFlags(type) & TileCharacteristics.Solid) != 0;
        }

        internal static bool IsOneWayTopTile(TileType type)
        {
            return (GetFlags(type) & TileCharacteristics.One_Way_Top) != 0;
        }

        internal static bool IsOneWayRightTile(TileType type)
        {
            return (GetFlags(type) & TileCharacteristics.One_Way_Right) != 0;
        }

        internal static bool IsOneWayBottomTile(TileType type)
        {
            return (GetFlags(type) & TileCharacteristics.One_Way_Bottom) != 0;
        }

        internal static bool IsOneWayLeftTile(TileType type)
        {
            return (GetFlags(type) & TileCharacteristics.One_Way_Left) != 0;
        }

        internal static bool IsDecorativeTile(TileType type)
        {
            return (GetFlags(type) & TileCharacteristics.Decorative) != 0;
        }

        internal static bool IsInternalTile(TileType type)
        {
            return (GetFlags(type) & TileCharacteristics.Internal) != 0;
        }

        internal static Rectangle GetBlockSpriteRectangle(TileType tileType)
        {
            return tileType switch
            {
                #region BGOs

                // Fence
                TileType.B01 => new(new(00, 80), new(Constants.TILE_SIZE)),
                TileType.B02 => new(new(16, 80), new(Constants.TILE_SIZE)),
                TileType.B03 => new(new(32, 80), new(Constants.TILE_SIZE)),

                // Shrub
                TileType.B04 => new(new(00, 96), new(Constants.TILE_SIZE)),
                TileType.B05 => new(new(16, 96), new(Constants.TILE_SIZE)),
                TileType.B06 => new(new(32, 96), new(Constants.TILE_SIZE)),

                // Tree
                TileType.B07 => new(new(48, 80), new(Constants.TILE_SIZE)),

                // Tree Leaf
                TileType.B08 => new(new(48, 64), new(Constants.TILE_SIZE)),
                TileType.B09 => new(new(64, 64), new(Constants.TILE_SIZE)),
                TileType.B10 => new(new(64, 80), new(Constants.TILE_SIZE)),

                // Cloud
                TileType.B11 => new(new(64, 00), new(Constants.TILE_SIZE)),
                TileType.B12 => new(new(80, 00), new(Constants.TILE_SIZE)),
                TileType.B13 => new(new(96, 00), new(Constants.TILE_SIZE)),
                TileType.B14 => new(new(64, 16), new(Constants.TILE_SIZE)),
                TileType.B15 => new(new(80, 16), new(Constants.TILE_SIZE)),
                TileType.B16 => new(new(96, 16), new(Constants.TILE_SIZE)),

                #endregion

                #region TILES

                // Sand
                TileType.T01 => new(new(16, 00), new(Constants.TILE_SIZE)),
                TileType.T02 => new(new(16, 16), new(Constants.TILE_SIZE)),

                // Stone
                TileType.T03 => new(new(00, 16), new(Constants.TILE_SIZE)),

                // Wood Platforms
                TileType.T04 => new(new(32, 0), new(Constants.TILE_SIZE)),
                TileType.T05 => new(new(48, 0), new(Constants.TILE_SIZE)),
                TileType.T06 => new(new(48, 16), new(Constants.TILE_SIZE)),
                TileType.T07 => new(new(32, 16), new(Constants.TILE_SIZE)),

                // Ground
                TileType.T08 => new(new(00, 32), new(Constants.TILE_SIZE)),
                TileType.T09 => new(new(16, 32), new(Constants.TILE_SIZE)),
                TileType.T10 => new(new(32, 32), new(Constants.TILE_SIZE)),
                TileType.T11 => new(new(00, 48), new(Constants.TILE_SIZE)),
                TileType.T12 => new(new(16, 48), new(Constants.TILE_SIZE)),
                TileType.T13 => new(new(32, 48), new(Constants.TILE_SIZE)),
                TileType.T14 => new(new(00, 64), new(Constants.TILE_SIZE)),
                TileType.T15 => new(new(16, 64), new(Constants.TILE_SIZE)),
                TileType.T16 => new(new(32, 64), new(Constants.TILE_SIZE)),
                TileType.T17 => new(new(48, 32), new(Constants.TILE_SIZE)),
                TileType.T18 => new(new(64, 32), new(Constants.TILE_SIZE)),
                TileType.T19 => new(new(48, 48), new(Constants.TILE_SIZE)),
                TileType.T20 => new(new(64, 48), new(Constants.TILE_SIZE)),

                // Semi-Solid Structure
                TileType.T21 => new(new(80, 32), new(Constants.TILE_SIZE)),
                TileType.T22 => new(new(96, 32), new(Constants.TILE_SIZE)),
                TileType.T23 => new(new(112, 32), new(Constants.TILE_SIZE)),
                TileType.T24 => new(new(80, 48), new(Constants.TILE_SIZE)),
                TileType.T25 => new(new(96, 48), new(Constants.TILE_SIZE)),
                TileType.T26 => new(new(112, 48), new(Constants.TILE_SIZE)),
                TileType.T27 => new(new(80, 64), new(Constants.TILE_SIZE)),
                TileType.T28 => new(new(96, 64), new(Constants.TILE_SIZE)),
                TileType.T29 => new(new(112, 64), new(Constants.TILE_SIZE)),

                #endregion

                // Fallback
                _ => new(Point.Zero, new(Constants.TILE_SIZE)),
            };
        }
    }
}
