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
    internal class EditCommand : ICommand
    {
        private ViewModels.LocalNoteViewModel lnvm;
        
        public event EventHandler CanExecuteChanged;

        public EditCommand(ViewModels.LocalNoteViewModel lnvm)
        {
            this.lnvm = lnvm;
        }

        public bool CanExecute(object parameter)
        {
            return lnvm.textboxStatus();
        }

        public void Execute(object parameter)
        {
            lnvm.MainPage.textUnlock();                      //unlock textbox
            lnvm.SaveCommand.FireCanExecuteChanged();        //Change the executable of savecommand
            lnvm.EditCommand.FireCanExecuteChanged();        //Change the executable of editcoomand 

        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
