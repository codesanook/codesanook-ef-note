using Microsoft.EntityFrameworkCore;

namespace Codesanook.EFNote.Models
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options) { }

        public virtual DbSet<Notebook> Notebooks  => Set<Notebook>();
        public virtual DbSet<Note> Notes => Set<Note>();
        public virtual DbSet<Tag> Tags => Set<Tag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Notebook>()
                .ToTable("notebook", t => t.IsTemporal())
                .HasMany(e => e.Notes)
                .WithOne(e => e.Notebook)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Tag>()
                .ToTable("tag", t => t.IsTemporal());

            modelBuilder.ApplyConfiguration(new NoteConfiguration());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveMaxLength(32);
        }
    }
}
