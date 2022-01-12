using CinemaApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaApi.Infrastructure
{
    public class MoviesRepository
    {
        private readonly MovieNightDbContext _dbContext;

        public MoviesRepository(MovieNightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Movie newMovie, CancellationToken cancellationToken = default)
        {
            await _dbContext.Movies.AddAsync(newMovie, cancellationToken);
        }

        public async Task<Movie> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id, cancellationToken);
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
