using System.ComponentModel.DataAnnotations;

namespace FarmApp.DTOs
{
    public class CreateAnimalDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        public string Breed { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string HealthStatus { get; set; } = "Healthy";

        public DateTime? LastVaccinationDate { get; set; }
    }
}