using CinemaApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;

namespace CinemaApi.Domain
{
    public sealed class Movie
    {
        private IList<Session> _sessions;

        [Key]
        public Guid Id { get; private set; }
        [Required]
        [MinLength(5, ErrorMessage = "Movie title should have at least 5 characters")]
        public string Title { get; private set; }
        [Required]
        public int Duration { get; private set; }
        public string Synopsis { get; private set; }
        public IEnumerable<Session> Sessions => _sessions;

        private Movie() { }

        public Movie(Guid id, string title, int duration, string synopsis, List<Session> sessions)
        {
            Id = id;
            Title = title;
            Duration = duration;
            Synopsis = synopsis;
            _sessions = sessions;
        }

        public static Result<Movie> Create(NewMovieInputModel inputModel)
        {
            var newMovie = new Movie(Guid.NewGuid(), inputModel.Title, inputModel.Duration, inputModel.Synopsis, new List<Session>());

            return newMovie;
        }

        public void Update(UpdateMovieInputModel inputModel)
        {
            Title = inputModel.Title;
            Duration = inputModel.Duration;
            Synopsis = inputModel.Synopsis;
        }
    }
}
