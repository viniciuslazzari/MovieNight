using CinemaApi.Domain;
using CinemaApi.Infrastructure;
using CinemaApi.Models;
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
    public class SessionsController : ControllerBase
    {
        private readonly ILogger<SessionsController> _logger;
        private readonly SessionsRepository _sessionsRepository;

        public SessionsController(ILogger<SessionsController> logger, SessionsRepository sessionsRepository)
        {
            _logger = logger;
            _sessionsRepository = sessionsRepository;
        }

        [HttpGet]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetAll([FromQuery] string movie, [FromQuery] string date, CancellationToken cancellationToken)
        {
            var sessions = await _sessionsRepository.GetAll(cancellationToken);

            if (!string.IsNullOrEmpty(movie))
            {
                if (!Guid.TryParse(movie, out var movieGuid))
                    return BadRequest("Movie ID could not be converted");

                sessions = sessions.Where(session => session.MovieId == movieGuid);
            }

            if (!string.IsNullOrEmpty(date))
            {
                if (!DateTime.TryParse(date, out var datetime))
                    return BadRequest();

                sessions = sessions.Where(session => session.Date == datetime);
            }

            return Ok(sessions);
        }

        [HttpGet("{id}")]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var session = await _sessionsRepository.GetById(guid, cancellationToken);

            return Ok(session);
        }

        [HttpPost]
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Post([FromBody] NewSessionInputModel inputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(inputModel.MovieId, out var guid))
                return BadRequest("Movie ID could not be converted");

            var newSession = Session.Create(inputModel);
            if (newSession.IsFailure)
            {
                _logger.LogInformation($"Error: {newSession.Error}");
                return BadRequest(newSession.Error);
            }

            await _sessionsRepository.Create(newSession.Value, cancellationToken);
            await _sessionsRepository.Commit(cancellationToken);

            _logger.LogInformation($"Session {newSession.Value.Id} created successfully");

            return CreatedAtAction("GetById", new { id = newSession.Value.Id }, newSession.Value.Id);
        }

        [HttpPut("{id}")]
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateSessionInputModel inputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var session = await _sessionsRepository.GetById(guid, cancellationToken);

            if (session == null)
                return NotFound();

            session.Update(inputModel);
            _sessionsRepository.Update(session);
            await _sessionsRepository.Commit(cancellationToken);

            _logger.LogInformation($"Session {session.Id} updated successfully");

            return Ok(session);
        }

        [HttpDelete("{id}")]
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var session = await _sessionsRepository.GetById(guid, cancellationToken);

            if (session == null)
                return NotFound();

            _sessionsRepository.Delete(session);
            await _sessionsRepository.Commit(cancellationToken);

            _logger.LogInformation($"Session {session.Id} deleted successfully");

            return Ok("Session removed successfully!");
        }
    }
}
