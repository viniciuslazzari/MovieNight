using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Domain
{
    public class Session
    {
        private IList<Ticket> _tickets;

        [Required]
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public int MaxOccupation { get; private set; }
        public double Price { get; private set; }
        public IEnumerable<Ticket> Tickets => _tickets;

        public Session(Guid id, DateTime date, int maxOccupation, double price, List<Ticket> tickets)
        {
            Id = id;
            Date = date;
            MaxOccupation = maxOccupation;
            Price = price;
            _tickets = tickets;
        }
    }
}
