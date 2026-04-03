namespace StarMage.Core.Audio.Channels
{
    internal sealed class PulseChannel : Channel
    {
        internal float Duty { get; set; }

        internal PulseChannel() : base()
        {
            this.Duty = 0.5f;
        }

        internal override float GenerateSample(float deltaTime)
        {
            if (!Enabled || Volume <= 0f || Frequency <= 0f)
                return 0f;

            this.phase += this.Frequency * deltaTime;
            this.phase %= 1.0f;

            return (this.phase < this.Duty ? 1.0f : -1.0f) * this.Volume;
        }
    }
}
