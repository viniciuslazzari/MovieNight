
using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public class NewSessionInputModel
    {
        [Required]
        public DateTime Date { get; set; }
        public int MaxOccupation { get; set; }
        public double Price { get; set; }
    }
}
