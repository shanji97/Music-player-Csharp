using System;
using System.Collections.Generic;


using System.IO;


using System.Runtime.InteropServices;

using System.Windows.Forms;

namespace Predvajalnik_v_CSharp
{
    public partial class Form1 : Form
    {
        //DLL-ji in delo z hardwerowm
        [DllImport("wininet.dll")] //DLL za delo z internetom
        private extern static bool InternetGetConnectedState(out int description, int reservedValue); //spremenljivka za delo z mrežno kartico (stranje interneta)
        public Form1()
        {
            InitializeComponent();
            //ZAČETNE SPREMENLJIVKE
          
            timer1.Interval = 1000; //nastavim interval štoparice na 1s (na eno sekundo se naj dejanje dogodek timer tick ponavlja

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\slika.txt"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\slika.txt");
            }
            //STVARI, KI SE IZVEDEJO OB NALAGANJU APLIKACIJA
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player")  )//preveri ce obstaja imenik z imenom Media Player
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player"); //naredimo imenik za media player
                        
            }
           
            if(!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\Music Player\Povezave_za_pesmi.sqlite"))// preverimo,če na tej lokaciji osbtaja že kaka baza
            {
                new SQLite().naredi_bazo(); //ustvarimo podatkovno bazo
            }


            



        }
        //GLOBALNE SPREMENLJIVKE
        List<string> skladba = new List<string>(); // seznam v katerega vstavimo poti  audio datotek,ki jih bomo predvajali
        Metapodatki metapodatki = new Metapodatki(); //inicializiramo nov objekt razreda Metapodatki
        SQLite poizvedba = new SQLite();
        Predvajanje glasba = new Predvajanje(); // incializiramo nov objekt razreda Predvajanje
    
        private int stevec = 0; //stevec,ki nam bo stel čas
        private short index = 0,  sekunde = 0, klik=0 ; //z indeksom bomo si pomagali, da zajamemo datoteko iz 









       
        //GUMBEKI
        private void oProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }
        //s pritiskom na gumb odpremo novo okno - "O programu" (končana)
        private void izhodToolStripMenuItem_Click(object sender, EventArgs e)
        {
          Application.Exit();
        }
        //s pritiskom na gumb končamo z aplikacijo (končana)

        // Stvari, ki se zgodijo,ko se aplikacija naloži
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            glasba.stop_mp3();
           
            if((index-1)==-1)
            {
                index = Convert.ToInt16(listBox1.Items.Count - 1);
            }
            else
            {
                index--;
            }
            //vrni metapodatke
            predvajaj(skladba[index]);
            //nazaj
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                odpri();
            }
            else
            {
                klik++;
                if (klik == 1)
                {

                    predvajaj("");
                }
                else if (klik % 2 == 0)
                {
                    timer1.Stop();

                    glasba.stop_mp3();
                }
                else if (klik != 1 && klik % 2 == 1)
                {
                    predvajaj(skladba[index]);
                }
            }
         }
        private void button3_Click(object sender, EventArgs e)
        {
           
            glasba.stop_mp3();
          
            if((index+1)==listBox1.Items.Count)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            //vrni metapodatke
            predvajaj(skladba[index]);
            
        }
      
        private void audioDatotekaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            odpri();

        }
        //odpremo eno audio datoteko
   
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                odpri();
            }
            else
            {
                glasba.stop_mp3();
                predvajaj(skladba[listBox1.SelectedIndex]);
                index = Convert.ToInt16(listBox1.SelectedIndex);
                
                priredi_meta(skladba[listBox1.SelectedIndex]);  // pridobimo meta_podatke
            //    vrni_sliko(izvajalec.Text, album.Text);    //pridobimo sliko
                




               



            }
        }



    //FUNKCIJE
    private void odpri()
        {
           string vhodna_datoteka = "";
            string[] a_datoteke;
           
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "MP3|*.mp3"; //filter za audio datoteke
            openFileDialog1.Title = "Izberite več audio datotek.";//naslov dialoga
            openFileDialog1.Multiselect = true; //izbira več datotek
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("Izbiranje glasbe preklicano!", "Preklicano!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //nastavi picturebox na generic
                album.Text = "Glasba ni naložena";
                izvajalec.Text = "Glasba ni naložena";
                naslov.Text = "Glasba ni naložena";
            }
            else
            {
               try
                {
                   a_datoteke = openFileDialog1.FileNames;
               
                   skladba.AddRange(a_datoteke);
                   listBox1.Items.Clear();

                   foreach(string napolni_listbox in skladba)
                    {                       
                        metapodatki.Meta_podatki = napolni_listbox;
                        vhodna_datoteka = metapodatki.Meta_podatki;
                        vhodna_datoteka = vhodna_datoteka.Split(',')[0];
                        listBox1.Items.Add(vhodna_datoteka);
                    }
                  
                    Array.Clear(a_datoteke, 0, a_datoteke.Length);

                 
                }
                
                catch(Exception izjema)
                {
                    MessageBox.Show("Izbrane datoteke ni mogoče odpreti" + Environment.NewLine + "Razlog: " + izjema.ToString(), "Napaka!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (listBox1.Items.Count == 0)
                    {
                        button3.Enabled = false;
                        button1.Enabled = false;
                    }
                    else
                    {
                        button3.Enabled = true;
                        button1.Enabled = true;
                    }

                }
            }
      
       }//funkcija odpre eno ali več audio datotek 

        private void priredi_meta(string datoteka_za_metapodatke)
    {       
     Label[] oznake = new Label[] { naslov, izvajalec, album, dolzina };
     metapodatki.Meta_podatki = datoteka_za_metapodatke;
     datoteka_za_metapodatke = metapodatki.Meta_podatki;
     ushort id = 0;

     foreach(Label oznaka in oznake)
          {
           oznaka.Text = datoteka_za_metapodatke.Split(',')[id];
           id++;
          }
          
    }//funkcija nam priredi metapodatke (končana)





        private void vrni_sliko(string i,string a)
     {
            stevec = 0;
            if (InternetGetConnectedState(out stevec,0))
            {
                poizvedba.album_izvajalec_in_link = izvajalec.Text + "," + album.Text;


               // else ce ne dolpotegni pa ven vzemi pa shrani v bazo 
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Media Player\slika.txt"))
                {
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Media Player\slika.txt");
                }
                metapodatki.pridobitev_slike = izvajalec.Text + "," + album.Text;
                   
                if(metapodatki.pridobitev_slike=="0")
                {
                    //nastavi generično sliko na picturebox-u od 
                    // pa insertamo v podatkovno bazo
                }
                else
                {
                    //vstavi sliko od
                }
               
            }

            else
            {
                MessageBox.Show("Slike albuma nisem uspel pridobiti, ker ni povezave do interneta.\nPreverite fizično povezavo (kabli,omrežna oprema in nastavitve", "Ni interneta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    

     }

        private void dolzina_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            if (p_cas.Text == dolzina.Text)
            {
                glasba.stop_mp3();
                timer1.Stop();



            
                sekunde = 0;

                if ((index + 1) == listBox1.Items.Count)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }

                predvajaj(skladba[index]);

            }
            else
            {
                sekunde++;
               
            }
            TimeSpan cas = TimeSpan.FromSeconds(sekunde);
            p_cas.Text = cas.ToString(@"hh\:mm\:ss");

        }//funkcija za upravljaje s časom

      private void predvajaj(string audio_file)
        {   if(audio_file=="")
            {
                audio_file = skladba[index];
            }

          //daj v zanko
          if(!File.Exists(audio_file))
          {
              MessageBox.Show("Skladba s tem imenom, ne obstaja, preverite, če se datoteka nahaja na tem mestu, če ne ste jo morda izbrisali ", "Ne obstaja!");
              skladba.Remove(skladba[index]);
              listBox1.Items.Clear();
              // napiši kodo za ponovno vstavitev 
              foreach (string napolni_listbox in skladba)
              {
                  metapodatki.Meta_podatki = napolni_listbox;
                  audio_file = metapodatki.Meta_podatki;
                  audio_file = audio_file.Split(',')[0];
                  listBox1.Items.Add(audio_file);
              }
                           

              audio_file = skladba[index];
          }





            sekunde = 0; //sekunde postavimo na nič
            p_cas.Text = "00:00:00"; //  posravimo na nula
            timer1.Start();// začnemo s štetjem
            glasba.open_mp3(audio_file);
                glasba.play_mp3();
              
                priredi_meta(audio_file);
          }

     

      


    }
}
/*
INDEX PRIREDI spet takrat, ko zoveš novo skladbo od listbox.item.selected index
 * 
 * 
 * album že vrni striman

    */