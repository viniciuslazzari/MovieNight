using CinemaApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Session> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Sessions.FirstOrDefaultAsync(session => session.Id == id, cancellationToken);
        }

        public async Task Create(Session newSession, CancellationToken cancellationToken = default)
        {
            await _dbContext.Sessions.AddAsync(newSession, cancellationToken);
        }
        public void CreateOrUpdate(Session modifiedSession)
        {
            _dbContext.Sessions.Update(modifiedSession);
        }


        public async Task Commit(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
