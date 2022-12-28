using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace MediaPlayer{
    [XmlRoot("Song")]
    public class Song : INotifyPropertyChanged{
        private string? _title;
        private string? _artist;
        private string? _genre;
        private string? _path;
        private int _releaseYear;
        private bool _isIsSongPlaying;
        private ImageSource? _image;

        public Song() {
        }


        public bool IsSongPlaying
        {
            get => _isIsSongPlaying;
            set {
                _isIsSongPlaying = value;
                NotifyPropertyChanged("IsSongPlaying");
            }
        }

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

        public Song(string? title, string? artist, string? genre, int releaseYear, ImageSource? image, string? path,
            bool isSongPlaying) {
            Title = title;
            Artist = artist;
            Genre = genre;
            ReleaseYear = releaseYear;
            Image = image;
            Path = path;
            IsSongPlaying = isSongPlaying;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string attribute) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(attribute));
        }

        public static ImageSource? LoadImage(string fileName) {
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