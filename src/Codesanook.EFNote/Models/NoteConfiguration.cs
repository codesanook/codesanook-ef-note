using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Codesanook.EFNote.Models
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder
                .ToTable("note")
                .Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(1024);

            builder
                .HasMany(e => e.Tags)
                .WithMany(e => e.Notes)
                .UsingEntity(j => j.ToTable("note_tag"));
                // .UsingEntity<Dictionary<string, object>>(
                //     "note_tag"
                // );

        }
    }
}
