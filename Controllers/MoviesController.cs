using CinemaApi.Domain;
using CinemaApi.Infrastructure;
using CinemaApi.Models;
using CinemaApi.Models.Responses;
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

            return Ok(new SuccessJsonResponse(movies));
        }

        [HttpGet("{id}")]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest(new ErrorJsonResponse("ID could not be converted"));

            var movie = await _moviesRepository.GetById(guid, cancellationToken);

            return Ok(new SuccessJsonResponse(movie));
        }

        [HttpPost]
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Post([FromBody] NewMovieInputModel inputModel, CancellationToken cancellationToken)
        {
            var movie = Movie.Create(inputModel);
            if (movie.IsFailure)
            {
                _logger.LogInformation($"Error: {movie.Error}");
                return BadRequest(new ErrorJsonResponse(movie.Error));
            }

            await _moviesRepository.Create(movie.Value, cancellationToken);
            await _moviesRepository.Commit(cancellationToken);

            _logger.LogInformation($"Movie {movie.Value.Id} created successfully");

            return Ok(new SuccessJsonResponse("Movie created successfully!", movie.Value));
        }

        [HttpPut("{id}")]
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateMovieInputModel inputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest(new ErrorJsonResponse("ID could not be converted"));

            var movie = await _moviesRepository.GetById(guid, cancellationToken);

            if (movie == null)
                return NotFound(new ErrorJsonResponse("Movie not found"));

            movie.Update(inputModel);
            _moviesRepository.Update(movie);
            await _moviesRepository.Commit(cancellationToken);

            _logger.LogInformation($"Movie {movie.Id} updated successfully");

            return Ok(new SuccessJsonResponse("Movie updated successfully!", movie));
        }

        [HttpDelete("{id}")]
        [Authorize]
        //[RequireHttpsOrClose]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest(new ErrorJsonResponse("ID could not be converted"));

            var removedMovie = await _moviesRepository.GetById(guid, cancellationToken);

            if (removedMovie == null)
                return NotFound(new ErrorJsonResponse("Movie not found"));

            _moviesRepository.Delete(removedMovie);
            await _moviesRepository.Commit(cancellationToken);

            _logger.LogInformation($"Movie {removedMovie.Id} deleted successfully");

            return Ok(new SuccessJsonResponse("Movie deleted successfully!"));
        }
    }
}
