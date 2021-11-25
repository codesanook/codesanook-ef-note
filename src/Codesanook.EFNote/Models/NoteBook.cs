using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codesanook.EFNote.Models
{
    public class Notebook : EntityBase
    {
        public Notebook() => Notes = new HashSet<Note>();

        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
