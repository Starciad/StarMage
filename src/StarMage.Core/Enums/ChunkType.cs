namespace StarMage.Core.Enums
{
    /// <summary>
    /// Represents the type of a chunk within the game world.
    /// Used to categorize different areas and their purposes.
    /// </summary>
    internal enum ChunkType : byte
    {
        /// <summary>
        /// The chunk type is unknown or not specified.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The starting chunk where the player begins.
        /// </summary>
        Beginning = 1,

        /// <summary>
        /// A standard chunk with no special properties.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// The ending chunk, typically marking the level's completion.
        /// </summary>
        Ending = 3,

        /// <summary>
        /// The final chunk of the map is designated for a boss battle.
        /// </summary>
        Arena = 4,
    }
}
