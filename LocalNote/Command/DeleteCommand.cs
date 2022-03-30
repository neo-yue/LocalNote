using LocalNote.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LocalNote.Command
{
    public class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.LocalNoteViewModel lnvm;
        public event EventHandler DeleteSelected;
       
        public DeleteCommand(ViewModels.LocalNoteViewModel lnvm) { 
        this.lnvm = lnvm;
        }


        public bool CanExecute(object parameter)        //Executable when note is selected
        {
           return lnvm.SelectedNote != null;
        }

        //public async void Execute(object parameter)
        //{
        //    ContentDialog confirmDelete = new ContentDialog         //Confirm delete again
        //    { 
        //        Title = "Confirm Delete",
        //        Content = "Are you sure you want to delete?",
        //        PrimaryButtonText = "Confirm", 
        //        SecondaryButtonText = "Cancel" 
        //    };
        //    ContentDialogResult result = await confirmDelete.ShowAsync();

        //    if (result == ContentDialogResult.Primary)
        //    {
        //        try
        //        {
        //            LocalNoteRepo.DeleteNote(lnvm.SelectedNote.NoteTitle);    //delete note
        //            DeleteSelected?.Invoke(this, EventArgs.Empty);                 //trigger DeleteSelected event
        //        }
        //        catch (Exception)
        //        {
        //            ContentDialog errorDialog = new ContentDialog
        //            { 
        //                Title = "Error",
        //                Content = "There was an error deleting the file", 
        //                PrimaryButtonText = "OK" };
        //            await errorDialog.ShowAsync();
        //        }

        //    }
        //}

        public async void Execute(object parameter)
        {
            ContentDialog confirmDelete = new ContentDialog         //Confirm delete again
            {
                Title = "Confirm Delete",
                Content = "Are you sure you want to delete?",
                PrimaryButtonText = "Confirm",
                SecondaryButtonText = "Cancel"
            };
            ContentDialogResult result = await confirmDelete.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    LocalNoteSqlite.DeleteNote(lnvm.SelectedNote.NoteTitle);    //delete note
                    DeleteSelected?.Invoke(this, EventArgs.Empty);                 //trigger DeleteSelected event
                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "There was an error deleting the file",
                        PrimaryButtonText = "OK"
                    };
                    await errorDialog.ShowAsync();
                }

            }
        }



        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }


}
