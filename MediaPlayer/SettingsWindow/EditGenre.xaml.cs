using System.Windows;

namespace MediaPlayer.SettingsWindow;

public partial class EditGenre{
    public EditGenre() {
        InitializeComponent();
    }

    /// If the value in the edit box is not already in the list of genres, then add it to the list and save the list
    private void Button_ClickSet(object sender, RoutedEventArgs e) {
        if (!Settings.Genres.Contains(Settings.Value)) return;
        Settings.Genres[Settings.Genres.IndexOf(Settings.Value)] = EditBox.Text;
        Settings.SaveGenre();
        Close();
    }
}