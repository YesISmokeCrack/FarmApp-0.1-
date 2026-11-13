namespace FarmApp.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;       // Название продукта
        public string Type { get; set; } = string.Empty;       // Тип: "Milk", "Meat", "Eggs"
        public decimal Quantity { get; set; }                  // Общее произведенное количество
        public string Unit { get; set; } = string.Empty;       // Единица измерения: "l", "kg", "pcs"
        public DateTime ProductionDate { get; set; }           // Дата производства
        public DateTime ExpiryDate { get; set; }               // Срок годности
        public int? AnimalId { get; set; }                     // ID животного (если продукт от конкретного животного)
        public string? AnimalName { get; set; }                 // Имя животного (для удобства)
    }
}