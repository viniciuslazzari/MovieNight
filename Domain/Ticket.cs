using System;

namespace CinemaApi.Domain
{
    public class Ticket
    {
        public Guid Id { get; private set; }
        public string Client { get; private set; }
        public int Amount { get; private set; }

        public Ticket(Guid id, string client, int amount)
        {
            Id = id;
            Client = client;
            Amount = amount;
        }
    }
}
