using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Serialization;
using Blazorise;
using MediaPlayer.SettingsWindow;
using Microsoft.Win32;
using static MediaPlayer.Data;
using MouseButton = System.Windows.Input.MouseButton;


namespace MediaPlayer{
    /* The MainWindow class is the main window of the application */
    public partial class MainWindow{
        private readonly DispatcherTimer _timer;
        private bool _shuffleMode = false;

        public MainWindow() {
            InitializeComponent();
            MPlaylist.Items.Clear();
            MPlaylist.ItemsSource = Songs;
            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
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
                Songs.Add(new Song("Title", "Artist", "Genre", 2000,
                    Song.LoadImage("D:\\Projects\\College\\C#\\MediaPlayer\\MediaPlayer\\Resources\\Media\\images.png"),
                    file, false));
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
                if (SelectedSong is { IsSongPlaying: true }) {
                    MediaElement.Pause();
                    CurrentTitle.Content = "--";
                    SelectedSong.IsSongPlaying = false;
                    ImgPausePlay.Source = new BitmapImage(new Uri("Resources/Icons/play-solid.png", UriKind.Relative));
                }

                Songs.Remove(song);

                if (Songs.Count == 0) {
                    CurrentTitle.Content = "--";
                    EditButton.IsEnabled = false;
                }

                SelectedSong = null;
                ShownImage.ImageSource = null;
            }
            else {
                MessageBox.Show("Please choose a song to delete.");
            }
        }


        /// When the user selects a playlist, the edit button is enabled
        private void mPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (MPlaylist.SelectedItem as Song is { IsSongPlaying: true }) {
                ImgPausePlay.Source =
                    new BitmapImage(new Uri("Resources/Icons/circle-pause-solid.png", UriKind.Relative));
            }
            else {
                ImgPausePlay.Source = new BitmapImage(new Uri("Resources/Icons/play-solid.png", UriKind.Relative));
                EditButton.IsEnabled = true;
            }

