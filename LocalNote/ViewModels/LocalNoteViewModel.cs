using LocalNote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LocalNote.ViewModels
{
    public class LocalNoteViewModel
    {
        public ObservableCollection<NoteModel> LocalNotes { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        public string Title { get; set; }
        public string Content { get; set; }

        private string _filter;

        private List<NoteModel> _allLocalNotes = new List<NoteModel>();
        private NoteModel _selectedNote;



        public LocalNoteViewModel() {

            LocalNotes=new ObservableCollection<NoteModel>();
            for (int i = 1; i < 10; i++) {
               NoteModel note = new NoteModel( "Note "+i, "Just for test: Here is Note "+i);
                _allLocalNotes.Add(note);
            }
            PerformFiltering();
        }

        public NoteModel SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                if (value == null)
                {
                    Content = "Wrong";
                }
                else
                {
                    Title = value.NoteTitle;

                    Content = value.NoteContent;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Content"));
                //Event to call the save functionality
                //AcceptCommand.FireCanExecuteChanged();
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

        private void PerformFiltering()
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
