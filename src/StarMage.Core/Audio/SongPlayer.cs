using Microsoft.Xna.Framework.Media;

using StarMage.Core.Databases;
using StarMage.Core.Enums;

namespace StarMage.Core.Audio
{
    internal static class SongPlayer
    {
        internal static Song CurrentSong { get; private set; }
        internal static SongIndex CurrentSongIndex { get; private set; }

        internal static void Play(SongIndex songIndex)
        {
            CurrentSong = AssetDatabase.GetSong(songIndex);
            CurrentSongIndex = songIndex;

            MediaPlayer.Stop();
            MediaPlayer.Play(CurrentSong);
        }
    }
}
