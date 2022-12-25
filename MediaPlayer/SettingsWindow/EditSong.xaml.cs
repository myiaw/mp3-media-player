using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MediaPlayer;


namespace MediaPlayer{
    /// <summary>
    /// Interaction logic for editSongMain.xaml
    /// </summary>
    public partial class EditSong : Window{
        public EditSong() {
            InitializeComponent();
        }


        private void YearBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong != null) {
                int ind = Data.Songs.IndexOf(Data.SelectedSong);
                Data.Songs[ind].ReleaseYear = int.Parse(YearBox.Text);
            }
        }

        private void TitleBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong != null) {
                int ind = Data.Songs.IndexOf(Data.SelectedSong);
                Data.Songs[ind].Title = TitleBox.Text;
                Title = TitleBox.Text;
            }
        }

        private void PathBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong != null) {
                int ind = Data.Songs.IndexOf(Data.SelectedSong);
                Data.Songs[ind].Path = PathBox.Text;
            }
        }


        private void ArtistBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (Data.SelectedSong != null) {
                int ind = Data.Songs.IndexOf(Data.SelectedSong);
                Data.Songs[ind].Artist = ArtistBox.Text;
            }
        }

        private void GenreComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (Data.SelectedSong != null) {
                var selectedGenre = sender as ComboBox;
                int ind = Data.Songs.IndexOf(Data.SelectedSong);
                Data.Songs[ind].Genre = selectedGenre?.SelectedItem as String;
            }
        }

        private void comboBox1_Loaded(object sender, RoutedEventArgs e) {
            if (sender is not ComboBox combo) return;
            combo.ItemsSource = Properties.Settings.Default.Genres;
            combo.SelectedIndex = 0;
        }
    }
}