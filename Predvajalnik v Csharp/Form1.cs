using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Predvajalnik_v_CSharp
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            trackBar1.TickFrequency = 1;
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 2;
            trackBar1.Visible = false;
            timer1.Interval = 1000;

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\slika.txt"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\slika.txt");
            }
            //Checking if file exists (for future implementations when Gracenote will be deleted).
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player"))//preveri ce obstaja imenik z imenom Media Player
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player"); //naredimo imenik za media player
            }

            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite"))// preverimo,če na tej lokaciji osbtaja že kaka baza
            {

               
               
                new Database().create_db();// new SQLITE.n//Creating a DB for the links of the album covers that will be downloaded, if the DB doesn't exist. 
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 2)
            {
                odpri();
            }
        }
        //GLOBAL VARIABLES
        List<string> s = new List<string>(); //A list witch  contains paths of all the music files that we port them in the program.
    Metadata metapodatki = new Metadata(); //A new object of the metadata class.
       
       Playback glasba = new Playback();   // This will do the "play music thing".

        private string globalni_string = ""; 
        bool playing = false;
        bool ponovi = false;
        bool nakljucno = false;
        short klik = 0, sekunde = 0, index = 0, s_nakljucno = 0, s_ponovi = 0; //The index will serve to determine the index of the audio file in the "skladba" collection.              

        //Buttons
        //The "About" section
        private void oProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }
        //Opening the menu openfile dialog
        private void skladbeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            odpri();
        }
        private void audioDatotekaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            odpri();
        }
        private void dolzina_Click(object sender, EventArgs e)
        {
            odpri();
        }
        private void izhodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Checking and keeping an eye on the duration of a song 
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan cas = TimeSpan.FromSeconds(trackBar1.Value);
            globalni_string = cas.ToString(@"hh\:mm\:ss");
            if (globalni_string != dolzina.Text)
            {
                sekunde++;
                trackBar1.Value++;
            }
            else
            {
                glasba.stop_music();
                timer1.Stop();
                trackBar1.Value = 0;
                sekunde = 0;
                if (ponovi == true)
                {
                    predvajaj(s[index]);
                }
                else
                {
                    if (nakljucno == true)
                    {
                        index = Convert.ToInt16(new Random().Next(0,
                        listBox1.Items.Count));
                    }
                    else
                    {
                        if ((index + 1) == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    predvajaj(s[index]);
                }
            }
            cas = TimeSpan.FromSeconds(sekunde);
            p_cas.Text = cas.ToString(@"hh\:mm\:ss");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            s_ponovi++;
            if (s_ponovi % 2 == 1)
            {
                ponovi = true;
             
            }
            else
            {
              ponovi = false;
            }
        }
        //On button click music stops and one song before starts to play
        private void button1_Click(object sender, EventArgs e)
        {
            Funkcije_gumbov("Nazaj");
        }
        //On button click music stops and one song before starts to play
        private void button2_Click(object sender, EventArgs e)
        {
            Funkcije_gumbov("Naprej");
        }
        //Play/STOP button (STOP is gonna be separated and replaced pause):
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                odpri();
            }
            else
            {
                if (!playing)
                {
                    predvajaj(s[index]);
                }
                else
                {
                 glasba.pause_song();
                 timer1.Stop();
                 playing = false;
                 
                }
            }
        }
        //On listbox click the song will be changed
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            trackBar1.Value = 0;
            timer1.Stop();
            Funkcije_gumbov("Listbox");
        }
        //The trackbar sets the time property to the part of the song we scrolled, returns us the time and seeks the position in the song 
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
          p_cas.Text = Convert.ToInt16(trackBar1.Value).ToString(@"hh\:mm\:ss");
          glasba.isci(Convert.ToInt16(trackBar1.Value) * 1000);
        }
        //Enables the random play functionality
        private void button5_Click(object sender, EventArgs e)
        {
            s_nakljucno++;
            if (s_nakljucno % 2 == 1)
            {
                nakljucno = true;
              //  button5.Image = Resource1.nakljucno_ne;
            }
            else
            {
            //    button5.Image = Resource1.nakljucno;
                nakljucno = false;
            }
        }
        //Fuctions
        //The function opens the Open file dialog, initializes a string array that helds the path to the music files,
        //loads them into the collection, plays the first song 
        private void odpri()
        {
            string[] a_datoteke;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "MP3|*.mp3|WAV|*.wav|FLAC|*.flac|Vse datoteke |*.* ";
            openFileDialog1.Title = "Izberite več audio datotek (Vsaj 2 datoteki).";
            openFileDialog1.Multiselect = true;
            openFileDialog1.InitialDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("Izbiranje glasbe preklicano!", "Preklicano!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (listBox1.Items.Count == 0)
                {
                    album.Text = izvajalec.Text = naslov.Text = "Unknown";
                }
            }
            else
            {
                try
                {
                    a_datoteke = openFileDialog1.FileNames;
                    s.AddRange(a_datoteke);
                    predvajaj(s[0]);
                    listBox1.Items.Clear();
                    Napolni_lisbox(s);
                    Array.Clear(a_datoteke, 0, a_datoteke.Length);
                    klik++;
                    trackBar1.Visible = true;
                }
                catch (Exception izjema)
                {
                    MessageBox.Show("The file could be not opened" +Environment.NewLine + "Razlog: " + izjema.ToString(), "Error!", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
        //The function gets the current playing file path and retrieves metadata from it. It also sets the album art cover.
        private void metapodatek(string datoteka_za_metapodatke)
        {
            Bitmap slika;
            metapodatki.Meta_podatki = datoteka_za_metapodatke;
            datoteka_za_metapodatke = metapodatki.Meta_podatki;
            ushort id = 0;
            Label[] oznake = new Label[] { naslov, izvajalec, album, dolzina };
            foreach (Label oznaka in oznake)
            {
                oznaka.Text = datoteka_za_metapodatke.Split(',')[id];
                id++;
            }
    //        
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\Music Player\AlbumArt\" + album.Text + " " + izvajalec.Text + ".jpg"))
            {
                slika = new Bitmap(Image.FromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ @"\Music Player\AlbumArt\" + album.Text + " " + izvajalec.Text + ".jpg"), new
                Size(120, 120));
                pictureBox1.Image = slika;
         
                //change this part of the code  to > if there is no album art cover look for an entry in the link in the database
                //and fetcj the link from there > i'll rewrite the database class as soon as i can
            }
            else 
            {
                if (Preveri_povezavo() && album.Text != "Neznano" && izvajalec.Text != "Neznano")
                {
                    metapodatki.Album_art = album.Text + "," + izvajalec.Text + "," +
                    naslov.Text;
                    if (metapodatki.Album_art != "Privzeto")
                    {
                        try
                        {
                            slika = new Bitmap(Image.FromFile(metapodatki.Album_art), new Size(120, 120));
                            pictureBox1.Image = slika;
                        }
                        catch (Exception a)
                        {
                            MessageBox.Show(a.ToString());
                        }
                    }
                    else
                    {
                        error_file(s[index]);
                    }
                }
                else
                {
                    if (!Preveri_povezavo())
                    {
                        //Create an icon and make changes that indicate the 
                    }
                    error_file(s[index]);
                }
            }
        }

        //Playing the audio over the playback class and (re)setting the parameters
        private void predvajaj(string audio_file)
        {
            trackBar1.Value = 0;
            sekunde = 0; 
            p_cas.Text = "00:00:00"; 
            playing = true;
            if (!File.Exists(s[index]))
            {
                MessageBox.Show("Skladba s tem imenom, ne obstaja, preverite, če se datoteka nahaja na tem mestu, če ne ste jo morda izbrisali ", "Ne obstaja!");
                s.Remove(s[index]);
                Napolni_lisbox(s);
                if (index >= listBox1.Items.Count)
                {
                    index--;
                }
                audio_file = s[index];
            }
            metapodatek(s[index]);
            int cas = Convert.ToInt16(TimeSpan.Parse(dolzina.Text).TotalSeconds);
            trackBar1.Maximum = cas;
            timer1.Start();// začnemo s štetjem
            glasba.open_song(s[index]);
            glasba.play_song();
            //poizvedba.(izvajalec.Text + "," + album.Text + "," +metapodatki.Album_art);
           
        }
    
        //The function connects to google to see if it can reach it, if not the program will search for album art.
        private bool Preveri_povezavo()
        {
            try
            {
                using (var klient = new WebClient())
                {
                    using (var try_open = klient.OpenRead("https://google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        
        //The function fills the listbox with the audio files
        private void Napolni_lisbox(List<string> seznam)
        {
            listBox1.Items.Clear();
            foreach (string napolni_listbox in seznam)
            {
              metapodatki.Meta_podatki = napolni_listbox;
              globalni_string = metapodatki.Meta_podatki.Split(',')[0];
              listBox1.Items.Add(globalni_string);
            }
        }
        //The function controlls buttons forward, back. It also controls the function to choose the song over the listbox.
        private void Funkcije_gumbov(string Funkcija)
        {
            if (listBox1.Items.Count < 2)
            {
                odpri();
            }
            else
            {
              glasba.stop_music();
                if (Funkcija == "Naprej")
                {
                    trackBar1.Value = 0;
                    if (nakljucno == true)
                    {
                        index = Convert.ToInt16(new Random().Next(0,listBox1.Items.Count));
                    }
                    else
                    {
                        if ((index + 1) == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
                else if (Funkcija == "Listbox")
                {
                    index = Convert.ToInt16(listBox1.SelectedIndex);
                    if (index == -1)
                    {
                        index = 0;
                    }
                }
                else if (Funkcija == "Nazaj")
                {
                    if (nakljucno == true)
                    {
                        index = Convert.ToInt16(new Random().Next(0,
                        listBox1.Items.Count));
                    }
                    else
                    {
                        if ((index - 1) == -1)
                        { 
                            index = Convert.ToInt16(listBox1.Items.Count - 1);
                        }
                        else
                        {
                            index--;
                        }
                    }
                }
                predvajaj(s[index]);
                playing = true;
            }
        }

        //If there is no album art or medata could not be read, generic images will be displayed
        private void error_file(string type)
        {
            if (vrsta.Contains(".wav"))
            {
             //  pictureBox1.Image = Resource1.wav;
            }
            else if (vrsta.Contains(".mp3"))
            {
             //pictureBox1.Image = Resource1.mp3;
            }
            else if (vrsta.Contains(".flac"))
            {
             //pictureBox1.Image = Resource1.flac;
            }
        }
   }
}
