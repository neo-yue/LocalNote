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

        //that function only use for unit test
        List<NoteModel> List { get; set;} = new List<NoteModel>();
        public static String getContent(List<NoteModel> List,String title) {

            foreach (var item in List) {
                if (item.NoteTitle == title) {
                    return item.NoteContent;
                }
            }
            return null;
        }
    }
}
