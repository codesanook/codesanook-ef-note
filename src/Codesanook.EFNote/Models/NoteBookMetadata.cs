using System.ComponentModel.DataAnnotations.Schema;

namespace Codesanook.EFNote.Models
{
    public class Settings
    {
        public string ColorTheme { get; set; }

        [NotMapped]
        public SupportedFileFormat[] SupportedFileFormats { get; set; }
    }

    public class SupportedFileFormat
    {
        public string FileFormat { get; set; }
        public string Editor { get; set; }
    }

    // Notebook
    // Metadata

}
