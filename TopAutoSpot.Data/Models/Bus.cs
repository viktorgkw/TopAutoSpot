﻿using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Models
{
    public class Bus
    {
        [Key]
        public string Id { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string Location { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public double Price { get; set; }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public DateTime ManufactureDate { get; set; }

        public int? HorsePower { get; set; }

        public double? Mileage { get; set; }

        public string Transmission { get; set; } = null!;

        public string EngineType { get; set; } = null!;

        public string EuroStandart { get; set; } = null!;

        public int AxlesCount { get; set; }

        public int SeatsCount { get; set; }

        public double LoadCapacity { get; set; }

        public string Status { get; set; } = null!;
    }
}