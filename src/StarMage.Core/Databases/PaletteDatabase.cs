using StarMage.Core.Colors;
using StarMage.Core.Enums;

namespace StarMage.Core.Databases
{
    internal static class PaletteDatabase
    {
        private static readonly Palette[] palettes =
        [
            // Background
            new(ColorIndex.Black, ColorIndex.DarkGreen, ColorIndex.SeaGreen, ColorIndex.LightGray),
            new(ColorIndex.Black, ColorIndex.Peru, ColorIndex.OliveDrab, ColorIndex.LightGray),
            new(ColorIndex.Black, ColorIndex.RoyalBlue, ColorIndex.BlueViolet, ColorIndex.LightGray),
            new(ColorIndex.Black, ColorIndex.Crimson, ColorIndex.MediumVioletRed, ColorIndex.LightGray),

            // Sprites
            new(ColorIndex.Black, ColorIndex.BlueViolet, ColorIndex.MediumVioletRed, ColorIndex.LightGray),
            new(ColorIndex.Black, ColorIndex.OliveDrab, ColorIndex.Green, ColorIndex.LightGray),
            new(ColorIndex.Black, ColorIndex.Crimson, ColorIndex.Peru, ColorIndex.LightGray),
            new(ColorIndex.Black, ColorIndex.Teal, ColorIndex.RoyalBlue, ColorIndex.LightGray),
        ];

        internal static Palette GetPalette(PaletteIndex index)
        {
            return palettes[(byte)index];
        }
    }
}
