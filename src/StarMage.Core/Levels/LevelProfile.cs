namespace StarMage.Core.Levels
{
    internal readonly struct LevelProfile(int horizontalChunkCount)
    {
        internal readonly int HorizontalChunkCount => horizontalChunkCount;
    }
}
