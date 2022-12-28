using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;


namespace MediaPlayer{
    /* It's a static class that contains a collection of songs, a collection of genres, and two properties that are used to
    store the currently selected song and the song that was last saved */
    internal static class Data{
        public static readonly ObservableCollection<Song> Songs = new();
        public static readonly StringCollection GenreCollection = new();
        public static Song? SelectedSong;
        public static Song? SavedSong;
    }
}