using CinemaApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Infrastructure
{
    public class MovieNightDbContext : DbContext
    {
        public MovieNightDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
