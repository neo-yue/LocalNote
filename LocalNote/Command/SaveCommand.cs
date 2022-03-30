using LocalNote.Models;
using LocalNote.Repositories;
using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LocalNote.Command
{
    public class SaveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.LocalNoteViewModel lnvm;
        public string savedTitle;
        public string savedContent;
        public NoteModel newNote;
       
        public event EventHandler createdNewNote;
        public event EventHandler editNewNote;
        public SaveCommand(ViewModels.LocalNoteViewModel lnvm) { 
        this.lnvm = lnvm;
        }

        public bool CanExecute(object parameter)
        {
            return !lnvm.textboxStatus();

        }

        //use for LocalNoteRepo
        //public async void Execute(object parameter)
        //{
        //    if (lnvm.SelectedNote != null)              //edit mode
        //    {
        //        savedTitle = lnvm.Title;                 //Get the content and title of the selected note               
        //        savedContent = lnvm.Content;
        //        await LocalNoteRepo.EditToFile(savedTitle, savedContent);         //save the new content 
        //        editNewNote?.Invoke(this, new EventArgs());             //trager the editNewNote event


        //    }
        //    else
        //    {                                                   //save mode

        //        SaveCommandDialog snd = new SaveCommandDialog();
        //        ContentDialogResult result = await snd.ShowAsync();

        //        if (result == ContentDialogResult.Primary)          //after confirm storage
        //        {
        //            try
        //            {


        //                savedTitle = snd.NewTitle;                  //save the new title and new content 
        //                savedContent = lnvm.Content;
        //                LocalNoteRepo.SaveNoteToFile(snd.NewTitle, savedContent);

        //                if (LocalNoteRepo.saveStatus)
        //                {
        //                    newNote = new NoteModel(snd.NewTitle, savedContent);
        //                    createdNewNote?.Invoke(this, EventArgs.Empty);
        //                    lnvm.SelectedNote = newNote;
        //                    ContentDialog savedDialog = new ContentDialog()
        //                    {
        //                        Title = "Save Successful",
        //                        Content = "Names saved successfully to file, hurray!",
        //                        PrimaryButtonText = "OK"
        //                    };
        //                    await savedDialog.ShowAsync();
        //                }





        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine("Error when saving to file");
        //            }

        //        }
        //    }
        //}
        public async void Execute(object parameter)
        {
            if (lnvm.SelectedNote != null)              //edit mode
            {
                savedTitle = lnvm.Title;                 //Get the content and title of the selected note               
                savedContent = lnvm.Content;
                LocalNoteSqlite.EdidNote(savedTitle, savedContent);         //save the new content 
                editNewNote?.Invoke(this, new EventArgs());             //trager the editNewNote event


            }
            else
            {                                                   //save mode

                SaveCommandDialog snd = new SaveCommandDialog();
                ContentDialogResult result = await snd.ShowAsync();

                if (result == ContentDialogResult.Primary)          //after confirm storage
                {
                    try
                    {
                        savedTitle = snd.NewTitle;                  
                        savedContent = lnvm.Content;

                        //Execute the insert command when a note with the same name does not exist in the database
                        if (LocalNoteSqlite.checkTitle(savedTitle))   
                        {
                            LocalNoteSqlite.AddNote(snd.NewTitle, savedContent);   //save the new title and new content 
                            newNote = new NoteModel(snd.NewTitle, savedContent);
                            createdNewNote?.Invoke(this, EventArgs.Empty);
                            lnvm.SelectedNote = newNote;
                            ContentDialog savedDialog = new ContentDialog()
                            {
                                Title = "Save Successful",
                                Content = "Names saved successfully to file, hurray!",
                                PrimaryButtonText = "OK"
                            };
                            await savedDialog.ShowAsync();
                        }
                        else {
                            ContentDialog savedDialog = new ContentDialog()
                            {
                                Title = "Save Unsuccessful",
                                Content = "A Note with the same name already exists",
                                PrimaryButtonText = "OK"
                            };
                            await savedDialog.ShowAsync();

                        }





                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error when saving to file");
                    }

                }
            }
        }
        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
