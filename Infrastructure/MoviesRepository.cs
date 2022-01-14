using CinemaApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Movie>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Movies.Include(c => c.Sessions).ToListAsync(cancellationToken);
        }

        public async Task<Movie> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Movies.Include(c => c.Sessions).FirstOrDefaultAsync(movie => movie.Id == id, cancellationToken);
        }

        public async Task Create(Movie newMovie, CancellationToken cancellationToken = default)
        {
            await _dbContext.Movies.AddAsync(newMovie, cancellationToken);
        }

        public void Update(Movie movie)
        {


        }

        public void Delete(Movie deletedMovie)
        {
            _dbContext.Movies.Remove(deletedMovie);
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
