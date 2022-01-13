
using CinemaApi.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public class NewSessionInputModel
    {
        [Required]
        public string MovieId { get; set; }
        [Required]
        public string Date { get; set; }
        public int MaxOccupation { get; set; }
        public double Price { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
