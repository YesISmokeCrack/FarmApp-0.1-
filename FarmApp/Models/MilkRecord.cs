namespace FarmApp.Models
{
    public class MilkRecord
    {
        public int Id { get; set; } // Уникальный номер записи

        public int AnimalId { get; set; } // Внешний ключ — указывает, какому животному принадлежит удой

        // Навигационное свойство: связанный объект Animal
        public Animal Animal { get; set; } = null!; // null! означает, что мы гарантируем, что это не null (будет загружено из БД)

        public DateTime Date { get; set; } // Дата надоя

        public decimal Quantity { get; set; } // Количество молока в литрах

        public decimal FatPercentage { get; set; } // Жирность в процентах

        public decimal ProteinPercentage { get; set; } // Белок в процентах
    }
}