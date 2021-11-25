using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codesanook.EFNote.Models
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder
                .ToTable("note");
                //.ForMySQLHasCollation("utf8mb4_unicode_ci")
                //.ForMySQLHasCharset("utf8mb4");

            builder
                .HasMany(e => e.Tags)
                .WithMany(e => e.Notes)
                .UsingEntity(j => j.ToTable("note_tag"));
        }
    }
}
