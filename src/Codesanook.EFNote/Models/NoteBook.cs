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
        public Settings Settings { get; set; }
    }

    public class Settings
    {
        public string ColorTheme { get; set; }
        public List<SupportedFileFormat> SupportedFileFormats { get; set; }
    }

    public class SupportedFileFormat
    {
        public string FileFormat { get; set; }
        public string Editor { get; set; }
    }
}
