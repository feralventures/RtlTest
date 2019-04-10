using RtlTestRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace RtlTestRepository
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Show>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Show>()
                .HasIndex(s => s.TvMazeId);

            modelBuilder.Entity<Show>()
                .HasMany<Person>(s => s.Cast)
                .WithOne(p => p.Show)
                .HasForeignKey(p => p.ShowId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Show> Show { get; set; }
    }
}
