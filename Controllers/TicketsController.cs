using CinemaApi.Domain;
using CinemaApi.Infrastructure;
using CinemaApi.Models;
using CinemaApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
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

            return Ok(new SuccessJsonResponse(tickets));
        }

        [HttpGet("{id}")]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest(new ErrorJsonResponse("ID could not be converted"));

            var ticket = await _ticketsRepository.GetById(guid, cancellationToken);

            return Ok(new SuccessJsonResponse(ticket));
        }

        [HttpPost]
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Post([FromBody] NewTicketInputModel inputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(inputModel.SessionId, out var guid))
                return BadRequest(new ErrorJsonResponse("Session ID could not be converted"));

            var ticketSession = await _sessionsRepository.GetById(guid, cancellationToken);
            var soldTickets = ticketSession.Tickets.Select(x => x.Amount).Sum();

            if (soldTickets == ticketSession.MaxOccupation)
                return BadRequest(new ErrorJsonResponse("Session is already full"));

            var restTickets = ticketSession.MaxOccupation - soldTickets;

            if (restTickets < inputModel.Amount)
                return BadRequest(
                    new ErrorJsonResponse($"There are not enough tickets to purchase. You can buy only {restTickets} tickets!"));

            var ticket = Ticket.Create(inputModel);
            if (ticket.IsFailure)
            {
                _logger.LogInformation($"Error: {ticket.Error}");
                return BadRequest(new ErrorJsonResponse(ticket.Error));
            }

            await _ticketsRepository.Create(ticket.Value, cancellationToken);
            await _ticketsRepository.Commit(cancellationToken);

            _logger.LogInformation($"Ticket {ticket.Value.Id} sold successfully");

            return Ok(new SuccessJsonResponse("Ticket sold successfully!", ticket));
        }
    }
}
