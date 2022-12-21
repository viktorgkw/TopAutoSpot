﻿using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Models
{
    public class Trailer
    {
        [Key]
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        public double Payload { get; set; }
        public int AxleCount { get; set; }
        public string Status { get; set; }
    }
}