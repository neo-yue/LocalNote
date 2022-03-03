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

        public SaveCommand(ViewModels.LocalNoteViewModel lnvm) { 
        this.lnvm = lnvm;
        }

        public bool CanExecute(object parameter)
        {
            return lnvm.AddCommand.addActivity;

        }

        public async void Execute(object parameter)
        {
            SaveCommandDialog snd = new SaveCommandDialog();
            ContentDialogResult result = await snd.ShowAsync();
            
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    Repositories.LocalNoteRepo.SaveNoteToFile(snd.NewTitle, "testContent");

                    savedTitle = snd.NewTitle;
                    savedContent = "testContent";
                    lnvm.Add(lnvm.SaveCommand.savedTitle, "testContent");
                    //ContentDialog savedDialog = new ContentDialog()
                    //{
                    //    Title = "Save Successful",
                    //    Content = "Names saved successfully to file, hurray!",
                    //    PrimaryButtonText = "OK"
                    //};
                    //await savedDialog.ShowAsync();
                    

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error when saving to file");
                }
            }   
        }
        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
