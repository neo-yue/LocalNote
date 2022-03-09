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

namespace LocalNote.Repositories
{

    public class LocalNoteRepo
    {
        //private static List<NoteModel> _Notes=new List<NoteModel>();

        //public List<NoteModel> Notes { get { return _Notes; } }
        //    public LocalNoteRepo(){
        //        this.Notes=new List<NoteModel>();

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
        public async static void ReadNoteToFile(List<NoteModel> Notes)
        {
            
            //try
            //{
            
                IReadOnlyList<StorageFile> fileList = await noteFolder.GetFilesAsync();

            //    foreach (StorageFile file in fileList)
            //    {
            //        NoteModel note = new NoteModel(file.Name, await Windows.Storage.FileIO.ReadTextAsync(file));
            //        Debug.WriteLine(note.NoteContent);
            //        Notes.Add(note);

            //    }
            //    Debug.WriteLine(Notes.Count);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Oh noes!222222");
            //}
            foreach (StorageFile file in fileList)
            {
                NoteModel note = new NoteModel(file.DisplayName,"");
                Notes.Add(note);

            }
            
        }

        public static List<NoteModel> ReadNote() {
            List < NoteModel > Notes=new List<NoteModel>();
            string basepath = @"C:\Users\Administrator\AppData\Local\Packages\7f68f349-8a6c-4585-9cda-a5b8d368e038_75cr2b68sm664\LocalState";
            string[] strDataFiles = Directory.GetFiles(basepath);
            foreach (string file in strDataFiles)
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
    }    
}
