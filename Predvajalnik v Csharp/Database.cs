using System;
using System.Data.SQLite;

namespace Predvajalnik_v_CSharp
{
    class Database
    {
        SQLiteConnection db_connection;
    
        SQLiteCommand command;

        private string sql, row, link;
    
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
        public string select_link_from_db(string artist, string album)
        {
            connect_to_db();
            string link = "";
            try
            {
                sql = "SELECT link FROM album_art_covers WHERE album='" + album + "' AND artist='" + artist + "'";
                command = new SQLiteCommand(sql, db_connection);
                link = (string)command.ExecuteScalar();
                return link;
            }
            finally
            {
                db_connection.Close();
            }
        }
   
        private void connect_to_db()
        {
           db_connection = new SQLiteConnection("Data Source=" +Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Music Player\Povezave_za_pesmi.sqlite" + "; Version=3;");
            db_connection.Open();
        }

        //This method is going to insert the link of the album cover into the db, because, we don't want to search for it every time on the net
        
        public void instert_into_or_update(string artist,string album,string link,string operation)
        {
            connect_to_db();
            try
            {
                if(operation=="UPDATE") //Updating the database mannually
                {
                    sql = "UPDATE album_art_covers SET'" + link + "' WHERE artist='" + artist + "' AND alubm='" + album + "'";
                }
                else
                {
                    sql = "INSERT INTO album_art_covers VALUES ('" + artist + "','" + album + "','" + link + "')";
                }
              
                command = new SQLiteCommand(sql, db_connection);
                command.ExecuteNonQuery();
            }

            finally
            {
                db_connection.Close();
            }
        }
    }
}
