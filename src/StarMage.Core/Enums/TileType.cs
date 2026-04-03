namespace StarMage.Core.Enums
{
    internal enum TileType : byte
    {
        /// <summary>
        /// Represents a null or empty tile.
        /// </summary>
        NNN,

        #region BACKGROUND OBJECTS (BGOS)

        /// <summary>
        /// Fence Tile (left)
        /// </summary>
        B01,

        /// <summary>
        /// Fence Tile (center)
        /// </summary>
        B02,

        /// <summary>
        /// Fence Tile (right)
        /// </summary>
        B03,

        /// <summary>
        /// Shrub (left)
        /// </summary>
        B04,

        /// <summary>
        /// Shrub (center)
        /// </summary>
        B05,

        /// <summary>
        /// Shrub (right)
        /// </summary>
        B06,

        /// <summary>
        /// Tree (stem)
        /// </summary>
        B07,

        /// <summary>
        /// Tree Leaf (small)
        /// </summary>
        B08,

        /// <summary>
        /// Tree leaf (large, top)
        /// </summary>
        B09,

        /// <summary>
        /// Tree leaf (large, bottom)
        /// </summary>
        B10,

        /// <summary>
        /// Cloud (northwest)
        /// </summary>
        B11,

        /// <summary>
        /// Cloud (north)
        /// </summary>
        B12,

        /// <summary>
        /// Cloud (northeast)
        /// </summary>
        B13,

        /// <summary>
        /// Cloud (southwest)
        /// </summary>
        B14,

        /// <summary>
        /// Cloud (south)
        /// </summary>
        B15,

        /// <summary>
        /// Cloud (southeast)
        /// </summary>
        B16,

        #endregion

        #region COLLECTIBLES

        /// <summary>
        /// Big Crystal
        /// </summary>
        C01,

        /// <summary>
        /// Treasure Chest
        /// </summary>
        C02,

        /// <summary>
        /// Starting Star
        /// </summary>
        C03,

        /// <summary>
        /// Finishing Star
        /// </summary>
        C04,

        #endregion

        #region ENTITIES

        /// <summary>
        /// Ground Enemy
        /// </summary>
        E01,

        /// <summary>
        /// Ceiling Enemy
        /// </summary>
        E02,

        /// <summary>
        /// Wall Enemy (left)
        /// </summary>
        E03,

        /// <summary>
        /// Wall Enemy (right)
        /// </summary>
        E04,

        /// <summary>
        /// Aerial Enemy
        /// </summary>
        E05,

        /// <summary>
        /// Boss Enemy
        /// </summary>
        E06,

        #endregion

        #region TILES

        /// <summary>
        /// Sand Tile (surface)
        /// </summary>
        T01,

        /// <summary>
        /// Sand Tile (underground)
        /// </summary>
        T02,

        /// <summary>
        /// Stone Tile
        /// </summary>
        T03,

        /// <summary>
        /// Wood Platform Tile (top)
        /// </summary>
        T04,

        /// <summary>
        /// Wood Platform Tile (right)
        /// </summary>
        T05,

        /// <summary>
        /// Wood Platform Tile (bottom)
        /// </summary>
        T06,

        /// <summary>
        /// Wood Platform Tile (left)
        /// </summary>
        T07,

        /// <summary>
        /// Grass Tile (northwest)
        /// </summary>
        T08,

        /// <summary>
        /// Grass Tile (north)
        /// </summary>
        T09,

        /// <summary>
        /// Grass Tile (northeast)
        /// </summary>
        T10,

        /// <summary>
        /// Grass Tile (west)
        /// </summary>
        T11,

        /// <summary>
        /// Grass Tile (center)
        /// </summary>
        T12,

        /// <summary>
        /// Grass Tile (east)
        /// </summary>
        T13,

        /// <summary>
        /// Grass Tile (southwest)
        /// </summary>
        T14,

        /// <summary>
        /// Grass Tile (south)
        /// </summary>
        T15,

        /// <summary>
        /// Grass Tile (southeast)
        /// </summary>
        T16,

        /// <summary>
        /// Grass Tile Gap (northwest)
        /// </summary>
        T17,

        /// <summary>
        /// Grass Tile Gap (northeast)
        /// </summary>
        T18,

        /// <summary>
        /// Grass Tile Gap (southwest)
        /// </summary>
        T19,

        /// <summary>
        /// Grass Tile Gap (southeast)
        /// </summary>
        T20,

        /// <summary>
        /// Semi-Solid Structure (northwest)
        /// </summary>
        T21,

        /// <summary>
        /// Semi-Solid Structure (north)
        /// </summary>
        T22,

        /// <summary>
        /// Semi-Solid Structure (northeast)
        /// </summary>
        T23,

        /// <summary>
        /// Semi-Solid Structure (west)
        /// </summary>
        T24,

        /// <summary>
        /// Semi-Solid Structure (center)
        /// </summary>
        T25,

        /// <summary>
        /// Semi-Solid Structure (east)
        /// </summary>
        T26,

        /// <summary>
        /// Semi-Solid Structure (southwest)
        /// </summary>
        T27,

        /// <summary>
        /// Semi-Solid Structure (south)
        /// </summary>
        T28,

        /// <summary>
        /// Semi-Solid Structure (southeast)
        /// </summary>
        T29,

        #endregion
    }
}
