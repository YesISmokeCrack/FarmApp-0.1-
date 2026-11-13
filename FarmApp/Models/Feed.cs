namespace FarmApp.Models
{
    public class Feed
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty; // Название корма

        public string Type { get; set; } = string.Empty; // Тип: "Hay" (сено), "Concentrate" (комбикорм) и т.д.

        public decimal Quantity { get; set; } // Текущий остаток в кг

        public string Unit { get; set; } = "kg"; // Единица измерения (кг)

        public DateTime? ExpiryDate { get; set; } // Срок годности (если есть)
    }
}