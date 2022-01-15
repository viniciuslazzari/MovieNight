using CinemaApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public DbSet<Ticket> Tickets { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                foreach (var item in ChangeTracker.Entries())
                {
                    if (item.State is EntityState.Modified or EntityState.Added
                        && item.Properties.Any(c => c.Metadata.Name == "UpdateAt"))
                        item.Property("UpdateAt").CurrentValue = DateTime.Now;

                    if (item.State == EntityState.Added
                        && item.Properties.Any(c => c.Metadata.Name == "CreatedAt"))
                        item.Property("CreatedAt").CurrentValue = DateTime.Now;
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException e)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
