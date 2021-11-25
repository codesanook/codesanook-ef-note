using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Codesanook.EFNote.Models
{
    public class Note : EntityBase
    {
        public Note() => Tags = new HashSet<Tag>();
        public string Title { get; set; }
        public string Content { get; set; }

        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)] does not work now for MS SQL 2016 + EF Core 6
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public bool IsDeleted { get; set; }
        public int ViewCount { get; set; }

        public int NotebookId { get; set; }
        public virtual Notebook Notebook { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public override string ToString()
        {
            return string.Join(
                Environment.NewLine,
                new[]{
                    $"{nameof(Id)}: {Id}",
                    $"{nameof(Title)}: {Title}",
                    $"{nameof(Content)}: {Content}",
                    $"{nameof(CreatedUtc)}: {CreatedUtc}",
                    $"{nameof(UpdatedUtc)}: {UpdatedUtc}",
                    $"{nameof(ViewCount)}: {ViewCount}",
                    $"{nameof(NotebookId)}: {NotebookId}",

                    $"Notebook name: {this.Notebook.Name}",
                    $"Tag names: {string.Join(", ", this.Tags.Select(t=>t.Name))}",
                }
            );
        }
    }
}
