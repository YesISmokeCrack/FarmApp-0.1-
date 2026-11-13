namespace FarmApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty; // Например, "Молоко пастеризованное"

        public string Type { get; set; } = string.Empty; // "Milk", "Meat", "Eggs"

        public decimal Quantity { get; set; } // Количество (например, 100 литров)

        public string Unit { get; set; } = "l"; // Единица измерения: "l" (литры), "kg", "pcs"

        public DateTime ProductionDate { get; set; } // Дата производства

        public DateTime ExpiryDate { get; set; } // Срок годности

        public int? AnimalId { get; set; } // Если продукт от конкретного животного (например, мясо)
        public Animal? Animal { get; set; } // Навигация
    }
}