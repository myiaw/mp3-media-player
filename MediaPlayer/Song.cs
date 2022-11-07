using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.ComponentModel;
using System.Xml.Linq;

namespace MediaPlayer01
{
    internal class Song : INotifyPropertyChanged
    {
        private String title;
        private String artist;
        private String genre;
        private string path;
        private int releaseYear;
        private int coverPicture;   

        public string Title
        {
            get { return title; }
            set {
                title = value;
                NotifyPropertyChanged("Title"); 
            }
        }
        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                NotifyPropertyChanged("Artist");
            }
        }

        public string Genre
        {
            get { return genre; }
            set
            {
                genre = value;
                NotifyPropertyChanged("Genre");
            }
        }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                NotifyPropertyChanged("Path");
            }
        }
        public int CoverPicture
        {
            get { return coverPicture; }
            set
            {
                coverPicture = value;
                NotifyPropertyChanged("CoverPicture");
            }
        }
        public int ReleaseYear {
            get { return releaseYear; }
            set {
                releaseYear = value;
                NotifyPropertyChanged("ReleaseYear");
            }
        }
        public Song(string Title, string Artist, string Genre, string Path, int CoverPicture, int ReleaseYear)
        {
            this.Title = Title;
            this.Artist = Artist;
            this.Genre = Genre;
            this.Path = Path;
            this.CoverPicture = CoverPicture;
            this.ReleaseYear = ReleaseYear;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string attribute)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(attribute));
            }
        }



    }
}
