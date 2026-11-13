namespace FarmApp.Models
{
    public class FeedConsumption
    {
        public int Id { get; set; }

        public int FeedId { get; set; } // Внешний ключ на корм
        public Feed Feed { get; set; } = null!; // Навигация

        public DateTime Date { get; set; } // Дата расхода

        public decimal Quantity { get; set; } // Сколько скормлено

        public string? Notes { get; set; } // Примечания (необязательно)
    }
}