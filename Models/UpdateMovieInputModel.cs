using System;
using System.Collections.Generic;
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
        public List<UpdateSessionInputModel> Sessions { get; set; }

        public sealed class UpdateSessionInputModel
        {
            public Guid Id { get; set; }
            [Required]
            public DateTime Date { get; set; }
            public int MaxOccupation { get; set; }
            public double Price { get; set; }
        }
    }
}
