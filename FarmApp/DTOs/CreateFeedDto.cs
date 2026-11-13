using System.ComponentModel.DataAnnotations;

namespace FarmApp.DTOs
{
    public class CreateFeedDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(10)]
        public string Unit { get; set; } = "kg";

        public DateTime? ExpiryDate { get; set; }
    }
}