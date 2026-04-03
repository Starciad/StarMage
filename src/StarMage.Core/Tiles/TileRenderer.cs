using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;

namespace StarMage.Core.Tiles
{
    internal static class TileRenderer
    {
        internal static void Draw(SpriteBatch spriteBatch, TileType tileType, Point position)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Tiles), new Vector2(position.X, position.Y) * Constants.TILE_SIZE, TileProperty.GetBlockSpriteRectangle(tileType), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
