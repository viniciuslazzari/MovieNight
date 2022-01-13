using CinemaApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaApi.Infrastructure
{
    public class TicketsRepository
    {
        private readonly MovieNightDbContext _dbContext;

        public TicketsRepository(MovieNightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Ticket>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tickets.ToListAsync(cancellationToken);
        }

        public async Task<Ticket> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tickets.FirstOrDefaultAsync(ticket => ticket.Id == id, cancellationToken);
        }

        public async Task Create(Ticket newTicket, CancellationToken cancellationToken = default)
        {
            await _dbContext.Tickets.AddAsync(newTicket, cancellationToken);
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
