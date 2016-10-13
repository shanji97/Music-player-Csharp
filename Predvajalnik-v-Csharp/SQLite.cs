
using System;
using System.Data.SQLite;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predvajalnik_v_CSharp
{
    class SQLite
    {
        //SPREMENJLJIVKE
        private string sql;
        private string vrstica;
        private string rezultat;
        //  private short stevilo_vstic = 0;

        //OBJEKTI
        SQLiteConnection povezava_z_bazo = new SQLiteConnection("Data Source=Povezave_za_pesmi.sqlite;Version=3;"); // ustvarimo povezavo z bazo

        //LASTNOSTI
        public string album_izvajalec_in_link
        {
            get { return vrstica; }
            set {vrstica = iskanje_vnosa_in_izpis(value); }
        }//za vpis izvajalca, albuma in linka slike

        //METODE


        public void naredi_bazo()
        {
            SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite");
            povezava_z_bazo.Open();
            sql = "CREATE TABLE slike_albuma (izvajalec VARCHAR(50) NOT NULL, album VARCHAR(50),slika VARCHAR(50));";
            new SQLiteCommand(sql, povezava_z_bazo).ExecuteNonQuery();
            povezava_z_bazo.Close();
        }//naredimo novo podatkovno bazo (končana)
        public void vnos_izvajalca_albuma_in_linka_do_slike(string vnos_parametrov)
        {
            povezava_z_bazo.Open();
            sql = "INSERT INTO slike_albuma VALUES ("+vnos_parametrov.Split()[0]+","+ vnos_parametrov.Split()[1] + ","+ vnos_parametrov.Split()[2] + ");";
             new SQLiteCommand(sql, povezava_z_bazo).ExecuteNonQuery();
            povezava_z_bazo.Close();
            //0 izvajalec, 1 album, 2 link od slike
        }
      
        private string iskanje_vnosa_in_izpis(string album_izvajalec_link)
        {
            povezava_z_bazo.Open();

            album_izvajalec_link = "0";
            try
            {
                sql = "SELECT count(slika) FROM slike_albuma WHERE album="+album_izvajalec_link.Split(',')[1]+"and izvajalec="+album_izvajalec_link.Split(',')[0] + ";";
                SQLiteCommand iskanje = new SQLiteCommand(sql, povezava_z_bazo);
               if ((short) iskanje.ExecuteScalar()==1)
                {
                  
                    try
                    {
                        sql = "SELECT slika FROM slike_albuma WHERE album=" + album_izvajalec_link.Split(',')[1] + "and izvajalec=" + album_izvajalec_link.Split(',')[0] + ";";
                        iskanje = new SQLiteCommand(sql, povezava_z_bazo);
                        album_izvajalec_link = (string)iskanje.ExecuteScalar();
                    }
                    catch 
                    {
                        album_izvajalec_link = "0";
                    }
                }
               else
                {
                    album_izvajalec_link = "0";
                }
       
            }
            catch
            {
                album_izvajalec_link = "0";
            }
            finally
            {
                povezava_z_bazo.Close();
            }
            return album_izvajalec_link;
         }//funkcija preveri,če je v bazi link od slike in če ja ga vrne
 

        

    }
}
