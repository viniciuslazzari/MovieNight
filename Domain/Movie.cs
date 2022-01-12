using CinemaApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;

namespace CinemaApi.Domain
{
    public sealed class Movie
    {
        [Key]
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public int Duration { get; private set; }
        public string Synopsis { get; private set;  }
        public List<Session> Sessions { get; private set; }

        private Movie() { }

        public Movie(Guid id, string title, int duration, string synopsis, List<Session> sessions)
        {
            Id = id;
            Title = title;
            Duration = duration;
            Synopsis = synopsis;
            Sessions = sessions;
        }

        public static Result<Movie> Create(NewMovieInputModel inputModel)
        {
            var newMovie = new Movie(Guid.NewGuid(), inputModel.Title, inputModel.Duration, inputModel.Synopsis, new List<Session>());

            return newMovie;
        }
    }
}
