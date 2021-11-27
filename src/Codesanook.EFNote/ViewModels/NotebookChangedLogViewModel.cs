using Codesanook.EFNote.Models;
using System;

namespace Codesanook.EFNote.ViewModels
{
    public class NotebookChangedLogViewModel
    {
        public Notebook Notebook { get; set; }
        public DateTime ValidFrom {get;set;}
        public DateTime ValidTo {get;set;}
    }
}
