using System.ComponentModel.DataAnnotations;

namespace FarmApp.DTOs
{
    public class FeedConsumptionDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal Quantity { get; set; }

        public string? Notes { get; set; } // примечания (необязательно)
    }
}