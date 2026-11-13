namespace FarmApp.DTOs
{
    public class FeedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
    }
}