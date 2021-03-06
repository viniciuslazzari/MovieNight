using CinemaApi.Models;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Domain
{
    public class Session : BaseModel
    {
        private IList<Ticket> _tickets;

        [Key]
        public Guid Id { get; private set; }
        [Required]
        public Guid MovieId { get; private set; }
        [Required]
        public DateTime Date { get; private set; }
        public int MaxOccupation { get; private set; }
        public double Price { get; private set; }
        public IEnumerable<Ticket> Tickets => _tickets;

        private Session() { }

        public Session(Guid id, Guid movieId, DateTime date, int maxOccupation, double price, List<Ticket> tickets)
        {
            Id = id;
            MovieId = movieId;
            Date = date;
            MaxOccupation = maxOccupation;
            Price = price;
            _tickets = tickets;
        }

        public static Result<Session> Create(NewSessionInputModel inputModel)
        {
            if (!Guid.TryParse(inputModel.MovieId, out var movieGuid))
                return Result.Failure<Session>("Movie ID could not be converted");

            if (!DateTime.TryParse(inputModel.Date, out var date))
                return Result.Failure<Session>("Date could not be converted");

            var newSession = 
                new Session(
                    Guid.NewGuid(), movieGuid, date, inputModel.MaxOccupation, inputModel.Price, new List<Ticket>());

            return newSession;
        }

        public void Update(UpdateSessionInputModel inputModel)
        {
            MovieId = Guid.Parse(inputModel.MovieId);
            Date = DateTime.Parse(inputModel.Date);
            MaxOccupation = inputModel.MaxOccupation;
            Price = inputModel.Price;
        }
    }
}
