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
        public int ReleaseYear
        {
            get { return releaseYear; }
            set
            {
                releaseYear = value;
                NotifyPropertyChanged("ReleaseYear");
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
        public Song(string Title, string Artist, string Genre, int ReleaseYear, int CoverPicture)
        {
            this.Title = Title;
            this.Artist = Artist;
            this.Genre = Genre;
            this.ReleaseYear = ReleaseYear;
            this.CoverPicture = CoverPicture;
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
