using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LocalNote
{
    public class AddCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.LocalNoteViewModel lnvm;

        public bool addActivity=false;

        public AddCommand(ViewModels.LocalNoteViewModel lnvm) { 
            this.lnvm = lnvm;
        }

        public bool CanExecute(object parameter) {
            return true;
        
        }

        public  void Execute(object parameter)
        {
            addActivity = true;
            lnvm.SaveCommand.FireCanExecuteChanged();

            lnvm.SelectedNote = null;
            
           
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
