using Microsoft.Xna.Framework;

using StarMage.Core.Enums;

namespace StarMage.Core.Colors
{
    internal sealed class Palette
    {
        internal Color[] Colors => this.colors;
        internal Vector4[] Vectors => this.vectors;

        private readonly Color[] colors = new Color[4];
        private readonly Vector4[] vectors = new Vector4[4];

        internal Palette(ColorIndex darkColor, ColorIndex mediumDarkColor, ColorIndex mediumLightColor, ColorIndex lightColor)
        {
            this.colors[0] = Constants.DEFAULT_COLOR_PALETTE[(byte)darkColor];
            this.colors[1] = Constants.DEFAULT_COLOR_PALETTE[(byte)mediumDarkColor];
            this.colors[2] = Constants.DEFAULT_COLOR_PALETTE[(byte)mediumLightColor];
            this.colors[3] = Constants.DEFAULT_COLOR_PALETTE[(byte)lightColor];

            this.vectors[0] = Constants.DEFAULT_COLOR_PALETTE_VECTORS[(byte)darkColor];
            this.vectors[1] = Constants.DEFAULT_COLOR_PALETTE_VECTORS[(byte)mediumDarkColor];
            this.vectors[2] = Constants.DEFAULT_COLOR_PALETTE_VECTORS[(byte)mediumLightColor];
            this.vectors[3] = Constants.DEFAULT_COLOR_PALETTE_VECTORS[(byte)lightColor];
        }
    }
}
