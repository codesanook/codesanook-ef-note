using Microsoft.EntityFrameworkCore;
// using MySql.EntityFrameworkCore.Extensions;

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

            //modelBuilder.Entity<Notebook>();
            //.ForMySQLHasCollation("utf8mb4_unicode_ci")
            //.ForMySQLHasCharset("utf8mb4");


            //.ForMySQLHasCollation("utf8mb4_unicode_ci")
            //.ForMySQLHasCharset("utf8mb4");

            modelBuilder.Entity<Tag>()
                .ToTable("tag");
            //.ForMySQLHasCollation("utf8mb4_unicode_ci")
            //.ForMySQLHasCharset("utf8mb4");
            modelBuilder.ApplyConfiguration(new NoteConfiguration());

        }
    }
}
