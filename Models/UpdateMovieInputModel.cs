using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public class UpdateMovieInputModel
    {
        [Required]
        [MinLength(10, ErrorMessage = "Movie title should have at least 10 characters")]
        public string Title { get; set; }
        [Required]
        public int Duration { get; set; }
        public string Synopsis { get; set; }
    }
}
