using LocalNote.Models;
using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace LocalNote.Repositories
{

    public class LocalNoteRepo
    {
        private static string _basepath = @"C:\Users\Administrator\AppData\Local\Packages\7f68f349-8a6c-4585-9cda-a5b8d368e038_75cr2b68sm664\LocalState";
        private static string[] _strDataFiles = Directory.GetFiles(_basepath); 

        private static StorageFolder noteFolder = ApplicationData.Current.LocalFolder;

        public static bool saveStatus=false;
        public async static void SaveNoteToFile(string NewTitle, string NewContent)
        {
            try
            {
                if (CheckNoteTitle(NewTitle))
                {
                    saveStatus = true;
                    StorageFile newNote = await noteFolder.CreateFileAsync(NewTitle,
                        CreationCollisionOption.OpenIfExists);
                    await Windows.Storage.FileIO.AppendTextAsync(newNote, NewContent);
                   
                }
                else {
                    saveStatus = false;
                    ContentDialog savedDialog = new ContentDialog()
                    {
                        Title = "Save Unsuccessful",
                        Content = "There is already a file with the same name",
                        PrimaryButtonText = "OK"
                    };
                    await savedDialog.ShowAsync();

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with file writing. Ahhhhh!");
            }

        }

        public static List<NoteModel> ReadNote() {
            List < NoteModel > Notes=new List<NoteModel>();
            
            foreach (string file in _strDataFiles)
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                string content="";
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                   content+=line;
                }

                NoteModel note = new NoteModel(Path.GetFileName(file), content);
                Notes.Add(note);

            }
            return Notes;
        }

        public static bool CheckNoteTitle(string title) {

            foreach (string file in _strDataFiles)
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);

                if (Path.GetFileName(file) == title)
                    return false;
            
            
            }

            return true;
        }
        public async static void EditToFile(string editTitle, string editContent)
        {
            try
            {

                Windows.Storage.StorageFile editNote =
                await noteFolder.GetFileAsync(editTitle);
                await FileIO.WriteTextAsync(editNote, editContent);




            }
            catch (Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with no file with title "+ editTitle+" found!");
            }

        }
    }    

}
