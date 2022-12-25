using System.Windows;

namespace MediaPlayer;

public partial class EditGenre : Window{
    public EditGenre() {
        InitializeComponent();
    }

    private void Button_ClickSet(object sender, RoutedEventArgs e) {
        if (!Settings.Genres.Contains(Settings.Value)) return;
        Settings.Genres[Settings.Genres.IndexOf(Settings.Value)] = EditBox.Text;
        Settings.SaveGenre();
        Close();
    }
}