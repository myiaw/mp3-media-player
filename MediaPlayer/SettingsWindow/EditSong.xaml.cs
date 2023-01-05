using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using TagLib;

namespace MediaPlayer.SettingsWindow{
    /// <summary>
    /// Interaction logic for editSongMain.xaml
    /// </summary>
    public partial class EditSong{
        public EditSong() {
            InitializeComponent();
        }


        /// When the text in the YearBox changes, the release year of the selected song is changed to the new text
        private void YearBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong == null) return;
            var ind = Data.Songs.IndexOf(Data.SelectedSong);
            Data.Songs[ind].ReleaseYear = int.Parse(YearBox.Text);
        }


        /// When the text in the title box changes, the title of the song is changed to the text in the title box.
        /// Title is also changed to song title in the song list.
        private void TitleBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong == null) return;
            Data.Songs[Data.Songs.IndexOf(Data.SelectedSong)].Title = TitleBox.Text;
            Title = TitleBox.Text;
        }

        /// When the text in the PathBox changes, the path of the selected song is changed to the new text.
        private void PathBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong == null) return;
            Data.Songs[Data.Songs.IndexOf(Data.SelectedSong)].Path = PathBox.Text;
        }


        /// When the text in the ArtistBox changes, the artist of the selected song is changed to the new text.
        private void ArtistBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong == null) return;
            Data.Songs[Data.Songs.IndexOf(Data.SelectedSong)].Artist = ArtistBox.Text;
        }

        /// When the user selects a new genre from the dropdown menu, the genre of the selected song is updated to the new
        /// genre from all genres saved in user settings.
        private void GenreComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (Data.SelectedSong == null) return;
            var selectedGenre = sender as ComboBox;
            var ind = Data.Songs.IndexOf(Data.SelectedSong);
            Data.Songs[ind].Genre = selectedGenre?.SelectedItem as string;
        }

        private void comboBox1_Loaded(object sender, RoutedEventArgs e) {
            if (sender is not ComboBox combo) return;
            combo.ItemsSource = Properties.Settings.Default.Genres;
            combo.SelectedIndex = 0;
        }

        /// It opens a file dialog, and if the user selects a file, it loads the image into the selected song
        private void Image_Change(object sender, RoutedEventArgs e) {
            Song.ImageChange();
        }
    }
}