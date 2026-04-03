using System;

namespace StarMage.Core.Enums
{
    [Flags]
    internal enum TileCharacteristics : ushort
    {
        /// <summary>
        /// Indicates that the tile has no special characteristics.
        /// </summary>
        None = 0,

        /// <summary>
        /// The tile is solid and blocks movement; it is collidable.
        /// </summary>
        Solid = 1 << 0,

        /// <summary>
        /// The tile allows passage only from bottom to top, restricting movement in other directions.
        /// </summary>
        One_Way_Top = 1 << 1,

        /// <summary>
        /// The tile allows passage only from left to right, restricting movement in other directions.
        /// </summary>
        One_Way_Right = 1 << 2,

        /// <summary>
        /// The tile allows passage only from top to bottom, restricting movement in other directions.
        /// </summary>
        One_Way_Bottom = 1 << 3,

        /// <summary>
        /// The tile allows passage only from right to left, restricting movement in other directions.
        /// </summary>
        One_Way_Left = 1 << 4,

        /// <summary>
        /// The tile is purely decorative and does not affect movement or gameplay; it is non-collidable and does not interact with the player or other entities.
        /// </summary>
        Decorative = 1 << 5,

        /// <summary>
        /// The tile is used for internal game operations.
        /// </summary>
        Internal = 1 << 6
    }
}
