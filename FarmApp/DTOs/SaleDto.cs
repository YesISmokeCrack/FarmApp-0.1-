namespace FarmApp.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; // название продукта для удобства
        public string Customer { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal TotalAmount { get; set; } // вычисляемое поле
    }
}