using Microsoft.Xna.Framework;

using StarMage.Core.Enums;

namespace StarMage.Core.TextSystem
{
    internal sealed class TextRenderSettings
    {
        internal static readonly TextRenderSettings LeftAligned = new()
        {
            HorizontalAlignment = TextAlignment.Left,
        };
        internal static readonly TextRenderSettings Centered = new()
        {
            HorizontalAlignment = TextAlignment.Center,
        };
        internal static readonly TextRenderSettings RightAligned = new()
        {
            HorizontalAlignment = TextAlignment.Right,
        };

        internal sbyte CharacterSpacing { get; set; }
        internal bool HasMaxDimensions { get; set; }
        internal TextAlignment HorizontalAlignment { get; set; }
        internal sbyte LineSpacing { get; set; }
        internal Point MaxDimensions { get; set; }
        internal TextVerticalAlignment VerticalAlignment { get; set; }
        internal bool WrapText { get; set; }

        internal TextRenderSettings()
        {
            this.HorizontalAlignment = TextAlignment.Left;
            this.VerticalAlignment = TextVerticalAlignment.Top;
            this.CharacterSpacing = 0;
            this.LineSpacing = 0;
            this.WrapText = false;
            this.HasMaxDimensions = false;
            this.MaxDimensions = Point.Zero;
        }
    }
}
