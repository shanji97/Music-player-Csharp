using System;
using System.Data.SQLite;

namespace Predvajalnik_v_CSharp
{
    class Database
    {
        SQLiteConnection db_connection;
<<<<<<< HEAD
        //SPREMENJLJIVKE
        SQLiteCommand command;

        private string sql, row, link;
       

 
        //OBJEKTI
        //LASTNOSTI






     


        //DB connection method
        private void connect_to_db()
        {
            db_connection = new SQLiteConnection("Data Source=" +
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite" + "; Version=3;"); // ustvarimo povezavo z bazo
            db_connection.Open();
        }
        //DB creation
        public void create_db()
        {
            SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite");
            connect_to_db();
            sql = "CREATE TABLE album_art_covers (artist VARCHAR(50) NOT NULL, album VARCHAR(50),link VARCHAR(75))";
            SQLiteCommand commmand = new SQLiteCommand(sql, db_connection);
            command.ExecuteNonQuery();
            db_connection.Close();
        }

        //This method is going to select the link from the database when no album art is available
        private string select_link_from_db(string artist, string album)
        {
            connect_to_db();
            string link = "";
            try
            {
                sql = "SELECT link FROM album_art_covers WHERE album='"+album+"' AND artist='"+artist+"'";
                command = new SQLiteCommand(sql, db_connection);
                link = (string)command.ExecuteScalar();
=======
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
>>>>>>> origin/master
            }
           finally
            {
                db_connection.Close();
            }
<<<<<<< HEAD
            return link;
        }
        //This method is going to insert the link of the album cover into the db, because, we don't want to search for it every time
        private void instert_into(string artist,string album,string link)
        {
            connect_to_db();
            sql="INSERT INTO album_art_covers VALUES ('"+artist+"','"+album+"','"+link+"')";
            command = new SQLiteCommand(sql, db_connection);
            command.ExecuteNonQuery();
            db_connection.Close();
        
=======
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
>>>>>>> origin/master
        }
    }
}// This class will be rewritten, the purpose of this class is to save the links of the album art, which is not downloaded with the Graceenote API.
//There will be also method where you can mannually add the link to the album art.
