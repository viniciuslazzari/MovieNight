using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public class UpdateSessionInputModel
    {
        [Required]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int MaxOccupation { get; set; }
        public double Price { get; set; }
        public List<UpdateTicketInputModel> Tickets { get; set; }

        public sealed class UpdateTicketInputModel
        {
            public Guid Id { get; set; }
            public string Client { get; set; }
            public int Amount { get; set; }
        }
    }
}
