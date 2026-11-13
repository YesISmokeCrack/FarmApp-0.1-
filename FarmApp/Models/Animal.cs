using System.ComponentModel.DataAnnotations;

namespace FarmApp.Models
{
    public class Animal
    {
        public int Id { get; set; }

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

        // Навигационное свойство для связи с записями надоев
        public ICollection<MilkRecord> MilkRecords { get; set; } = new List<MilkRecord>();
    }
}