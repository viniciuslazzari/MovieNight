using CinemaApi.Domain;
using CinemaApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaApi.Infrastructure
{
    public class SessionsRepository
    {
        private readonly MovieNightDbContext _dbContext;

        public SessionsRepository(MovieNightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Sessions.Include(c => c.Tickets).ToListAsync(cancellationToken);
        }

        public async Task<Session> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Sessions.Include(c => c.Tickets).FirstOrDefaultAsync(session => session.Id == id, cancellationToken);
        }

        public async Task Create(Session newSession, CancellationToken cancellationToken = default)
        {
            await _dbContext.Sessions.AddAsync(newSession, cancellationToken);
        }

        public void Update(Guid id, UpdateSessionInputModel updatedSession, CancellationToken cancellationToken = default)
        {
            var oldSession = _dbContext.Sessions.Find(id);
            _dbContext.Entry(oldSession).CurrentValues.SetValues(updatedSession);
        }

        public void Delete(Session deletedSession)
        {
            _dbContext.Sessions.Remove(deletedSession);
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
