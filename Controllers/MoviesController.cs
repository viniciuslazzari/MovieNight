using CinemaApi.Domain;
using CinemaApi.Infrastructure;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var movie = await _moviesRepository.GetById(id, cancellationToken);

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewMovieInputModel inputModel, CancellationToken cancellationToken)
        {
            var newMovie = Movie.Create(inputModel);
            if (newMovie.IsFailure)  
                return BadRequest(newMovie.Error);

            await _moviesRepository.Create(newMovie.Value, cancellationToken);
            await _moviesRepository.Commit(cancellationToken);

            return CreatedAtAction("GetById", new { id = newMovie.Value.Id}, newMovie.Value.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateMovieInputModel inputModel, CancellationToken cancellationToken)
        {
            var oldMovie = await _moviesRepository.GetById(id, cancellationToken);

            if (oldMovie == null)
                return NotFound();

            // BUG ESTRANHO AQUI
            var existingSessions =
                oldMovie.Sessions
                    .Where(c => inputModel.Sessions.Any(input => input.Id == c.Id))
                    .Select(c => c.Id);

            var deletedSessions =
                oldMovie.Sessions
                    .Where(c => existingSessions.Any(id => id != c.Id))
                    .Select(c => c.Id);

            oldMovie.DeleteSessions(deletedSessions);

            foreach (var session in inputModel.Sessions)
            {
                if (session.Id == Guid.Empty)
                {
                    oldMovie.AddSession(session.Date, session.MaxOccupation, session.Price);
                }
                else
                {
                    oldMovie.UpdateSession(session.Id, session.Date, session.MaxOccupation, session.Price);
                }
            }

            _moviesRepository.Update(id, inputModel, cancellationToken);
            await _moviesRepository.Commit(cancellationToken);

            return Ok(oldMovie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var removedMovie = await _moviesRepository.GetById(id, cancellationToken);

            if (removedMovie == null)
                return NotFound();

            _moviesRepository.Delete(removedMovie);
            await _moviesRepository.Commit(cancellationToken);

            return Ok("Movie removed successfully!");
        }
    }
}
