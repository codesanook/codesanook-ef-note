using Microsoft.EntityFrameworkCore;

namespace Codesanook.EFNote.Models
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options) { }

        public virtual DbSet<Notebook> Notebooks => Set<Notebook>();
        public virtual DbSet<Note> Notes => Set<Note>();
        public virtual DbSet<Tag> Tags => Set<Tag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Notebook>()
                .ToTable("notebook", e => e.IsTemporal())
                .HasMany(e => e.Notes)
                .WithOne(e => e.Notebook)
                .OnDelete(DeleteBehavior.Cascade);

            // Map Metadata to JSON column
            modelBuilder
                .Entity<Notebook>()
                .OwnsOne(e => e.Metadata)
                .ToJson();

            modelBuilder
                .Entity<Tag>()
                .ToTable("tag");

            // Setting for Note
            modelBuilder.ApplyConfiguration(new NoteConfiguration());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
            configurationBuilder.Properties<string>().HaveMaxLength(32);
    }
}
