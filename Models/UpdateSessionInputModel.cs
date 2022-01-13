using System.Collections.Generic;

namespace CinemaApi.Models
{
    public sealed class UpdateSessionInputModel
    {
        public string MovieId { get; set; }
        public string Date { get; set; }
        public int MaxOccupation { get; set; }
        public double Price { get; set; }
        public List<UpdateTicketInputModel> Tickets { get; set; }

        public sealed class UpdateTicketInputModel
        {
            public string Id { get; set; }
            public string Client { get; set; }
            public int Amount { get; set; }
        }
    }
}
