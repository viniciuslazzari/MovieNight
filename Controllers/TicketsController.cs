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
    public class TicketsController : ControllerBase
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly TicketsRepository _ticketsRepository;
        private readonly SessionsRepository _sessionsRepository;

        public TicketsController(ILogger<TicketsController> logger, TicketsRepository ticketsRepository, SessionsRepository sessionsRepository)
        {
            _logger = logger;
            _ticketsRepository = ticketsRepository;
            _sessionsRepository = sessionsRepository;
        }

        [HttpGet]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var tickets = await _ticketsRepository.GetAll(cancellationToken);

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var ticket = await _ticketsRepository.GetById(guid, cancellationToken);

            return Ok(ticket);
        }

        [HttpPost]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Post([FromBody] NewTicketInputModel inputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(inputModel.SessionId, out var guid))
                return BadRequest("Session ID could not be converted");

            var ticketSession = await _sessionsRepository.GetById(guid, cancellationToken);
            var soldTickets = ticketSession.Tickets.Select(x => x.Amount).Sum();

            if (soldTickets == ticketSession.MaxOccupation)
                return BadRequest("Session is already full");

            var restTickets = ticketSession.MaxOccupation - soldTickets;

            if (restTickets < inputModel.Amount)
                return BadRequest($"There are not enough tickets to purchase. You can buy only {restTickets} tickets!");

            var newTicket = Ticket.Create(inputModel);
            if (newTicket.IsFailure)
            {
                _logger.LogInformation($"Error: {newTicket.Error}");
                return BadRequest(newTicket.Error);
            }

            await _ticketsRepository.Create(newTicket.Value, cancellationToken);
            await _ticketsRepository.Commit(cancellationToken);

            return CreatedAtAction("GetById", new { id = newTicket.Value.Id }, newTicket.Value.Id);
        }
    }
}
