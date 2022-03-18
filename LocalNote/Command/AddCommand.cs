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
        //public bool addActivity=false;

        public AddCommand(ViewModels.LocalNoteViewModel lnvm) { 
            this.lnvm = lnvm;
        }

        public bool CanExecute(object parameter) {
            return lnvm.textboxStatus();

        }

        public  void Execute(object parameter)
        {
         
            lnvm.MainPage.textUnlock();
            lnvm.SaveCommand.FireCanExecuteChanged();
            CancellSelected?.Invoke(this, EventArgs.Empty);
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
