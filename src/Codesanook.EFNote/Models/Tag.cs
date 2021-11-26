using System.Collections.Generic;

namespace Codesanook.EFNote.Models
{
    public class Tag:EntityBase
    {
        public string Name { get; set; }
        public Tag() => Notes = new HashSet<Note>();
        public virtual ICollection<Note> Notes { get; set; }
    }
}
