using CinemaApi.Models;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CinemaApi.Domain
{
    public class Session
    {
        private IList<Ticket> _tickets;

        [Required]
        public Guid Id { get; private set; }
        [Required]
        public Guid MovieId { get; private set; }
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
            var newSession = 
                new Session(
                    Guid.NewGuid(), Guid.Parse(inputModel.MovieId), DateTime.Parse(inputModel.Date), 
                    inputModel.MaxOccupation, inputModel.Price, inputModel.Tickets);

            return newSession;
        }

        public void AddTicket(string client, int amount)
        {
            var newTicket = new Ticket(Guid.NewGuid(), Id, client, amount);
            _tickets.Add(newTicket);
        }

        public void UpdateTicket(Guid id, string client, int amount)
        {
            var ticket = _tickets.FirstOrDefault(item => item.Id == id);
            if (ticket != null)
                ticket = new Ticket(id, Id, client, amount);
        }

        public void DeleteTickets(IEnumerable<Guid> deletedTickets)
        {
            var tickets = _tickets.Where(c => deletedTickets.Any(id => id == c.Id));
            foreach (var ticket in tickets)
                _tickets.Remove(ticket);
        }
    }
}
