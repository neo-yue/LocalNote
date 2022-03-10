using LocalNote.Command;
using LocalNote.Models;
using LocalNote.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace LocalNote.ViewModels
{
    public class LocalNoteViewModel : INotifyPropertyChanged
    {
        public MainPage MainPage { get; set; }
        
        public AddCommand AddCommand { get; }
        public SaveCommand SaveCommand { get; } 

       // public LocalNoteRepo localNoteRepo { get; }

        public ObservableCollection<NoteModel> LocalNotes { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

       // private string _content;    
        public string Title { get; set; }
        
       

        private string _content;

        public string Content { get { return _content; } set { _content = MainPage.NoteContent; } }

        

        private string _filter;

        // private ObservableCollection<NoteModel> _allLocalNotes = new ObservableCollection<NoteModel>();

        private List<NoteModel> _allLocalNotes = new List<NoteModel>();
        private NoteModel _selectedNote;

        public void Refresh(NoteModel newNote) {

            _selectedNote = newNote;
            SelectedNote = _selectedNote;
        }

        public LocalNoteViewModel() {

            AddCommand = new AddCommand(this);
            SaveCommand = new SaveCommand(this);
            LocalNotes=new ObservableCollection<NoteModel>();
            _allLocalNotes= LocalNoteRepo.ReadNote();
            
            PerformFiltering();

        }

        public bool textboxStatus()
        {

            return MainPage.textboxStatus();
        }



        public NoteModel SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                
                if (value == null)
                {
                    MainPage.CleanTextbox();
                    Title = "Untitiled Note";
                   
                    
                }
                else
                {
                    Title = value.NoteTitle;

                    _content = value.NoteContent;
                    MainPage.textLock();
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Content"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("_selectedNote"));
                //Event to call the save functionality
                AddCommand.FireCanExecuteChanged();
                SaveCommand.FireCanExecuteChanged();
            }
           
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) { return; }
                _filter = value;
                PerformFiltering();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
            }
        }

        public void PerformFiltering()
        {
            if (_filter == null)
            {
                _filter = "";
            }
            //If _filter has a value (ie. user entered something in Filter textbox)
            //Lower-case and trim string
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            //Use LINQ query to get all personmodel names that match filter text, as a list
            var result =
                _allLocalNotes.Where(d => d.NoteTitle.ToLowerInvariant()
                .Contains(lowerCaseFilter))
                .ToList();

            //Get list of values in current filtered list that we want to remove
            //(ie. don't meet new filter criteria)
            var toRemove = LocalNotes.Except(result).ToList();

            //Loop to remove items that fail filter
            foreach (var x in toRemove)
            {
                LocalNotes.Remove(x);
            }

            var resultCount = result.Count;
            // Add back in correct order.
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > LocalNotes.Count || !LocalNotes[i].Equals(resultItem))
                {
                    LocalNotes.Insert(i, resultItem);
                }
            }
        }

    }
}
