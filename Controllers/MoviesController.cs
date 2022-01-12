using CinemaApi.Domain;
using CinemaApi.Infrastructure;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
            {
                return BadRequest(newMovie.Error);
            }

            await _moviesRepository.Create(newMovie.Value, cancellationToken);
            await _moviesRepository.Commit(cancellationToken);

            return CreatedAtAction("GetById", new { id = newMovie.Value.Id}, newMovie.Value.Id);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
