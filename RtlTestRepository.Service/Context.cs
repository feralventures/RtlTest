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
            modelBuilder.Entity<Person>().HasKey(p => p.Id);

            modelBuilder.Entity<Show>().HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Show> Show { get; set; }
    }
}
