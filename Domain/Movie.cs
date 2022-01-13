using CinemaApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using System.Linq;

namespace CinemaApi.Domain
{
    public sealed class Movie
    {
        private IList<Session> _sessions;

        [Key]
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public int Duration { get; private set; }
        public string Synopsis { get; private set;  }
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

        public static Result<Movie> Create(Guid id, UpdateMovieInputModel inputModel)
        {
            var newMovie = new Movie(id, inputModel.Title, inputModel.Duration, inputModel.Synopsis, new List<Session>());

            return newMovie;
        }

        public void AddSession(DateTime date, int maxOccupation, double price)
        {
            // Lembrar de tirar os nulls
            var newSession = new Session(Guid.NewGuid(), Id, date, maxOccupation, price, null);
            _sessions.Add(newSession);
        }

        public void UpdateSession(Guid id, DateTime date, int maxOccupation, double price)
        {
            var session = _sessions.FirstOrDefault(item => item.Id == id);
            if (session != null)
                // Lembrar de tirar os nulls
                session = new Session(id, Id, date, maxOccupation, price, null);
        }

        public void DeleteSessions(IEnumerable<Guid> deletedSessions)
        {
            var sessions = _sessions.Where(c => deletedSessions.Any(id => id == c.Id));
            foreach (var session in sessions)
                _sessions.Remove(session);
        }
    }
}
