using System.Collections.Generic;

namespace Codesanook.EFNote.Models
{
    public class Notebook : EntityBase
    {
        public Notebook() => Notes = new HashSet<Note>();
        public string Name { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
