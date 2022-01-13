using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public sealed class NewMovieInputModel
    {
        [Required]
        [MinLength(10, ErrorMessage = "Movie title should have at least 10 characters")]
        public string Title { get; set; }
        [Required]
        public int Duration { get; set; }
        public string Synopsis { get; set; }
        public List<NewSessionInputModel> Sessions { get; set; }
    }
}
