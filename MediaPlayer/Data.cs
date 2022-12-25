using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace MediaPlayer {
    internal class Data {
        public static ObservableCollection<Song> Songs = new();
        public static StringCollection GenreCollection = new();
        public static Song? SelectedSong;
    }
}