using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;


namespace MediaPlayer.SettingsWindow{
    public partial class Settings{
        public static string? Value;
        public static readonly ObservableCollection<string?> Genres = new();

        public Settings() {
            InitializeComponent();
            MGenres.ItemsSource = Genres;
            GetSettings();
            PreviewKeyDown += HandleEsc;
            if (Properties.Settings.Default.SyncFolder is not null) {
                SyncFolderTextBox.Text = Properties.Settings.Default.SyncFolder;
            }

            if (Properties.Settings.Default.TimerValue > 0) {
                setTimeLabel.Content = "Time set to: " + Properties.Settings.Default.TimerValue + " seconds";
            }
            else {
                setTimeLabel.Content = "Time set to: Disabled";
            }
        }

        /// If the key pressed is the escape key, close the window
        private void HandleEsc(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
                Close();
            }
        }

        /// If the Genres property of the Settings object is null, then set it to a new StringCollection object
        /// Add all the items in the Genres property to the Genres ObservableCollection
        private static void GetSettings() {
            Properties.Settings.Default.Genres ??= new StringCollection();

            if (Properties.Settings.Default.Genres == null) return;
            {
                foreach (var genre in Properties.Settings.Default.Genres) {
                    if (Genres.Contains(genre)) continue;
                    Genres.Add(genre);
                }
            }
        }

        /// It saves the Genres list to the GenreCollection list in the Data class, then saves the GenreCollection list to the
        /// Genres property in the Properties.Settings.Default class
        public static void SaveGenre() {
            Data.GenreCollection.Clear();
            foreach (var s in Genres) {
                Data.GenreCollection.Add(s);
            }

            Properties.Settings.Default.Genres = Data.GenreCollection;
            Properties.Settings.Default.Save();
        }


        /// If the selected item in the listbox is a string, then remove it from the listbox
        /// Else it prompts the user to select an item to remove.
        private void removeGenre_btn_Click(object sender, RoutedEventArgs e) {
            if (MGenres.SelectedItem is string str) {
                Genres.Remove(str);
            }
            else {
                MessageBox.Show("Please choose a genre to delete.");
            }

            SaveGenre();
        }

        /// If the textbox is not empty, add the text to the list and clear the textbox.
        private void addGenre_btn_Click(object sender, RoutedEventArgs e) {
            if (GenreBox.Text.Length > 0) {
                if (Genres.Contains(GenreBox.Text)) {
                    MessageBox.Show("Duplicate");
                    return;
                }

                Genres.Add(GenreBox.Text);
                GenreBox.Clear();
            }
            else {
                MessageBox.Show("Please enter a genre.");
                return;
            }

            SaveGenre();
        }

        /// If the user double clicks on a genre in the list, the genre is set as the current value and opens EditGenreWindow
        private void mGenres_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (MGenres.SelectedItem is not string str) return;
            Value = str;
            var editWin = new EditGenre();
            editWin.Show();
        }

        private void SelectSyncFolder_Click(object sender, RoutedEventArgs e) {
            var dlg = (VistaFolderBrowserDialog)Activator.CreateInstance(typeof(VistaFolderBrowserDialog))!;
            var result = dlg.ShowDialog();
            if (result != true) return;
            Properties.Settings.Default.SyncFolder = dlg.SelectedPath;
            Properties.Settings.Default.Save();
            MessageBox.Show("Sync folder set to: " + dlg.SelectedPath +
                            "\n\nThis folder will be used to sync your music library on start, changes will take effect after restart.",
                "Sync folder set", MessageBoxButton.OK);
        }


        private void TimeSetButton_onClick(object sender, RoutedEventArgs e) {
            if (TimerValueBox.Text.Contains('.') || TimerValueBox.Text.Contains(',')) {
                MessageBox.Show("Only whole numbers are allowed.");
                return;
            }

            if (TimerValueBox.Text.Length == 0) {
                MessageBox.Show("Please enter a value.");
            }


            if (!int.TryParse(TimerValueBox.Text, out var time)) return;
            if (time <= 0) {
                MessageBox.Show("The value has to be greater than 0.");
                TimerValueBox.Text = "";
                return;
            }

            Properties.Settings.Default.TimerValue = time;
            Properties.Settings.Default.Save();
            setTimeLabel.Content = "Time set to: " + Properties.Settings.Default.TimerValue + " seconds";
        }

        private void TimeDisableButton_onClick(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.TimerValue = 0;
            Properties.Settings.Default.Save();
            setTimeLabel.Content = "Time set to: Disabled";
        }
    }
}