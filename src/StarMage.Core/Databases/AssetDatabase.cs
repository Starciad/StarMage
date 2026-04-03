using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Enums;

namespace StarMage.Core.Databases
{
    internal static class AssetDatabase
    {

        private static Effect[] effects;
        private static Texture2D[] textures;

        internal static void LoadContent(ContentManager contentManager)
        {
            effects =
            [
                contentManager.Load<Effect>("Effects/PaletteIndexed"),
            ];

            textures =
            [
                contentManager.Load<Texture2D>("Textures/Actors"),
                contentManager.Load<Texture2D>("Textures/Tiles"),
                contentManager.Load<Texture2D>("Textures/UI"),
            ];
        }

        internal static Effect GetEffect(EffectIndex effectIndex)
        {
            return effects[(byte)effectIndex];
        }

        internal static Texture2D GetTexture(TextureIndex textureIndex)
        {
            return textures[(byte)textureIndex];
        }
    }
}
