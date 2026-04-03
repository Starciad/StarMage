using System;

namespace StarMage.Core.Audio.Channels
{
    internal sealed class TriangleChannel : Channel
    {
        internal override float GenerateSample(float deltaTime)
        {
            if (!Enabled || Volume <= 0f || Frequency <= 0f)
                return 0f;

            this.phase += this.Frequency * deltaTime;
            this.phase %= 1.0f;

            float value = (2.0f * Math.Abs((2.0f * this.phase) - 1.0f)) - 1.0f;

            return value * this.Volume;
        }
    }
}
