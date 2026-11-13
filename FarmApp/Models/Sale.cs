namespace FarmApp.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public int ProductId { get; set; } // Внешний ключ на продукт
        public Product Product { get; set; } = null!;

        public string Customer { get; set; } = string.Empty; // Покупатель

        public decimal Quantity { get; set; } // Проданное количество

        public DateTime SaleDate { get; set; } = DateTime.UtcNow; // Дата продажи (по умолчанию текущая)

        public decimal PricePerUnit { get; set; } // Цена за единицу

        // Это вычисляемое поле — не хранится в БД, но удобно для использования в коде
        public decimal TotalAmount => Quantity * PricePerUnit;
    }
}