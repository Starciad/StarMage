namespace StarMage.Core.Audio.Channels
{
    internal abstract class Channel
    {
        internal bool Enabled { get; set; }
        internal float Frequency { get; set; }
        internal float Volume { get; set; }

        protected float phase;

        internal Channel()
        {
            this.Enabled = true;
            this.Frequency = 1.0f;
            this.Volume = 1.0f;
        }

        internal abstract float GenerateSample(float deltaTime);
    }
}
