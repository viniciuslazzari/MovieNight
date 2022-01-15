using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public sealed class NewMovieInputModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "Movie title should have at least 5 characters")]
        public string Title { get; set; }
        [Required]
        public int Duration { get; set; }
        public string Synopsis { get; set; }
    }
}
