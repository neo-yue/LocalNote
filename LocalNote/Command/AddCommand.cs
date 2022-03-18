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
        public event EventHandler CancellSelected;

        public AddCommand(ViewModels.LocalNoteViewModel lnvm) { 
            this.lnvm = lnvm;
        }

        public bool CanExecute(object parameter) {
            return lnvm.textboxStatus();

        }

        public  void Execute(object parameter)
        {
         
            lnvm.MainPage.textUnlock();                         //unlock textbox
            lnvm.SaveCommand.FireCanExecuteChanged();           //Change the executable of savecommand
            CancellSelected?.Invoke(this, EventArgs.Empty);         //trigger Cancellselected event
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
