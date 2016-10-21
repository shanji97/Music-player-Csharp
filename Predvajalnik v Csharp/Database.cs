using System;
using System.Data.SQLite;

namespace Predvajalnik_v_CSharp
{
    class Database
    {
        SQLiteConnection db_connection;
        //VARIABLES
        private string sql;
        private string row;
       
        //PROPERTIES
        public string iskanje_vnosa
        {
            get { return row; }
            set {row = iskanje_vnosa_in_izpis(value); }
        }//za vpis izvajalca, albuma in linka slike
         //METHODS
        private void connect_to_db()
        {
           db_connection = new SQLiteConnection("Data Source=" +Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite" + "; Version=3;"); 
        }
        /*
       //Method that selects the link of album art cover, the artist and the album name and insert them into the db 
        public void vnos_slike(string vnos_parametrov)
        {
            connect_to_db();
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
        }*/
        //Searching for the album art cover
        private string iskanje_vnosa_in_izpis(string album_izvajalec)
        {
            connect_to_db();
            album_izvajalec = "0";
            try
            {
                sql = "SELECT count(slika) FROM slike_albuma WHERE album='" +
                album_izvajalec.Split(',')[0] + "' and izvajalec='" + album_izvajalec.Split(',')[1] +
                "'";
                SQLiteCommand iskanje = new SQLiteCommand(sql, db_connection);
                if ((short)iskanje.ExecuteScalar() == 1)
                {
                    try
                    {
                        sql = "SELECT slika FROM slike_albuma WHERE album='" +
                        album_izvajalec.Split(',')[1] + "' and izvajalec='" + album_izvajalec.Split(',')[0] +
                        "'";
                        iskanje = new SQLiteCommand(sql, db_connection);
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
                db_connection.Close();
            }
            return album_izvajalec;
        }
        //Creating the database
        public void create_db()
        {
            SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite");
            connect_to_db();
            sql = "CREATE TABLE album_cover (artist VARCHAR(50) NOT NULL, album VARCHAR(50),external_link VARCHAR(75))";
            SQLiteCommand create = new SQLiteCommand(sql,db_connection);
            create.ExecuteNonQuery();
            db_connection.Close();
        }
    }
}// This class will be rewritten, the purpose of this class is to save the links of the album art, which is not downloaded with the Graceenote API.
//There will be also method where you can mannually add the link to the album art.
