using System;
using System.Data.SQLite;
namespace Predvajalnik_glasbe_v_CSharpu
{
    class SQLite
    {
        SQLiteConnection povezava_z_bazo;
        //SPREMENJLJIVKE
        private string sql;
        private string vrstica;
        //OBJEKTI
        //LASTNOSTI
        public string iskanje_vnosa
        {
            get { return vrstica; }
            set { vrstica = iskanje_vnosa_in_izpis(value); }
        }//za vpis izvajalca, albuma in linka slike
         //METODE
        private void povezi()
        {
            povezava_z_bazo = new SQLiteConnection("Data Source=" +
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music
Player\Povezave_za_pesmi.sqlite" + "; Version=3;"); // ustvarimo povezavo z bazo
            povezava_z_bazo.Open();
        }
        public void naredi_bazo()
        {
            SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite");
            povezi();
            sql = "CREATE TABLE slike_albuma (izvajalec VARCHAR(50) NOT NULL, album VARCHAR(50),slika VARCHAR(75))";
            SQLiteCommand kreiraj = new SQLiteCommand(sql, povezava_z_bazo);
            kreiraj.ExecuteNonQuery();
            povezava_z_bazo.Close();
        }//naredimo novo podatkovno bazo
        public void vnos_slike(string vnos_parametrov)
        {
            povezi();
            sql = "INSERT INTO slike_albuma (izvajalec, album, slika) VALUES ('" +
            vnos_parametrov.Split(',')[0] + "','" + vnos_parametrov.Split(',')[1] + "','" +
            vnos_parametrov.Split(',')[2] + "')";
            try
            {
                SQLiteCommand vnos = new SQLiteCommand(sql, povezava_z_bazo);
                vnos.ExecuteNonQuery();
                povezava_z_bazo.Close();
            }
            catch
            { }
        }//vnos slike v podatkovno bazo
        private string iskanje_vnosa_in_izpis(string album_izvajalec)
        {
            povezi();
            album_izvajalec = "0";
            try
            {
                sql = "SELECT count(slika) FROM slike_albuma WHERE album='" +
                album_izvajalec.Split(',')[0] + "' and izvajalec='" + album_izvajalec.Split(',')[1] +
                "'";
                SQLiteCommand iskanje = new SQLiteCommand(sql, povezava_z_bazo);
                if ((short)iskanje.ExecuteScalar() == 1)
                {
                    try
                    {
                        sql = "SELECT slika FROM slike_albuma WHERE album='" +
                        album_izvajalec.Split(',')[1] + "' and izvajalec='" + album_izvajalec.Split(',')[0] +
                        "'";
                        iskanje = new SQLiteCommand(sql, povezava_z_bazo);
                        album_izvajalec = (string)iskanje.ExecuteScalar();
                    }
                    catch
                    {
                        album_izvajalec = "0";
                    }
                }
                else
                {
                    album_izvajalec = "0";
                }
            }
            catch
            {
                album_izvajalec = "0";
            }
            finally
            {
                povezava_z_bazo.Close();
            }
            return album_izvajalec;
        }//funkcija preveri,če je v bazi link od slike in če ja ga vrne
    }
}