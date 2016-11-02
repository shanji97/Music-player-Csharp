using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using ParkSquare.Gracenote;

namespace Predvajalnik_v_CSharp
{
    class Metadata_version_2
    {
        //variables
        private string filepath, title, artist, album, link_to_picture, duration, path;

        //constructors
        public Metadata_version_2(string filepath)
        {
            this.filepath = filepath;
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
        Database query = new Database(); // SQL class to query the album art link.

        //methods
        private string get_metadata(string filename)
        {
            return filename;
        }

        private void get_album_art_cover()
        {

        }



    }

}
