using System;
using System.Collections.Generic;

namespace CinemaApi.Domain
{
    public class Session
    {
        private IList<Ticket> _tickets;

        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public int MaxOccupation { get; private set; }
        public double Price { get; private set; }
        public IEnumerable<Ticket> Tickets => _tickets;

        public Session(Guid id, DateTime date, int maxOccupation, double price)
        {
            Id = id;
            Date = date;
            MaxOccupation = maxOccupation;
            Price = price;
        }
    }
}
