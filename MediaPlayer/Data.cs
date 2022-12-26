using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace MediaPlayer {
    internal static class Data {
        public static readonly ObservableCollection<Song> Songs = new();
        public static readonly StringCollection GenreCollection = new();
        public static Song? SelectedSong;
    }
}