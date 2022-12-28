using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace MediaPlayer;

/* It's a class that contains all the properties of a Song class, but it's serializable
    --> Has ImageData that stores a byte array instead of an ImageSource */
[XmlRoot("Song")]
public class SerializableSong{
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Genre { get; set; }
    public string? Path { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsSongPlaying { get; set; }
    public byte[] ImageData { get; set; }

    public SerializableSong() {
    }

    public SerializableSong(Song song) {
        Title = song.Title;
        Artist = song.Artist;
        Genre = song.Genre;
        Path = song.Path;
        ReleaseYear = song.ReleaseYear;
        IsSongPlaying = song.IsSongPlaying;

        if (song.Image != null) {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)song.Image));

            using (var stream = new MemoryStream()) {
                encoder.Save(stream);
                ImageData = stream.ToArray();
            }
        }
    }
}