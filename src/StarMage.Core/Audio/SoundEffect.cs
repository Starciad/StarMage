namespace StarMage.Core.Audio
{
    internal sealed class SoundEffect(float frequencyStart, float frequencyEnd, float seconds)
    {
        internal float FrequencyStart => frequencyStart;
        internal float FrequencyEnd => frequencyEnd;
        internal float Seconds => seconds;
    }
}
