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
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly MoviesRepository _moviesRepository;

        public MoviesController(ILogger<MoviesController> logger, MoviesRepository moviesRepository)
        {
            _logger = logger;
            _moviesRepository = moviesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var movies = await _moviesRepository.GetAll(cancellationToken);

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var movie = await _moviesRepository.GetById(guid, cancellationToken);

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewMovieInputModel inputModel, CancellationToken cancellationToken)
        {
            var newMovie = Movie.Create(inputModel);
            if (newMovie.IsFailure)
            {
                _logger.LogInformation($"Error: {newMovie.Error}");
                return BadRequest(newMovie.Error);
            }

            await _moviesRepository.Create(newMovie.Value, cancellationToken);
            await _moviesRepository.Commit(cancellationToken);

            _logger.LogInformation($"Movie {newMovie.Value.Id} created successfully");

            return CreatedAtAction("GetById", new { id = newMovie.Value.Id }, newMovie.Value.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateMovieInputModel inputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var oldMovie = await _moviesRepository.GetById(guid, cancellationToken);

            if (oldMovie == null)
                return NotFound();

            var existingSessions =
                oldMovie.Sessions
                    .Where(c => inputModel.Sessions.Any(input => input.Id == c.Id.ToString()))
                    .Select(c => c.Id);

            var deletedSessions =
                oldMovie.Sessions
                    .Where(c => existingSessions.Any(id => id != c.Id))
                    .Select(c => c.Id);

            oldMovie.DeleteSessions(deletedSessions);

            foreach (var session in inputModel.Sessions)
            {
                if (!DateTime.TryParse(session.Date, out var date))
                    return BadRequest("Session Datetime could not be converted");

                if (string.IsNullOrEmpty(session.Id))
                {
                    oldMovie.AddSession(date, session.MaxOccupation, session.Price);
                }
                else
                {
                    if (!Guid.TryParse(id, out var sessionGuid))
                        return BadRequest("Session ID could not be converted");

                    oldMovie.UpdateSession(sessionGuid, date, session.MaxOccupation, session.Price);
                }
            }

            _moviesRepository.Update(oldMovie);
            await _moviesRepository.Commit(cancellationToken);

            _logger.LogInformation($"Movie {oldMovie.Id} updated successfully");

            return Ok(oldMovie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var removedMovie = await _moviesRepository.GetById(guid, cancellationToken);

            if (removedMovie == null)
                return NotFound();

            _moviesRepository.Delete(removedMovie);
            await _moviesRepository.Commit(cancellationToken);

            _logger.LogInformation($"Movie {removedMovie.Id} deleted successfully");

            return Ok("Movie removed successfully!");
        }
    }
}
