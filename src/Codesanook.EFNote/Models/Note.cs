using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618
namespace Codesanook.EFNote.Models
{
    public class Note : EntityBase
    {
        public Note() => Tags = new HashSet<Tag>();
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedUtc { get; set; }
        public List<DateTime>? UtcUpdates { get; set; }
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
                    $"{nameof(UtcUpdates)}:",
                    $"{string.Join("\n", UtcUpdates ?? new List<DateTime>())}:",
                    $"{nameof(ViewCount)}: {ViewCount}",
                    $"{nameof(NotebookId)}: {NotebookId}",

                    $"Notebook name: {this.Notebook.Name}",
                    $"Tag names: {string.Join(", ", this.Tags.Select(t=>t.Name))}",
                }
            );
        }
    }
}

#pragma warning restore CS8618
