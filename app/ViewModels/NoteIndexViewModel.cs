using Codesanook.EFNote.Models;
using System.Collections.Generic;

namespace Codesanook.EFNote.ViewModels
{
    public class NoteIndexViewModel
    {
        public IReadOnlyCollection<Notebook> AllNotebooks { get; set; }
        public Notebook SelectedNotebook { get; set; }
        public IReadOnlyCollection<NoteViewModel> AllNotesForSelectedNotebook { get; set; }
        public NoteViewModel SelectedNote { get; set; }
    }
}
