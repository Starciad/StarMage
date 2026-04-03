using Microsoft.Xna.Framework;

using StarMage.Core.Enums;

namespace StarMage.Core.Audio
{
    internal static class SoundEffectPlayer
    {
        private static SoundEffect soundEffect;
        private static float timer;

        internal static void SetSoundEffect(SoundEffect soundEffect)
        {
            SoundEffectPlayer.soundEffect = soundEffect;
            timer = soundEffect.Seconds;
        }

        internal static void Update(GameTime gameTime)
        {
            if (soundEffect == null || timer <= 0.0f)
            {
                return;
            }

            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            float progress = 1f - (timer / soundEffect.Seconds);
            float frequency = MathHelper.Lerp(soundEffect.FrequencyStart, soundEffect.FrequencyEnd, progress);

            AudioEngine.SetChannelFrequency(AudioChannelIndex.Pulse2, frequency);
            AudioEngine.SetChannelVolume(AudioChannelIndex.Pulse2, 1.0f);
        }
    }
}