            SelectedSong = MPlaylist.SelectedItem as Song;
        }


        /// This function is called when the user clicks the "Edit" button in the context menu. It opens a new window where
        /// the user can edit the song's information
        private void MenuItemEdit_Click(object sender, RoutedEventArgs e) {
            if (SelectedSong is { IsSongPlaying: true }) {
                MediaElement.Pause();
                CurrentTitle.Content = "--";
                SelectedSong.IsSongPlaying = false;
                ImgPausePlay.Source = new BitmapImage(new Uri("Resources/Icons/play-solid.png", UriKind.Relative));
            }


            var editWin = new EditSong {
                Title = SelectedSong?.Title,
                YearBox = {
                    Text = SelectedSong?.ReleaseYear.ToString()
                },
                ArtistBox = {
                    Text = SelectedSong?.Artist
                },
                PathBox = {
                    Text = SelectedSong?.Path
                },
                TitleBox = {
                    Text = SelectedSong?.Title
                }
            };
            editWin.Show();
        }


        /// If shuffle mode is on, then pause the current song, pick a random song from the list of songs, and play it
        private void BtnShuffle_OnClick(object sender, RoutedEventArgs e) {
            _shuffleMode = !_shuffleMode;
            BtnShuffle.Background = _shuffleMode ? Brushes.LightGreen : BtnBackward.Background;
            if (!_shuffleMode) return;
            if (SelectedSong == null) return;
            btnPausePlay_Click(sender, e);
            var rand = new Random().Next(0, Songs.Count);
            if (rand == Songs.IndexOf(SelectedSong)) {
                rand = new Random().Next(0, Songs.Count);
            }

            SelectedSong = Songs[rand];
            btnPausePlay_Click(sender, e);
        }

        /// If the song is playing, pause it, then select the next song in the list, then play it
        private void btnForward_OnClick(object sender, RoutedEventArgs e) {
            btnPausePlay_Click(sender, e);
            if (SelectedSong != null)
                SelectedSong = SelectedSong == Songs.Last() ? Songs.First() : Songs[Songs.IndexOf(SelectedSong) + 1];
            btnPausePlay_Click(sender, e);
        }

        /// When the user clicks the back button, the song pauses, the selected song is set to the previous song in the
        /// list, and the song plays again
        private void btnBackward_OnClick(object sender, RoutedEventArgs e) {
            btnPausePlay_Click(sender, e);
            if (SelectedSong != null) {
                if (SelectedSong != Songs.First()) {
                    SelectedSong = Songs[Songs.IndexOf(SelectedSong) - 1];
                }
            }

            btnPausePlay_Click(sender, e);
        }


        /// If a song is playing, pause it and change the image to a play button. If a song is not playing, play it and
        /// change the image to a pause button
        private void btnPausePlay_Click(object sender, RoutedEventArgs e) {
            foreach (var song in Songs) {
                if (song == SelectedSong || !song.IsSongPlaying) continue;
                CurrentTitle.Content = "--";
                song.IsSongPlaying = false;
                ImgPausePlay.Source = new BitmapImage(new Uri("Resources/Icons/play-solid.png", UriKind.Relative));
            }


            if (SelectedSong is null) {
                MessageBox.Show("Please select a song to play.");
                return;
            }

            MediaSlider.IsEnabled = true;

            if (SavedSong != SelectedSong) {
                if (SelectedSong.Path != null) MediaElement.Source = new Uri(SelectedSong.Path);
            }

            if (SelectedSong.IsSongPlaying) {
                MediaElement.Pause();
                CurrentTitle.Content = "--";
                SelectedSong.IsSongPlaying = false;
                ImgPausePlay.Source = new BitmapImage(new Uri("Resources/Icons/play-solid.png", UriKind.Relative));
            }
            else {
                MediaElement.Play();
                SavedSong = SelectedSong;
                SelectedSong.IsSongPlaying = true;
                if (SelectedSong.Title is { Length: >= 9 }) {
                    CurrentTitle.Content =
                        SelectedSong.Title.Remove(8, SelectedSong.Title.Length - 8).Insert(8, "...");
                }
                else {
                    CurrentTitle.Content = SelectedSong.Title;
                }

                ShownImage.ImageSource = SelectedSong.Image;
                ImgPausePlay.Source =
                    new BitmapImage(new Uri("Resources/Icons/circle-pause-solid.png", UriKind.Relative));
            }
        }

        //SLIDER 

        /// The timer ticks every second and updates the slider and the song counter
        private void Timer_Tick(object? sender, EventArgs e) {
            MediaSlider.Value = MediaElement.Position.TotalSeconds;
            SongCounter.Content = MediaElement.Position.ToString(@"mm\:ss");
        }

        /// When the slider's value changes, the media element's position changes
        private void MediaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (MediaElement.NaturalDuration.HasTimeSpan) {
                MediaElement.Position = TimeSpan.FromSeconds(e.NewValue);
            }
        }


        /// When the song ends, if shuffle mode is on, play a random song. If shuffle mode is off, stop the song
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e) {
            btnPausePlay_Click(sender, e);
            if (_shuffleMode) {
                var rand = new Random().Next(0, Songs.Count);
                if (rand == Songs.IndexOf(SelectedSong)) {
                    rand = new Random().Next(0, Songs.Count);
                }

                SelectedSong = Songs[rand];
                btnPausePlay_Click(sender, e);
                return;
            }


            MediaElement.Stop();
        }

        /// If the media element fails to load the media, display an error message
        private void mediaElement_MediaFailed(object? sender, ExceptionRoutedEventArgs e) {
            MessageBox.Show("Error: " + e.ErrorException.Message);
        }


        /// When the media is opened, set the maximum value of the slider to the length of the media, and start the timer
        private void MediaElement_OnMediaOpened(object sender, RoutedEventArgs e) {
            MediaSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            _timer.Start();
        }

        /// It opens a file dialog, deserializes the XML file, and adds the songs to the list
        private void MenuItemImport_Click(object sender, RoutedEventArgs e) {
            var dlg = new OpenFileDialog {
                Filter = "Import (*.xml)|*.xml",
                DefaultExt = ".xml"
            };
            var result = dlg.ShowDialog();
            if (result != true) return;

            var serializer = new XmlSerializer(typeof(ObservableCollection<SerializableSong>));

            using var reader = new StreamReader(dlg.FileName);
            try {
                var serializableSongs = (ObservableCollection<SerializableSong>)serializer.Deserialize(reader)!;

                foreach (var serializableSong in serializableSongs) {
                    var song = new Song {
                        Title = serializableSong.Title,
                        Artist = serializableSong.Artist,
                        Genre = serializableSong.Genre,
                        Path = serializableSong.Path,
                        ReleaseYear = serializableSong.ReleaseYear,
                        IsSongPlaying = serializableSong.IsSongPlaying
                    };

                    if (true) {
                        var decoder = new PngBitmapDecoder(new MemoryStream(serializableSong.ImageData),
                            BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        song.Image = decoder.Frames[0];
                    }

                    Songs.Add(song);
                }
            }
            catch (InvalidOperationException exception) {
                MessageBox.Show("Invalid Format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// We create a new `SaveFileDialog` object, set its filter to `.xml` files, and show it. If the user selects a
        /// file, we create a new `ObservableCollection<SerializableSong>` object, add all of the songs in the `Songs`
        /// collection to it, create a new `XmlSerializer` object, and use it to serialize the
        /// `ObservableCollection<SerializableSong>` object to the file the user selected
        private void MenuItemExport_Click(object sender, RoutedEventArgs e) {
            var dlg = new SaveFileDialog {
                Filter = "Export (*.xml)|*.xml",
                DefaultExt = ".xml"
            };
            var result = dlg.ShowDialog();
            if (result != true) return;

            var serializableSongs = new ObservableCollection<SerializableSong>();
            foreach (Song song in Songs) {
                serializableSongs.Add(new SerializableSong(song));
            }

            var serializer = new XmlSerializer(typeof(ObservableCollection<SerializableSong>));

            using var writer = new StreamWriter(dlg.FileName);
            serializer.Serialize(writer, serializableSongs);
        }

        private void Menu_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                DragMove();
            }
        }
    }
}