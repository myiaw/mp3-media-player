using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace MediaPlayer{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{
        public MainWindow() {
            InitializeComponent();
            MPlaylist.ItemsSource = Data.Songs;
        }


        private void MenuItemExit_Click(object sender, EventArgs e) => Application.Current.Shutdown();

        private void MenuItemAdd_Click(object sender, EventArgs e) {
            var dlg = new OpenFileDialog {
                Multiselect = true,
                Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"
            };
            var result = dlg.ShowDialog();
            if (result != true) return;
            foreach (string? file in dlg.FileNames) {
                Data.Songs.Add(new Song("Title", "Artist", "Genre", 2000,
                    Song.LoadImage("D:\\Projects\\College\\C#\\MediaPlayer\\MediaPlayer\\Resources\\Media\\images.png"),
                    file));
            }
        }

        private void MenuItemSettings_Click(object sender, EventArgs e) {
            var settingsWin = new Settings();
            settingsWin.Show();
        }

        private void MenuItemRemove_Click(object sender, EventArgs e) {
            if (MPlaylist.SelectedItem is Song song) {
                Data.Songs.Remove(song);
            }
            else {
                MessageBox.Show("Please choose a song to delete.");
            }
        }

        void ClickListViewItem(object sender, MouseEventArgs e) {
            if (MPlaylist.SelectedItem is not Song song) return;
            var editWin = new EditSong();
            Data.SelectedSong = song;
            EditButton.IsEnabled = true;
            editWin.Title = Data.SelectedSong.Title;
            editWin.YearBox.Text = song.ReleaseYear.ToString();
            editWin.ArtistBox.Text = song.Artist;
            editWin.PathBox.Text = song.Path;
            editWin.TitleBox.Text = song.Title;
            editWin.Show();
        }


        private void Button_Click_Play(object sender, RoutedEventArgs e) {
            MusicPlayer.Play();
        }

        private void Button_Click_Pause(object sender, RoutedEventArgs e) {
            MusicPlayer.Pause();
        }

        private void Button_Click_Rewind(object sender, RoutedEventArgs e) {
        }


        private void mPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        private void Button_Click_Image(object sender, MouseButtonEventArgs e) {
            var dlg = new OpenFileDialog {
                Multiselect = false,
                FilterIndex = 1,
                Filter =
                    "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
                    "|PNG Portable Network Graphics (*.png)|*.png" +
                    "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
                    "|BMP Windows Bitmap (*.bmp)|*.bmp" +
                    "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
                    "|GIF Graphics Interchange Format (*.gif)|*.gif"
            };
            var result = dlg.ShowDialog();
            if (result != true) return;
            if (MPlaylist.SelectedItem is Song song) {
                song.Image = Song.LoadImage(dlg.FileName);
            }
            // TODO: FIX ONCLICK IMAGE CHANGE FOR SONG
        }

        private void Button_Enable_Edit(object sender, MouseButtonEventArgs e) {
            EditButton.IsEnabled = true;
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e) {
            ClickListViewItem(sender, null);
        }
    }
}