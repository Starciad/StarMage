namespace StarMage.Core.Audio.Channels
{
    internal sealed class NoiseChannel : Channel
    {
        private uint shiftRegister = 1;

        internal override float GenerateSample(float deltaTime)
        {
            if (!Enabled || Volume <= 0f || Frequency <= 0f)
                return 0f;

            uint bit = (this.shiftRegister ^ (this.shiftRegister >> 1)) & 1;
            this.shiftRegister = (this.shiftRegister >> 1) | (bit << 14);

            return ((this.shiftRegister & 1) == 1 ? 1.0f : -1.0f) * this.Volume;
        }
    }
}
