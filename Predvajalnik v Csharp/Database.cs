﻿using System;
using System.Data.SQLite;

namespace Predvajalnik_v_CSharp
{
    class Database
    {
        SQLiteConnection db_connection;
        //SPREMENJLJIVKE
        SQLiteCommand command;

        private string sql;
        private string vrstica;
        //OBJEKTI
        //LASTNOSTI
    /*    public string iskanje_vnosa
        {
            get { return vrstica; }
            set { vrstica = select_from(value); }
   }*/
      

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
        private string select_from(string artist, string album)
        {
            connect_to_db();
            string link = "";
            try
            {
                sql = "SELECT link FROM album_art_covers WHERE album='"+album+"' AND artist='"+artist+"'";
                command = new SQLiteCommand(sql, db_connection);
                link = (string)command.ExecuteScalar();
            }
           finally
            {
                db_connection.Close();
            }
            return link;
        }
        private void instert_into(string artist,string album,string link)
        {
            connect_to_db();
            sql="INSERT INTO album_art_covers VALUES ('"+artist+"','"+album+"','"+link+"')";
            command = new SQLiteCommand(sql, db_connection);
            command.ExecuteNonQuery();
            db_connection.Close();
        
        }
    }
}
