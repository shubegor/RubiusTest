using System.Windows.Input;
using RubiusTest.Models;

namespace RubiusTest.ViewModels
{
    public class NoteEditorViewModel 
    {
        public NoteModel Note { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public NoteEditorViewModel(NoteModel note, ICommand okCommand, ICommand cancelCommand)
        {
            this.Note = note;
            this.OkCommand = okCommand;
            this.CancelCommand = cancelCommand;
        }
    }
}
