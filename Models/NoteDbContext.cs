using Microsoft.EntityFrameworkCore;

namespace Codesanook.EFNote.Models
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options) { }

        public virtual DbSet<Notebook> Notebooks { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notebook>()
                .ToTable("notebook")
                .HasMany(e => e.Notes)
                .WithOne(e => e.Notebook)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Note>()
                .ToTable("note")
                .HasMany(e => e.Tags)
                .WithMany(e => e.Notes)
                .UsingEntity(j => j.ToTable("note_tags"));
        }
    }
}
