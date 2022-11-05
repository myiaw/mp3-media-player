using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer01
{
    internal class ViewModel
    {
        public ObservableCollection<Song>? Playlist { get; set; }
        public Song? SelectedSong { get; set; }
    }
}
