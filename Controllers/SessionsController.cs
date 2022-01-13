using CinemaApi.Domain;
using CinemaApi.Infrastructure;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ILogger<SessionsController> _logger;
        private readonly SessionsRepository _sessionsRepository;

        public SessionsController(ILogger<SessionsController> logger, SessionsRepository moviesRepository)
        {
            _logger = logger;
            _sessionsRepository = moviesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var sessions = await _sessionsRepository.GetAll(cancellationToken);

            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var session = await _sessionsRepository.GetById(id, cancellationToken);

            return Ok(session);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewSessionInputModel inputModel, CancellationToken cancellationToken)
        {
            var newSession = Session.Create(inputModel);
            if (newSession.IsFailure)
                return BadRequest(newSession.Error);

            await _sessionsRepository.Create(newSession.Value, cancellationToken);
            await _sessionsRepository.Commit(cancellationToken);

            return CreatedAtAction("GetById", new { id = newSession.Value.Id }, newSession.Value.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateSessionInputModel inputModel, CancellationToken cancellationToken)
        {
            var oldSession = await _sessionsRepository.GetById(id, cancellationToken);

            if (oldSession == null)
                return NotFound();

            // BUG ESTRANHO AQUI
            var existingTickets =
                oldSession.Tickets
                    .Where(c => inputModel.Tickets.Any(input => input.Id == c.Id))
                    .Select(c => c.Id);

            var deletedTickets =
                oldSession.Tickets
                    .Where(c => existingTickets.Any(id => id != c.Id))
                    .Select(c => c.Id);

            oldSession.DeleteTickets(deletedTickets);

            foreach (var ticket in inputModel.Tickets)
            {
                if (ticket.Id == Guid.Empty)
                {
                    oldSession.AddTicket(ticket.Client, ticket.Amount);
                }
                else
                {
                    oldSession.UpdateTicket(ticket.Id, ticket.Client, ticket.Amount);
                }
            }

            _sessionsRepository.Update(id, inputModel, cancellationToken);
            await _sessionsRepository.Commit(cancellationToken);

            return Ok(oldSession);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var removedSession = await _sessionsRepository.GetById(id, cancellationToken);

            if (removedSession == null)
                return NotFound();

            _sessionsRepository.Delete(removedSession);
            await _sessionsRepository.Commit(cancellationToken);

            return Ok("Session removed successfully!");
        }
    }
}
