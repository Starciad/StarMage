namespace StarMage.Core.Audio
{
    internal sealed class Melody(params MusicNote[] notes)
    {
        internal MusicNote[] Notes => notes;
        internal int Length => notes.Length;
        internal MusicNote this[int index] => notes[index];
    }
}
