using System.ComponentModel.DataAnnotations;

namespace FarmApp.DTOs
{
    public class CreateSaleDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Customer { get; set; } = string.Empty;

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
        public decimal Quantity { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.UtcNow; // если не указано, будет текущая дата

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal PricePerUnit { get; set; }
    }
}