using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaPlayer{
    internal class Song : INotifyPropertyChanged{
        private string? _title;
        private string? _artist;
        private string? _genre;
        private string? _path;
        private ImageSource? _image;
        private int _releaseYear;

        public string? Title
        {
            get => _title;
            set {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public ImageSource? Image
        {
            get => _image;
            set {
                _image = value;
                NotifyPropertyChanged("Image");
            }
        }

        public string? Artist
        {
            get => _artist;
            set {
                _artist = value;
                NotifyPropertyChanged("Artist");
            }
        }

        public string? Genre
        {
            get => _genre;
            set {
                _genre = value;
                NotifyPropertyChanged("Genre");
            }
        }

        public int ReleaseYear
        {
            get => _releaseYear;
            set {
                _releaseYear = value;
                NotifyPropertyChanged("ReleaseYear");
            }
        }


        public string? Path
        {
            get => _path;
            set {
                _path = value;
                NotifyPropertyChanged("Path");
            }
        }

        public Song(string? title, string? artist, string? genre, int releaseYear, ImageSource? image, string? path) {
            Title = title;
            Artist = artist;
            Genre = genre;
            ReleaseYear = releaseYear;
            Image = image;
            Path = path;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string attribute) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(attribute));
        }

        public static ImageSource LoadImage(string fileName) {
            var image = new BitmapImage();

            using var stream = new FileStream(fileName, FileMode.Open);
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();

            return image;
        }
    }
}