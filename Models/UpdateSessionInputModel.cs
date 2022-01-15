using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public sealed class UpdateSessionInputModel
    {
        [Required]
        public string MovieId { get; set; }
        [Required]
        public string Date { get; set; }
        public int MaxOccupation { get; set; }
        public double Price { get; set; }
    }
}
