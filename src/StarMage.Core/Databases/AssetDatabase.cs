using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Enums;

namespace StarMage.Core.Databases
{
    internal static class AssetDatabase
    {
        private static Effect[] effects;
        private static Melody[] melodies;
        private static SoundEffect[] soundEffects;
        private static Texture2D[] textures;

        internal static void LoadContent(ContentManager contentManager)
        {
            effects =
            [
                contentManager.Load<Effect>("Effects/PaletteIndexed"),
            ];

            melodies =
            [
                // Main Menu Melody
                new(
                    new MusicNote(MidiNote.C4, 0.3f),
                    new MusicNote(MidiNote.E4, 0.3f),
                    new MusicNote(MidiNote.G4, 0.3f),
                    new MusicNote(MidiNote.C5, 0.6f)
                ),
            ];

            soundEffects =
            [
                new(800f, 200f, 0.2f), // Jump
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

        internal static Melody GetMelody(MelodyIndex melodyIndex)
        {
            return melodies[(byte)melodyIndex];
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
