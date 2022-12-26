using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using MediaPlayer.SettingsWindow;
using Microsoft.Win32;

namespace MediaPlayer{
    /* The MainWindow class is the main window of the application */
    public partial class MainWindow{
        public MainWindow() {
            InitializeComponent();
            MPlaylist.ItemsSource = Data.Songs;
        }

        /// When the user clicks the Exit menu item, the application shuts down
        private void MenuItemExit_Click(object sender, EventArgs e) => Application.Current.Shutdown();

        /// It opens a dialog box that allows the user to select multiple files, and then adds them to the list of songs
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

        /// When the user clicks on the "Settings" menu item, a new window is created and displayed
        private void MenuItemSettings_Click(object sender, EventArgs e) {
            var settingsWin = new Settings();
            settingsWin.Show();
        }

        /// If the selected item is a song, remove it from the list of songs
        private void MenuItemRemove_Click(object sender, EventArgs e) {
            if (MPlaylist.SelectedItem is Song song) {
                Data.Songs.Remove(song);
            }
            else {
                MessageBox.Show("Please choose a song to delete.");
            }
        }

        /// It opens a new window, and fills it with the data of the selected song
        private void ClickListViewItem(object sender, MouseEventArgs e) {
            if (MPlaylist.SelectedItem is not Song song) return;
            var editWin = new EditSong();
            Data.SelectedSong = song;
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


        /// When the user selects a playlist, the edit button is enabled
        private void mPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditButton.IsEnabled = true;
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e) {
            ClickListViewItem(sender, null!);
        }

        private void MenuItemImport_Click(object sender, RoutedEventArgs e) {
            var dlg = new OpenFileDialog {
                Multiselect = true,
                Filter = "XML Files (*.xml)|*.xml"
            };
            var result = dlg.ShowDialog();
            if (result != true) return;
            var serializer = new XmlSerializer(typeof(Song));
            using var writer = new StreamWriter(dlg.FileName);
            serializer.Serialize(writer, Data.Songs);
        }

        private void MenuItemExport_Click(object sender, RoutedEventArgs e) {
            Stream stream ;
            var dlg = new SaveFileDialog {
                Filter = "XML Files (*.xml)|*.xml",
            };
            var result = dlg.ShowDialog();
            if(result != true) return;
            if((stream = dlg.OpenFile()) != null)
            {
                // Code to write the stream goes here.
                stream.Close();
            }
        }
    }
}