using LocalNote.Models;
using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LocalNote.Repositories
{

    public class LocalNoteRepo
    {
        public static List<NoteModel> notes;
        //public List<string> Title { get; set; }
        //public List<string> Content { get; set; }

        //public LocalNoteRepo (){
        //    Title=new List<string>();
        //    Content=new List<string>();
        //}

        private static StorageFolder noteFolder = ApplicationData.Current.LocalFolder;

        public async static void SaveNoteToFile(string NewTitle, string NewContent)
        {
            try
            {
                StorageFile newNote = await noteFolder.CreateFileAsync(NewTitle,
                    CreationCollisionOption.OpenIfExists);
                await Windows.Storage.FileIO.AppendTextAsync(newNote, NewContent);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with file writing. Ahhhhh!");
            }

        }
        public async static void ReadNoteToFile()
        {
            
            try
            {
                StringBuilder outputText = new StringBuilder();

                IReadOnlyList<StorageFile> fileList = await noteFolder.GetFilesAsync();
                
                foreach (StorageFile file in fileList)
                {
                    NoteModel note = new NoteModel(file.Name, await Windows.Storage.FileIO.ReadTextAsync(file));
                    notes.Add(note);
                }

                }
            catch (Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with file writing. Ahhhhh!");
            }
        }
    }    
}
