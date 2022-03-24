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
        
        public AddCommand AddCommand { get; }      //use to add new to a file    
        public SaveCommand SaveCommand { get; }   //used to save a note to a file
        public EditCommand EditCommand { get; }  //used to edit the selected note 
        public DeleteCommand DeleteCommand { get; } //used to delete the sellected note

        public ExitCommand ExitCommand { get; }
        public ObservableCollection<NoteModel> LocalNotes { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; set; }

        private string _content;

        public string Content { get { return _content; } set { _content = MainPage.NoteContent; } }

        

        private string _filter;                                          //Private property is used to store filter key word 

        private List<NoteModel> _allLocalNotes = new List<NoteModel>();  //Private property is used to store all notes
        private NoteModel _selectedNote;

        public void UnSellected(object sender, EventArgs e) {                       //Unsellected Note
            SelectedNote = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));    
        }

        public void UpdateNote(object sender, EventArgs e)                      //update note content after edit
        {
            SelectedNote.NoteContent = MainPage.NoteContent;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));

        }

        public void AddNewNote(object sender, EventArgs e) {            // add new note to _allLocalNotes
            if (SaveCommand.newNote != null) { 
            _allLocalNotes.Add(SaveCommand.newNote);
            PerformFiltering();

            }
        }

        public void DeleteSellected(object sender, EventArgs e)
        {                       
            _allLocalNotes.Remove(SelectedNote);
            PerformFiltering();
            SelectedNote = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));
        }

        public LocalNoteViewModel() {

            AddCommand = new AddCommand(this);
            SaveCommand = new SaveCommand(this);
            EditCommand = new EditCommand(this);  
            DeleteCommand = new DeleteCommand(this);
            ExitCommand = new ExitCommand();

            LocalNotes =new ObservableCollection<NoteModel>();
            _allLocalNotes= LocalNoteRepo.LoadNote();
            SaveCommand.createdNewNote += AddNewNote;                       //Execute function after event is triggered
            AddCommand.UnSellected += UnSellected;
            SaveCommand.editNewNote += UpdateNote;
            DeleteCommand.DeleteSelected += DeleteSellected;
            PerformFiltering();

        }

        public bool textboxStatus()         //Editability of content textbox
        {

            return MainPage.textboxStatus();        
        }



        public NoteModel SelectedNote       //Two-Way bound to currently selected note
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                
                if (value == null)
                {
                    MainPage.CleanTextbox();                //Clear the content in the content textbox when it is not selected, and set the title to Untitiled Note
                    Title = "Untitiled Note";
                    MainPage.textUnlock();
                   
                    
                }
                else
                {
                    Title = value.NoteTitle;                
                     _content = value.NoteContent;
                    MainPage.textLock();
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Content"));

                //Check the executable of all commands
                AddCommand.FireCanExecuteChanged();
                SaveCommand.FireCanExecuteChanged();
                EditCommand.FireCanExecuteChanged();
                DeleteCommand.FireCanExecuteChanged();
            }
           
        }

        public string Filter                //that's Two-Way bound to search bar
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
