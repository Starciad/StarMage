using StarMage.Core.Audio.Channels;

namespace StarMage.Core.Audio
{
    internal static class APU
    {
        internal static PulseChannel Pulse1 = new();
        internal static PulseChannel Pulse2 = new();
        internal static TriangleChannel Triangle = new();
        internal static NoiseChannel Noise = new();

        internal static float MasterVolume = 0.1f;

        internal static float GenerateSample(float deltaTime)
        {
            float p1 = Pulse1.GenerateSample(deltaTime);
            float p2 = Pulse2.GenerateSample(deltaTime);
            float t = Triangle.GenerateSample(deltaTime);
            float n = Noise.GenerateSample(deltaTime);

            float mix = p1 + p2 + t + n;

            return mix * MasterVolume;
        }
    }
}
