using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

namespace MediaPlayer;

public partial class Settings{
    public static string? Value;
    public static readonly ObservableCollection<string?> Genres = new();

    public Settings() {
        InitializeComponent();
        MGenres.ItemsSource = Genres;
        GetSettings();
        PreviewKeyDown += HandleEsc;
    }

    private void HandleEsc(object sender, KeyEventArgs e) {
        if (e.Key == Key.Escape) {
            Close();
        }
    }

    private static void GetSettings() {
        Properties.Settings.Default.Genres ??= new StringCollection();

        if (Properties.Settings.Default.Genres == null) return;
        {
            foreach (string? genre in Properties.Settings.Default.Genres) {
                Genres.Add(genre);
            }
        }
    }

    public static void SaveGenre() {
        Data.GenreCollection.Clear();
        foreach (var s in Genres) {
            Data.GenreCollection.Add(s);
        }

        Properties.Settings.Default.Genres = Data.GenreCollection;
        Properties.Settings.Default.Save();
    }


    private void removeGenre_btn_Click(object sender, RoutedEventArgs e) {
        if (MGenres.SelectedItem is string str) {
            Genres.Remove(str);
        }
        else {
            MessageBox.Show("Please choose a genre to delete.");
        }

        SaveGenre();
    }

    private void addGenre_btn_Click(object sender, RoutedEventArgs e) {
        if (GenreBox.Text.Length > 0) {
            if (Genres.Contains(GenreBox.Text)) {
                MessageBox.Show("Duplicate");
                return;
            }

            Genres.Add(GenreBox.Text);
            GenreBox.Clear();
        }

        SaveGenre();
    }

    private void mGenres_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
        if (MGenres.SelectedItem is not string str) return;
        Value = str;
        var editWin = new EditGenre();
        editWin.Show();
    }
}