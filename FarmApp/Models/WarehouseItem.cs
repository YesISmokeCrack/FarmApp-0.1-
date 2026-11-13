namespace FarmApp.Models
{
    public class WarehouseItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; } // Внешний ключ на продукт
        public Product Product { get; set; } = null!;

        public decimal Quantity { get; set; } // Текущее количество на складе

        public string Location { get; set; } = "Main"; // Место хранения
    }
}