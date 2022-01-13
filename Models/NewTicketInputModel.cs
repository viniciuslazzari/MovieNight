using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public sealed class NewTicketInputModel
    {
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string Client { get; set; }
        public int Amount { get; set; }
    }
}
