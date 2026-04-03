using Microsoft.Xna.Framework;

using StarMage.Core.Enums;

namespace StarMage.Core.Audio
{
    internal static class MusicPlayer
    {
        internal static bool Loop { get; set; } = true;

        private static Melody melody;
        private static int index;
        private static float timer;

        internal static void SetMelody(Melody melody)
        {
            MusicPlayer.melody = melody;
            index = 0;
            timer = 0.0f;
        }

        internal static void Update(GameTime gameTime)
        {
            if (melody == null || melody.Length == 0)
            {
                return;
            }

            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0f)
            {
                MusicNote current = melody[index];

                AudioEngine.SetChannelFrequency(AudioChannelIndex.Pulse1, current.Note);

                timer = current.Seconds;
                index++;

                if (index >= melody.Length)
                {
                    index = Loop ? 0 : melody.Length - 1;
                }
            }
        }
    }
}
