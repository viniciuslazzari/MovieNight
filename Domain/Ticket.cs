using CinemaApi.Models;
using CSharpFunctionalExtensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Domain
{
    public class Ticket : BaseModel
    {
        [Key]
        public Guid Id { get; private set; }
        [Required]
        public Guid SessionId { get; private set; }
        [Required]
        public string Client { get; private set; }
        [Required]
        public int Amount { get; private set; }

        public Ticket(Guid id, Guid sessionId, string client, int amount)
        {
            Id = id;
            SessionId = sessionId;
            Client = client;
            Amount = amount;
        }

        public static Result<Ticket> Create(NewTicketInputModel inputModel)
        {
            var newTicket = new Ticket(Guid.NewGuid(), Guid.Parse(inputModel.SessionId), inputModel.Client, inputModel.Amount);

            return newTicket;
        }
    }
}
