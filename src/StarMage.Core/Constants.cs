using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace StarMage.Core
{
    internal static class Constants
    {
        internal const int SCREEN_WIDTH = 256;
        internal const int SCREEN_HEIGHT = 224;

        internal const int FONT_WIDTH = 9;
        internal const int FONT_HEIGHT = 10;

        internal const int TILE_SIZE = 16;

        internal const int CHUNK_WIDTH_TILES = 16;
        internal const int CHUNK_HEIGHT_TILES = 11;

        internal const uint TOTAL_SECONDS = 3_600;
        internal const ushort MAXIMUM_LEVEL_SECONDS = 10_799;
        internal const ushort DEFAULT_LEVEL_SECONDS = 120;
        internal const byte MINIMUM_LEVEL_SECONDS = 0;

        internal const float GAME_FPS = 1.0f / 60.0f;

        internal const byte DEFAULT_BIG_CRYSTALS_PER_LEVEL = 3;
        internal const byte FINAL_LEVEL = 16;

        internal const byte MAX_CONCURRENT_SOUNDS = 4;

        internal static readonly Point SCREEN_SIZE = new(SCREEN_WIDTH, SCREEN_HEIGHT);

        internal static readonly Color[] DEFAULT_COLOR_PALETTE =
        [
            new(175, 175, 175, 255), // [0] LightGray
            new(15, 81, 221, 255),   // [1] RoyalBlue
            new(68, 47, 243, 255),   // [2] BlueViolet
            new(114, 32, 226, 255),  // [3] DarkOrchid
            new(163, 25, 179, 255),  // [4] MediumVioletRed
            new(174, 28, 81, 255),   // [5] Crimson
            new(164, 52, 0, 255),    // [6] SaddleBrown
            new(136, 77, 0, 255),    // [7] Peru
            new(103, 109, 0, 255),   // [8] OliveDrab
            new(32, 128, 0, 255),    // [9] Green
            new(0, 139, 0, 255),     // [10] DarkGreen
            new(0, 127, 66, 255),    // [11] SeaGreen
            new(0, 108, 151, 255),   // [12] Teal
            new(1, 1, 1, 255)        // [13] Black
        ];

        internal static readonly Vector4[] DEFAULT_COLOR_PALETTE_VECTORS =
        [
            DEFAULT_COLOR_PALETTE[00].ToVector4(), // [0] LightGray
            DEFAULT_COLOR_PALETTE[01].ToVector4(), // [1] RoyalBlue
            DEFAULT_COLOR_PALETTE[02].ToVector4(), // [2] BlueViolet
            DEFAULT_COLOR_PALETTE[03].ToVector4(), // [3] DarkOrchid
            DEFAULT_COLOR_PALETTE[04].ToVector4(), // [4] MediumVioletRed
            DEFAULT_COLOR_PALETTE[05].ToVector4(), // [5] Crimson
            DEFAULT_COLOR_PALETTE[06].ToVector4(), // [6] SaddleBrown
            DEFAULT_COLOR_PALETTE[07].ToVector4(), // [7] Peru
            DEFAULT_COLOR_PALETTE[08].ToVector4(), // [8] OliveDrab
            DEFAULT_COLOR_PALETTE[09].ToVector4(), // [9] Green
            DEFAULT_COLOR_PALETTE[10].ToVector4(), // [10] DarkGreen
            DEFAULT_COLOR_PALETTE[11].ToVector4(), // [11] SeaGreen
            DEFAULT_COLOR_PALETTE[12].ToVector4(), // [12] Teal
            DEFAULT_COLOR_PALETTE[13].ToVector4()  // [13] Black
        ];

        private static readonly Dictionary<char, Rectangle> MAPPED_CHARACTERS = new()
        {
            #region Uppercase Letters

            ['A'] = new(9 * 00, 100, FONT_WIDTH, FONT_HEIGHT),
            ['B'] = new(9 * 01, 100, FONT_WIDTH, FONT_HEIGHT),
            ['C'] = new(9 * 02, 100, FONT_WIDTH, FONT_HEIGHT),
            ['D'] = new(9 * 03, 100, FONT_WIDTH, FONT_HEIGHT),
            ['E'] = new(9 * 04, 100, FONT_WIDTH, FONT_HEIGHT),
            ['F'] = new(9 * 05, 100, FONT_WIDTH, FONT_HEIGHT),
            ['G'] = new(9 * 06, 100, FONT_WIDTH, FONT_HEIGHT),
            ['H'] = new(9 * 07, 100, FONT_WIDTH, FONT_HEIGHT),
            ['I'] = new(9 * 08, 100, FONT_WIDTH, FONT_HEIGHT),
            ['J'] = new(9 * 09, 100, FONT_WIDTH, FONT_HEIGHT),
            ['K'] = new(9 * 10, 100, FONT_WIDTH, FONT_HEIGHT),
            ['L'] = new(9 * 11, 100, FONT_WIDTH, FONT_HEIGHT),
            ['M'] = new(9 * 12, 100, FONT_WIDTH, FONT_HEIGHT),
            ['N'] = new(9 * 13, 100, FONT_WIDTH, FONT_HEIGHT),
            ['O'] = new(9 * 14, 100, FONT_WIDTH, FONT_HEIGHT),
            ['P'] = new(9 * 15, 100, FONT_WIDTH, FONT_HEIGHT),
            ['Q'] = new(9 * 16, 100, FONT_WIDTH, FONT_HEIGHT),
            ['R'] = new(9 * 17, 100, FONT_WIDTH, FONT_HEIGHT),
            ['S'] = new(9 * 18, 100, FONT_WIDTH, FONT_HEIGHT),
            ['T'] = new(9 * 19, 100, FONT_WIDTH, FONT_HEIGHT),
            ['U'] = new(9 * 20, 100, FONT_WIDTH, FONT_HEIGHT),
            ['V'] = new(9 * 21, 100, FONT_WIDTH, FONT_HEIGHT),
            ['W'] = new(9 * 22, 100, FONT_WIDTH, FONT_HEIGHT),
            ['X'] = new(9 * 23, 100, FONT_WIDTH, FONT_HEIGHT),
            ['Y'] = new(9 * 24, 100, FONT_WIDTH, FONT_HEIGHT),
            ['Z'] = new(9 * 25, 100, FONT_WIDTH, FONT_HEIGHT),

            #endregion

            #region Lowercase Letters

            ['a'] = new(9 * 00, 110, FONT_WIDTH, FONT_HEIGHT),
            ['b'] = new(9 * 01, 110, FONT_WIDTH, FONT_HEIGHT),
            ['c'] = new(9 * 02, 110, FONT_WIDTH, FONT_HEIGHT),
            ['d'] = new(9 * 03, 110, FONT_WIDTH, FONT_HEIGHT),
            ['e'] = new(9 * 04, 110, FONT_WIDTH, FONT_HEIGHT),
            ['f'] = new(9 * 05, 110, FONT_WIDTH, FONT_HEIGHT),
            ['g'] = new(9 * 06, 110, FONT_WIDTH, FONT_HEIGHT),
            ['h'] = new(9 * 07, 110, FONT_WIDTH, FONT_HEIGHT),
            ['i'] = new(9 * 08, 110, FONT_WIDTH, FONT_HEIGHT),
            ['j'] = new(9 * 09, 110, FONT_WIDTH, FONT_HEIGHT),
            ['k'] = new(9 * 10, 110, FONT_WIDTH, FONT_HEIGHT),
            ['l'] = new(9 * 11, 110, FONT_WIDTH, FONT_HEIGHT),
            ['m'] = new(9 * 12, 110, FONT_WIDTH, FONT_HEIGHT),
            ['n'] = new(9 * 13, 110, FONT_WIDTH, FONT_HEIGHT),
            ['o'] = new(9 * 14, 110, FONT_WIDTH, FONT_HEIGHT),
            ['p'] = new(9 * 15, 110, FONT_WIDTH, FONT_HEIGHT),
            ['q'] = new(9 * 16, 110, FONT_WIDTH, FONT_HEIGHT),
            ['r'] = new(9 * 17, 110, FONT_WIDTH, FONT_HEIGHT),
            ['s'] = new(9 * 18, 110, FONT_WIDTH, FONT_HEIGHT),
            ['t'] = new(9 * 19, 110, FONT_WIDTH, FONT_HEIGHT),
            ['u'] = new(9 * 20, 110, FONT_WIDTH, FONT_HEIGHT),
            ['v'] = new(9 * 21, 110, FONT_WIDTH, FONT_HEIGHT),
            ['w'] = new(9 * 22, 110, FONT_WIDTH, FONT_HEIGHT),
            ['x'] = new(9 * 23, 110, FONT_WIDTH, FONT_HEIGHT),
            ['y'] = new(9 * 24, 110, FONT_WIDTH, FONT_HEIGHT),
            ['z'] = new(9 * 25, 110, FONT_WIDTH, FONT_HEIGHT),

            #endregion

            #region Numeric Digits

            ['0'] = new(9 * 00, 120, FONT_WIDTH, FONT_HEIGHT),
            ['1'] = new(9 * 01, 120, FONT_WIDTH, FONT_HEIGHT),
            ['2'] = new(9 * 02, 120, FONT_WIDTH, FONT_HEIGHT),
            ['3'] = new(9 * 03, 120, FONT_WIDTH, FONT_HEIGHT),
            ['4'] = new(9 * 04, 120, FONT_WIDTH, FONT_HEIGHT),
            ['5'] = new(9 * 05, 120, FONT_WIDTH, FONT_HEIGHT),
            ['6'] = new(9 * 06, 120, FONT_WIDTH, FONT_HEIGHT),
            ['7'] = new(9 * 07, 120, FONT_WIDTH, FONT_HEIGHT),
            ['8'] = new(9 * 08, 120, FONT_WIDTH, FONT_HEIGHT),
            ['9'] = new(9 * 09, 120, FONT_WIDTH, FONT_HEIGHT),

            #endregion

            #region Symbols

            [' '] = new(9 * 00, 130, FONT_WIDTH, FONT_HEIGHT),
            ['$'] = new(9 * 01, 130, FONT_WIDTH, FONT_HEIGHT),
            ['€'] = new(9 * 02, 130, FONT_WIDTH, FONT_HEIGHT),
            ['£'] = new(9 * 03, 130, FONT_WIDTH, FONT_HEIGHT),
            ['¥'] = new(9 * 04, 130, FONT_WIDTH, FONT_HEIGHT),
            ['¤'] = new(9 * 05, 130, FONT_WIDTH, FONT_HEIGHT),
            ['+'] = new(9 * 06, 130, FONT_WIDTH, FONT_HEIGHT),
            ['-'] = new(9 * 07, 130, FONT_WIDTH, FONT_HEIGHT),
            ['*'] = new(9 * 08, 130, FONT_WIDTH, FONT_HEIGHT),
            ['/'] = new(9 * 09, 130, FONT_WIDTH, FONT_HEIGHT),
            ['='] = new(9 * 10, 130, FONT_WIDTH, FONT_HEIGHT),
            ['%'] = new(9 * 11, 130, FONT_WIDTH, FONT_HEIGHT),
            ['\"'] = new(9 * 12, 130, FONT_WIDTH, FONT_HEIGHT),
            ['\''] = new(9 * 13, 130, FONT_WIDTH, FONT_HEIGHT),
            ['#'] = new(9 * 14, 130, FONT_WIDTH, FONT_HEIGHT),
            ['@'] = new(9 * 15, 130, FONT_WIDTH, FONT_HEIGHT),
            ['&'] = new(9 * 16, 130, FONT_WIDTH, FONT_HEIGHT),
            ['_'] = new(9 * 17, 130, FONT_WIDTH, FONT_HEIGHT),
            ['('] = new(9 * 18, 130, FONT_WIDTH, FONT_HEIGHT),
            [')'] = new(9 * 19, 130, FONT_WIDTH, FONT_HEIGHT),
            [','] = new(9 * 20, 130, FONT_WIDTH, FONT_HEIGHT),
            ['.'] = new(9 * 21, 130, FONT_WIDTH, FONT_HEIGHT),
            [';'] = new(9 * 22, 130, FONT_WIDTH, FONT_HEIGHT),
            [':'] = new(9 * 23, 130, FONT_WIDTH, FONT_HEIGHT),
            ['?'] = new(9 * 24, 130, FONT_WIDTH, FONT_HEIGHT),
            ['!'] = new(9 * 25, 130, FONT_WIDTH, FONT_HEIGHT),
            ['\\'] = new(9 * 26, 130, FONT_WIDTH, FONT_HEIGHT),
            ['|'] = new(9 * 27, 130, FONT_WIDTH, FONT_HEIGHT),
            ['{'] = new(9 * 28, 130, FONT_WIDTH, FONT_HEIGHT),
            ['}'] = new(9 * 29, 130, FONT_WIDTH, FONT_HEIGHT),
            ['<'] = new(9 * 30, 130, FONT_WIDTH, FONT_HEIGHT),
            ['>'] = new(9 * 31, 130, FONT_WIDTH, FONT_HEIGHT),
            ['['] = new(9 * 32, 130, FONT_WIDTH, FONT_HEIGHT),
            [']'] = new(9 * 33, 130, FONT_WIDTH, FONT_HEIGHT),
            ['`'] = new(9 * 34, 130, FONT_WIDTH, FONT_HEIGHT),
            ['^'] = new(9 * 35, 130, FONT_WIDTH, FONT_HEIGHT),
            ['~'] = new(9 * 36, 130, FONT_WIDTH, FONT_HEIGHT),
            ['©'] = new(9 * 37, 130, FONT_WIDTH, FONT_HEIGHT),

            #endregion
        };

        internal static bool TryGetCharacterSourceRectangle(char character, out Rectangle rectangle)
        {
            return MAPPED_CHARACTERS.TryGetValue(character, out rectangle);
        }
    }
}
