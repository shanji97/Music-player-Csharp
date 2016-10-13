using System;
using System.IO;
using System.Net;
using System.Text;


namespace Predvajalnik_v_CSharp
{ 
    class Metapodatki
    {






        //SPREMENLJIVKE
        private string audio_file;
        private string link = "";


        //LASTNOSTI
        public string Meta_podatki
        {
            get { return audio_file; }
            set { audio_file =metapodatki(value); }
        }
        public string pridobitev_slike
        {
            get { return audio_file; }
            set {  audio_file= pridobi_sliko(value); }
        }
        
       
        //METODE
        private string metapodatki(string datoteka)
        {
            TagLib.File a_dat = TagLib.File.Create(datoteka);
            
            datoteka = a_dat.Tag.Title + "," + a_dat.Tag.FirstPerformer + "," + trim_albuma(a_dat.Tag.Album) + "," + a_dat.Properties.Duration.ToString(@"hh\:mm\:ss");
            return datoteka;
       }//funckija za vračanje metapodatkov 
       private string pridobi_sliko(string slicica)
        {
          

         
                HttpWebRequest zahtevek_za_sliko = (HttpWebRequest)WebRequest.Create("http://covers.slothradio.com/?adv=&artist=" + slicica.Split(',')[0] + "&album=" + slicica.Split(',')[1]);
                zahtevek_za_sliko.Method = "GET";
                WebResponse odgovor_za_sliko = zahtevek_za_sliko.GetResponse();
                StreamReader branje_iz_html_datoteke = new StreamReader(odgovor_za_sliko.GetResponseStream(), Encoding.UTF8);

                StreamWriter zapis_v_fajl = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\slika.txt");
                slicica = branje_iz_html_datoteke.ReadToEnd();
                zapis_v_fajl.WriteLine(slicica);
                branje_iz_html_datoteke.Close();
                odgovor_za_sliko.Close();
                branje_iz_html_datoteke.Close();

                if (!(slicica.Contains("album0")))
                {
                    slicica = "0";
                }
                else
                {

/*


                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(link, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\");
                    }
                    */

                }

            
          

            return slicica;
        }//funkcija za pridobivanje slike
        private string trim_albuma(string trimm)
        {


            if (trimm.Contains("["))
            {
              trimm = trimm.Remove(trimm.IndexOf('['), (trimm.IndexOf(']') - trimm.IndexOf('[') + 1));
            }
             if(trimm.Contains("("))
            {
              trimm = trimm.Remove(trimm.IndexOf('('), (trimm.IndexOf(')') - trimm.IndexOf('(') + 1));
            }
             if(trimm.Contains("Disc"))
            {
              trimm = trimm.Remove(trimm.Length - 7);
            }
             if(trimm.Contains("CD"))
            {
              trimm = trimm.Remove(trimm.Length - 5);
            }
                   
            return trimm;
        }//funckija za trimmmanje znakov (za lažje pridobitev albuma)

    }
}