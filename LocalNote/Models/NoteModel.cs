using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote.Models
{
    public class NoteModel
    {
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }

        public NoteModel(string noteTitle, string noteContent) { 
            NoteTitle = noteTitle;
            NoteContent = noteContent;
        }
    }
}
