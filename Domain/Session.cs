using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Domain
{
    public class Session
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public int MaxOccupation { get; private set; }
        public double Price { get; private set; }

        private Session() { }

        public Session(Guid id, DateTime date, int maxOccupation, double price)
        {
            Id = id;
            Date = date;
            MaxOccupation = maxOccupation;
            Price = price;
        }
    }
}
