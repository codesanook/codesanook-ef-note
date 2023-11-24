using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codesanook.EFNote.Models
{
    public class Notebook : EntityBase
    {
        public Notebook() => Notes = new HashSet<Note>();
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Note> Notes { get; set; }

        [NotMapped]
        public Settings Settings { get; set; }
    }
}
