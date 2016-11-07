using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using ParkSquare.Gracenote;

namespace Predvajalnik_v_CSharp
{
    class Metadata_version_2
    {
        //variables
        private string filepath, title, artist, album, link_to_picture, duration, path;
        //other class instances
       
        //constructors
        public Metadata_version_2(string filepath)
        {
            
            this.filepath = filepath;
            FileInfo information = new FileInfo(this.filepath);
            title = trimm_the_info(information.Name,"song");
          


        }
        // properties
        public string Title_of_the_song
        {
            get
            {
                return title;
            }
        }
        public string Artist_of_the_song
        {
            get
            {
                return artist;
            }
        }
        public string Album_of_the_song
        {
            get
            {
                return album;
            }
        }
        public string Duration_of_the_song
        {
            get
            {
                return duration;
            }
        }



        //other class instances
    // SQL class to query the album art link.

        //methods

        static private string trimm_the_info(string some_string_to_trimm, string type_of_metadata )
        {
            if (some_string_to_trimm.Contains("["))
            {
                some_string_to_trimm = some_string_to_trimm.Remove(some_string_to_trimm.IndexOf('['), (some_string_to_trimm.IndexOf(']') -
                some_string_to_trimm.IndexOf('[') + 1));
            }
            if (some_string_to_trimm.Contains("("))
            {
                some_string_to_trimm = some_string_to_trimm.Remove(some_string_to_trimm.IndexOf('('), some_string_to_trimm.IndexOf(')') -
                some_string_to_trimm.IndexOf('(') + 1);
            }
            if (some_string_to_trimm.Contains("Disc"))
            {
                some_string_to_trimm = some_string_to_trimm.Remove(some_string_to_trimm.Length - 7);
            }
            if (some_string_to_trimm.Contains("CD"))
            {
                some_string_to_trimm = some_string_to_trimm.Remove(some_string_to_trimm.Length - 5);
            }
            if (some_string_to_trimm.Contains(".wav") || some_string_to_trimm.Contains(".mp3"))
            {
                some_string_to_trimm = some_string_to_trimm.Remove(some_string_to_trimm.IndexOf('.'), 4);
            }
            if(some_string_to_trimm.Contains(".flac"))
            {
                some_string_to_trimm = some_string_to_trimm.Remove(some_string_to_trimm.IndexOf('.'), 5);
            }
            if (some_string_to_trimm.Contains(',') && type_of_metadata != "pot")
            {
                some_string_to_trimm = some_string_to_trimm.Replace(",", "");
            }
            return some_string_to_trimm;
        }
        static private void get_album_art_cover(string title_of_the_song, string album_of_the_song, string artist_of_the_song)
        {
            try
            {
                


                try
                {
                    var odjemalec = new GracenoteClient("962650182-16615324626BA4A3EC0A5EADD71428E5");
                    var Slika = odjemalec.Search(new SearchCriteria
                    {
                        TrackTitle = title_of_the_song,
                        AlbumTitle = album_of_the_song,
                        Artist = artist_of_the_song,

                        SearchMode = SearchMode.BestMatchWithCoverArt,
                        SearchOptions = SearchOptions.ArtistImage
                    });

                    Slika.Albums.First().Artwork.First().Download(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\AlbumArt\" + album_of_the_song + " " + artist_of_the_song + ".jpg");
                }

                catch
                {

                    //try with other album art providers
                    /*
                     * string link = "http://covers.slothradio.com/?adv=&artist="+izvajalec_meta+"&album="+album_meta;
                     */

                }
                Database query = new Database();
                query.insert_into_or_update(artist_of_the_song, album_of_the_song, "link (to fetch)", "insert");

                var album_cover = new Bitmap(Image.FromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\AlbumArt\" + album_of_the_song + " " + artist_of_the_song + ".jpg"));
        
            }
            catch
            {
                
            }

        }



    }

}
