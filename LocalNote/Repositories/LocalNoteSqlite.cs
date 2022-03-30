using LocalNote.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LocalNote.Repositories
{
    public class LocalNoteSqlite
    {
        private static String dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "NotesDB.db");

        //Drop the table of the database
        public static void DropTable()
        {
            try
            {
                using (SqliteConnection db =
                    new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    String dropTable = "DROP TABLE IF EXISTS NoteTable";

                    SqliteCommand create = new SqliteCommand(dropTable, db);
                    create.ExecuteReader();

                    db.Close();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        //Create table when NoteTable does not exist
        public static void InitializeDB()
        {

            //DropTable();
            try
            {
                using (SqliteConnection db =
                    new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    //The table only has 2 fields, NoteName and NoteContent
                    String createTable = "CREATE TABLE IF NOT EXISTS " +
                        "NoteTable ( " +
                        "NoteTitle nvarchar(100) NOT NULL, " +
                        "NoteContent nvarchar(500) );";

                    SqliteCommand create = new SqliteCommand(createTable, db);
                    create.ExecuteReader();

                    db.Close();
                }
            }
            catch (Exception ex) {

                throw new Exception(ex.Message);

            }
        }


        // Adds a New note to the  database
        public static void AddNote(String NoteTitle, String NoteContent)
        {
            try
            {
                //Normalizes the names of the notes
                NoteTitle = NoteTitle.Replace(" ", "_");

                using (SqliteConnection db =
                    new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;
                    insertCommand.CommandText = "INSERT INTO NoteTable " +
                        "VALUES (@NoteTitle, @NoteContent);";
                    insertCommand.Parameters.AddWithValue("@NoteTitle", NoteTitle);
                    insertCommand.Parameters.AddWithValue("@NoteContent", NoteContent);
                    insertCommand.ExecuteReader();

                    db.Close();
                }
            }
            catch (Exception ex) { 
            
                throw new Exception(ex.Message);
            }
        }

        // Modify the selected note
        public static void EdidNote(String NoteTitle, String NoteContent)
        {
            try
            {
                //Normalizes the names of the notes
                NoteTitle = NoteTitle.Replace(" ", "_");

                using (SqliteConnection db =
                    new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand editCommand = new SqliteCommand();
                    editCommand.Connection = db;
                    editCommand.CommandText = "UPDATE NoteTable " +
                        "SET NoteContent=@NoteContent WHERE NoteTitle= @NoteTitle;";
                    editCommand.Parameters.AddWithValue("@NoteTitle", NoteTitle);
                    editCommand.Parameters.AddWithValue("@NoteContent", NoteContent);
                    editCommand.ExecuteReader();

                    db.Close();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        //Determine if a document named "NoteTitle" already exists
        public static bool checkTitle(String NoteTitle)
        {
            try
            {
                //Normalizes the names of the notes
                NoteTitle = NoteTitle.Replace(" ", "_");
                using (SqliteConnection db =
                    new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand();
                    selectCommand.Connection = db;

                    //Inserts the values passed in as paramaters into the DB, using parameters in the command to prevent SQL injection
                    selectCommand.CommandText = "SELECT NoteTitle FROM NoteTable Where NoteTitle= @NoteTitle;";
                    selectCommand.Parameters.AddWithValue("@NoteTitle", NoteTitle);

                    SqliteDataReader reader = selectCommand.ExecuteReader();

                    // Returns false if data exists, otherwise returns true
                    if (reader.Read())
                    {
                        return false;
                        
                    }
                    else
                    {
                        return true;
                    }

                }
            }
            catch (Exception ex) {
                //If the excpetion is captured, it will also return false,
                //and the save command will not continue to execute the stored function
                return false;
            }
        }


        // Gets all the notes from the database
        public static List<NoteModel> GetNotes()
        {
            try
            {
                List<NoteModel> notes = new List<NoteModel>();

                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand =
                        new SqliteCommand("SELECT NoteTitle, NoteContent FROM NoteTable;", db);

                    SqliteDataReader reader = selectCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        string NoteTitle = reader.GetString(0);
                        string NoteContent = reader.GetString(1);
                        //create new note and add to the notes
                        NoteModel note = new NoteModel(NoteTitle, NoteContent);
                        notes.Add(note);
                    }

                    db.Close();

                    //Returns the List of NoteModels 
                    return notes;
                }
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message);
            }
        }

        // Deletes a note from the database

        public static void DeleteNote(String NoteTitle)
        {
            try
            {
                NoteTitle = NoteTitle.Replace(" ", "_");
                using (SqliteConnection db =
                    new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    SqliteCommand deleteCommand =
                        new SqliteCommand("DELETE FROM NoteTable where NoteTitle =@NoteTitle;", db);

                    deleteCommand.Parameters.AddWithValue("@NoteTitle", NoteTitle);
                    SqliteDataReader query = deleteCommand.ExecuteReader();

                    db.Close();

                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }
}
