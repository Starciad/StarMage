using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StarMage.Core.Enums;

namespace StarMage.Core.Databases
{
    internal static class AssetDatabase
    {
        private static Effect[] effects;
        private static Song[] songs;
        private static SoundEffect[] soundEffects;
        private static Texture2D[] textures;

        internal static void LoadContent(ContentManager contentManager)
        {
            effects =
            [
                contentManager.Load<Effect>("Effects/PaletteIndexed"),
            ];

            songs =
            [
                contentManager.Load<Song>("Songs/Boss_Battle"),
                contentManager.Load<Song>("Songs/World_01"),
                contentManager.Load<Song>("Songs/World_02"),
                contentManager.Load<Song>("Songs/World_03"),
                contentManager.Load<Song>("Songs/World_04"),
            ];

            soundEffects =
            [

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

        internal static Song GetSong(SongIndex songIndex)
        {
            return songs[(byte)songIndex];
        }

        internal static SoundEffect GetSoundEffect(SoundEffectIndex soundEffectIndex)
        {
            return soundEffects[(byte)soundEffectIndex];
        }

        internal static Texture2D GetTexture(TextureIndex textureIndex)
        {
            return textures[(byte)textureIndex];
        }
    }
}
