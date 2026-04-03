using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;

using System;
using System.Collections.Generic;

namespace StarMage.Core.TextSystem
{
    internal static class TextRenderer
    {
        private static readonly List<string> lines = [];

        internal static void DrawCharacter(SpriteBatch spriteBatch, char character, Point position)
        {
            if (!Constants.TryGetCharacterSourceRectangle(character, out Rectangle sourceRectangle))
            {
                return; // If the character is not mapped, it is not drawn.
            }

            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), new Vector2(position.X, position.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        internal static void DrawText(SpriteBatch spriteBatch, string value, Point position, TextRenderSettings options)
        {
            lines.Clear();

            // Determines the maximum width for line breaks.
            int maxWidth = options.HasMaxDimensions ? options.MaxDimensions.X : Constants.SCREEN_WIDTH;

            if (options.WrapText)
            {
                int charWidthWithSpacing = Constants.FONT_WIDTH + options.CharacterSpacing;
                int charsPerLine = maxWidth / charWidthWithSpacing;

                if (charsPerLine < 1)
                {
                    charsPerLine = 1;
                }

                // Divide the text into lines according to the number of characters.
                for (int i = 0; i < value.Length; i += charsPerLine)
                {
                    int length = i + charsPerLine > value.Length ? value.Length - i : charsPerLine;
                    lines.Add(value.Substring(i, length));
                }
            }
            else
            {
                // No wrapping, text is rendered on a single line.
                lines.Add(value);
            }

            // Calculates the total height of the text block (including line spacing).
            int totalHeight = (lines.Count * Constants.FONT_HEIGHT) + ((lines.Count - 1) * options.LineSpacing);

            // Adjust the start Y position based on the vertical alignment.
            int startY = position.Y;

            switch (options.VerticalAlignment)
            {
                case TextVerticalAlignment.Center:
                    startY = position.Y - (totalHeight / 2);
                    break;

                case TextVerticalAlignment.Bottom:
                    startY = position.Y - totalHeight;
                    break;

                default:
                    break;
            }

            // Render each line.
            for (int lineIndex = 0, lineCount = lines.Count; lineIndex < lineCount; lineIndex++)
            {
                ReadOnlySpan<char> line = lines[lineIndex].AsSpan();

                // Calculate the width of the current line.
                int lineWidth = (line.Length * (Constants.FONT_WIDTH + options.CharacterSpacing)) - options.CharacterSpacing;

                // Adjust the start X position based on the horizontal alignment.
                int startX = position.X;

                switch (options.HorizontalAlignment)
                {
                    case TextAlignment.Center:
                        startX = position.X - (lineWidth / 2);
                        break;

                    case TextAlignment.Right:
                        startX = position.X - lineWidth;
                        break;

                    default:
                        break;
                }

                // Draw each character on the line.
                int currentX = startX;
                int currentY = startY + (lineIndex * (Constants.FONT_HEIGHT + options.LineSpacing));

                for (int i = 0; i < line.Length; i++)
                {
                    DrawCharacter(spriteBatch, line[i], new(currentX, currentY));
                    currentX += Constants.FONT_WIDTH + options.CharacterSpacing;
                }
            }
        }
    }
}
