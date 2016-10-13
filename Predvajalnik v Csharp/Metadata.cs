using System;
using System.IO;
using System.Linq;
using ParkSquare.Gracenote;


namespace Predvajalnik_v_CSharp
{
    class Metadata
    {
        //SPREMENLJIVKE
        private string audio_file;
        //LASTNOSTI
        public string Meta_podatki
        {
            get { return audio_file; }
            set { audio_file = metapodatki(value); }
        }
        public string Album_art
        {
            get { return audio_file; }
            set { audio_file = Prenos_slike(value); }
        }
        //METODE
        private string metapodatki(string datoteka)
        {
            TagLib.File a_dat = TagLib.File.Create(datoteka);
            if (a_dat.Tag.Title != null)
            {
                datoteka = a_dat.Tag.Title + ",";
            }
            if (datoteka.Contains(".wav") || a_dat.Tag.Title == null)
            {
                datoteka = trim_albuma(Path.GetFileName(datoteka), "pot") + ",";
            }
            if (a_dat.Tag.FirstPerformer != null)
            {
                datoteka += a_dat.Tag.FirstPerformer + ",";
            }
            else
            {
                datoteka += "Neznano,";
            }
            if (a_dat.Tag.Album != null)
            {
                datoteka += trim_albuma(a_dat.Tag.Album, "album") + ",";
            }
            else
            {
                datoteka += "Neznano,";
            }
            if (a_dat.Properties.Duration.ToString(@"hh\:mm\:ss") != null)
            {
                datoteka += a_dat.Properties.Duration.ToString(@"hh\:mm\:ss");
            }
            //Programiranje v C# 35
            return datoteka;
        }//funkcija za vračanje metapodatkov
        private string trim_albuma(string trimm, string type)
        {
            if (trimm.Contains("["))
            {
                trimm = trimm.Remove(trimm.IndexOf('['), (trimm.IndexOf(']') -
                trimm.IndexOf('[') + 1));
            }
            if (trimm.Contains("("))
            {
                trimm = trimm.Remove(trimm.IndexOf('('), (trimm.IndexOf(')') -
                trimm.IndexOf('(') + 1));
            }
            if (trimm.Contains("Disc"))
            {
                trimm = trimm.Remove(trimm.Length - 7);
            }
            if (trimm.Contains("CD"))
            {
                trimm = trimm.Remove(trimm.Length - 5);
            }
            if (trimm.Contains(".wav") || trimm.Contains(".mp3"))
            {
                trimm = trimm.Remove(trimm.IndexOf('.'), 4);
            }
            if (trimm.Contains(',') && type != "pot")
            {
                trimm = trimm.Replace(",", "");
            }
            return trimm;
        }//funckija za odrstanjevanje znakov
        private string Prenos_slike(string datoteka)
        {
            string album_meta = datoteka.Split(',')[0];
            string izvajalec_meta = datoteka.Split(',')[1];
            string ime_skladbe_meta = datoteka.Split(',')[2];
            string pot =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music
Player\AlbumArt\" + album_meta + " " + izvajalec_meta + ".jpg";
            if (!File.Exists(pot))
            {
                try
                { //import Gracenote package
                    var odjemalec = new GracenoteClient("962650182-16615324626BA4A3EC0A5EADD71428E5");
                    var Slika = odjemalec.Search(new SearchCriteria
                    {
                        TrackTitle = ime_skladbe_meta,
                        AlbumTitle = album_meta,
                        Artist = izvajalec_meta,
                        SearchMode = SearchMode.BestMatchWithCoverArt,
                        SearchOptions = SearchOptions.ArtistImage
                    });
                    Slika.Albums.First().Artwork.First().Download(pot); //0 album, 1
                                                                        // artist, 3 je lokacijađ
                }
                catch
                {
                    pot = "Privzeto";
                }
            }
            return pot;
        }//funkcija za prenos slike albuma
    }
}
