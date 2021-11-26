using Codesanook.EFNote.Models;
using System;
using System.Collections.Generic;

namespace Codesanook.EFNote.ViewModels
{
    public class NotebookIndexViewModel
    {
        public Notebook Notebook { get; set; }
        public DateTime ValidFrom {get;set;}
        public DateTime ValidTo {get;set;}
    }
}
