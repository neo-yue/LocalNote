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

        //The path of the package is used to read the file
        private static string _basepath = @"C:\Users\Administrator\AppData\Local\Packages\7f68f349-8a6c-4585-9cda-a5b8d368e038_75cr2b68sm664\LocalState";
        private static string[] _strDataFiles = Directory.GetFiles(_basepath); 

        private static StorageFolder noteFolder = ApplicationData.Current.LocalFolder;

        //Used to determine whether the storage fails due to the same name
        public static bool saveStatus=false;

        //Dynamically store files
        public async static void SaveNoteToFile(string NewTitle, string NewContent)
        {
            try
            {
                //Store files without duplicate names
                if (CheckNoteTitle(NewTitle))
                {
                    saveStatus = true;
                    StorageFile newNote = await noteFolder.CreateFileAsync(NewTitle,
                        CreationCollisionOption.OpenIfExists);
                    await Windows.Storage.FileIO.AppendTextAsync(newNote, NewContent);
                   
                }
                else {
                    //Alert users when the name is duplicated

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
                //Alert user when storage fails
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error Saving File",
                    Content = "There was an error saving the file, please try again",
                    PrimaryButtonText = "OK"
                };
                Debug.WriteLine("Oh noes! An error occurred with file writing. Ahhhhh!");
            }

        }

        //read file on initialization
        public static List<NoteModel> LoadNote() {
            List < NoteModel > Notes=new List<NoteModel>();
            try
            {
                foreach (string file in _strDataFiles)
                {
                    StreamReader sr = new StreamReader(file, Encoding.Default);
                    string content = "";
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        content += line;
                    }

                    NoteModel note = new NoteModel(Path.GetFileName(file), content);
                    Notes.Add(note);
                    sr.Close();
                }
                return Notes;
               
            }
            catch (IOException ex){
               
                Debug.WriteLine("Could not open the folder, please check the folder path ");
                return null;
            }
        }

        //Check if the file has the same name
        public static bool CheckNoteTitle(string title) {

            try
            {
                foreach (string file in _strDataFiles)
                {
                    StreamReader sr = new StreamReader(file, Encoding.Default);

                    if (Path.GetFileName(file) == title)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Could not open the folder, please check the folder path ");
                return false;
            }

      
        }
        //Edit file content
        public async static Task EditToFile(string editTitle, string editContent)
        {
            try
            {

                Windows.Storage.StorageFile editNote =
                await noteFolder.GetFileAsync(editTitle);
                await FileIO.WriteTextAsync(editNote, editContent);
                ContentDialog editDialog = new ContentDialog()
                {
                    Title = "Save Successful",
                    Content = editTitle + " content has been updated!",
                    PrimaryButtonText = "OK"
                };
                await editDialog.ShowAsync();

            }
            catch (Exception ex)
            {
                //Alert users when files are occupied
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error Saving File",
                    Content = "There was an error saving the file, please try again",
                    PrimaryButtonText = "OK"
                };

                await errorDialog.ShowAsync();

                Debug.WriteLine(editTitle+" is in use and the update failed");
            }

        }
        //Delete file
        public async static void DeleteNote(string title) {
            try
            {                                                   
                StorageFile deleteFile = await noteFolder.GetFileAsync(title);
                await deleteFile.DeleteAsync();

            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error Delete File",
                    Content = "There was an error deleted the file, please try again",
                    PrimaryButtonText = "OK"
                };
            }
        }

    }    

}
