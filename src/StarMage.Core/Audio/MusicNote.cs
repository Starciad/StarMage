using StarMage.Core.Enums;

namespace StarMage.Core.Audio
{
    internal readonly struct MusicNote(MidiNote note, float seconds)
    {
        internal readonly MidiNote Note => note;
        internal readonly float Seconds => seconds;
    }
}
