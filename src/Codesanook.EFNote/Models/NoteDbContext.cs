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
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>().ToTable("tag");

            modelBuilder.ApplyConfiguration(new NoteConfiguration());
        }
    }
}
