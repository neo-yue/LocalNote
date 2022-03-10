using LocalNote.Models;
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
        public MainPage MainPage { get; set; }

        public SaveCommand(ViewModels.LocalNoteViewModel lnvm) { 
        this.lnvm = lnvm;
        }

        public bool CanExecute(object parameter)
        {
            return !lnvm.textboxStatus();

        }

        public async void Execute(object parameter)
        {
            SaveCommandDialog snd = new SaveCommandDialog();
            ContentDialogResult result = await snd.ShowAsync();
            
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    savedTitle = snd.NewTitle;
                    savedContent = lnvm.Content;
                    Repositories.LocalNoteRepo.SaveNoteToFile(snd.NewTitle, savedContent);
                    newNote = new NoteModel(snd.NewTitle, savedContent);
                    lnvm.LocalNotes.Add(newNote);

                    ContentDialog savedDialog = new ContentDialog()
                    {
                        Title = "Save Successful",
                        Content = "Names saved successfully to file, hurray!",
                        PrimaryButtonText = "OK"
                    };
                    await savedDialog.ShowAsync();
                    
                    lnvm.Refresh(newNote);

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
