using CinemaApi.Domain;
using CinemaApi.Infrastructure;
using CinemaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var movies = await _moviesRepository.GetAll(cancellationToken);

            return Ok(movies);
        }

        [HttpGet("{id}")]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var movie = await _moviesRepository.GetById(guid, cancellationToken);

            return Ok(movie);
        }

        [HttpPost]
        [Authorize]
        //[RequireHttpsOrClose]
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
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateMovieInputModel inputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("ID could not be converted");

            var movie = await _moviesRepository.GetById(guid, cancellationToken);

            if (movie == null)
                return NotFound();

            movie.Update(inputModel);
            _moviesRepository.Update(movie);
            await _moviesRepository.Commit(cancellationToken);

            _logger.LogInformation($"Movie {movie.Id} updated successfully");

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        [Authorize]
        //[RequireHttpsOrClose]
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
