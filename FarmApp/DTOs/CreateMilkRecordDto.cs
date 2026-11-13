using System.ComponentModel.DataAnnotations;

namespace FarmApp.DTOs
{
    public class CreateMilkRecordDto
    {
        [Required]
        public int AnimalId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.1, 100)] // количество должно быть положительным и разумным
        public decimal Quantity { get; set; }

        [Range(0, 10)] // жирность обычно 0-10%
        public decimal FatPercentage { get; set; }

        [Range(0, 10)]
        public decimal ProteinPercentage { get; set; }
    }
}