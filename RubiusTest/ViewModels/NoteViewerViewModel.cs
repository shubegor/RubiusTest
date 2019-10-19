using System.Windows.Input;
using RubiusTest.Models;

namespace RubiusTest.ViewModels
{
    public class NoteViewerViewModel
    {
        public NoteModel Note { get; set; }
        
        public NoteViewerViewModel(NoteModel note)
        {
            this.Note = note;
        }
    }
}
