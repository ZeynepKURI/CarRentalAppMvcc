using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAppMvc.Models
{
	public class Vehicle
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(7)]
        public string LicensePlate { get; set; }
    }
}

