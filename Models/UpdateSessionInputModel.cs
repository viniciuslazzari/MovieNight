using System.Collections.Generic;

namespace CinemaApi.Models
{
    public sealed class UpdateSessionInputModel
    {
        public string MovieId { get; set; }
        public string Date { get; set; }
        public int MaxOccupation { get; set; }
        public double Price { get; set; }
    }
}
