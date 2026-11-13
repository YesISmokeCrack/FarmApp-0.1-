namespace FarmApp.DTOs
{
    public class FeedConsumptionRecordDto
    {
        public int Id { get; set; }
        public int FeedId { get; set; }
        public string FeedName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }
    }
}