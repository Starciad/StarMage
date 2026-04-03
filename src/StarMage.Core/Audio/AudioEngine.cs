using Microsoft.Xna.Framework.Audio;

using StarMage.Core.Enums;

using System;

namespace StarMage.Core.Audio
{
    internal static class AudioEngine
    {
        private static readonly DynamicSoundEffectInstance dynamicSoundEffectInstance = new(44_100, AudioChannels.Mono);
        private static readonly byte[] buffer = new byte[BUFFER_SIZE * 2]; // 16-bit

        private const int BUFFER_SIZE = 1024;

        internal static void Initialize()
        {
            dynamicSoundEffectInstance.Play();

            APU.Pulse1.Volume = 0.0f;
            APU.Pulse2.Volume = 0.0f;
            APU.Triangle.Volume = 0.0f;
            APU.Noise.Volume = 0.0f;

            APU.Pulse1.Enabled = false;
            APU.Pulse2.Enabled = false;
            APU.Triangle.Enabled = false;
            APU.Noise.Enabled = false;
        }

        private static void FillBuffer()
        {
            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                float sample = APU.GenerateSample(1f / 44_100f);

                short pcm = (short)(sample * short.MaxValue);

                buffer[i * 2] = (byte)(pcm & 0xFF);
                buffer[i * 2 + 1] = (byte)((pcm >> 8) & 0xFF);
            }

            dynamicSoundEffectInstance.SubmitBuffer(buffer);
        }

        internal static void Update()
        {
            while (dynamicSoundEffectInstance.PendingBufferCount < 3)
            {
                FillBuffer();
            }
        }

        internal static void Dispose()
        {
            dynamicSoundEffectInstance.Dispose();
        }

        internal static float NoteToFrequency(MidiNote note)
        {
            return 440.0f * (float)Math.Pow(2, ((int)note - 69) / 12.0f);
        }

        internal static void SetChannelFrequency(AudioChannelIndex channelIndex, float frequency)
        {
            switch (channelIndex)
            {
                case AudioChannelIndex.Pulse1:
                    APU.Pulse1.Frequency = frequency;
                    break;
                case AudioChannelIndex.Pulse2:
                    APU.Pulse2.Frequency = frequency;
                    break;
                case AudioChannelIndex.Triangle:
                    APU.Triangle.Frequency = frequency;
                    break;
                case AudioChannelIndex.Noise:
                    APU.Noise.Frequency = frequency;
                    break;
                default:
                    break;
            }
        }

        internal static void SetChannelFrequency(AudioChannelIndex channelIndex, MidiNote note)
        {
            SetChannelFrequency(channelIndex, NoteToFrequency(note));
        }

        internal static void SetChannelVolume(AudioChannelIndex channelIndex, float volume)
        {
            switch (channelIndex)
            {
                case AudioChannelIndex.Pulse1:
                    APU.Pulse1.Volume = volume;
                    break;
                case AudioChannelIndex.Pulse2:
                    APU.Pulse2.Volume = volume;
                    break;
                case AudioChannelIndex.Triangle:
                    APU.Triangle.Volume = volume;
                    break;
                case AudioChannelIndex.Noise:
                    APU.Noise.Volume = volume;
                    break;
                default:
                    break;
            }
        }
    }
}
