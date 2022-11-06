using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MediaPlayer01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel viewModel;

        public MainWindow()

        {
            InitializeComponent();
            this.viewModel = new ViewModel
            {
                Playlist = new ObservableCollection<Song>
                {
                }
            };
            this.DataContext = this.viewModel;
        }


        private void MenuItemExit_Click(object sender, System.EventArgs e) => Application.Current.Shutdown();

        private void MenuItemAdd_Click(object sender, System.EventArgs e) => this.viewModel.Playlist.Add(new Song("addedTitle", "addedArtist", "addedGenre", 2001, 1));

        private void MenuItemRemove_Click(object sender, System.EventArgs e)
        {

            if (this.viewModel.Playlist.Count == 0)
            {
                MessageBox.Show("No songs to remove.");
                return;
            }
            if (this.viewModel.SelectedSong == null)
            {
                MessageBox.Show("No selected song.");
            }

            Song? selected = this.viewModel.SelectedSong;

            this.viewModel.Playlist.Remove(selected);

        }
        void ClickListViewItem(object sender, MouseEventArgs e) => MessageBox.Show("Artist: " + this.viewModel.SelectedSong.Artist + "\n" + "Song title: " + this.viewModel.SelectedSong.Title + "\nGenre: " + this.viewModel.SelectedSong.Genre + "\nRelease Year: " + this.viewModel.SelectedSong.ReleaseYear.ToString() + "\nCover Picture: "+ this.viewModel.SelectedSong.CoverPicture.ToString() + "\n") ;

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_Play(object sender, RoutedEventArgs e)
        {
            musicPlayer.Play();

        }

        private void Button_Click_Pause(object sender, RoutedEventArgs e)
        {
            musicPlayer.Pause();
        }

        private void Button_Click_Rewind(object sender, RoutedEventArgs e)
        {

        }
    }

}


