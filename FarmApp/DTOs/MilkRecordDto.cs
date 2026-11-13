namespace FarmApp.DTOs
{
    public class MilkRecordDto
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public string AnimalName { get; set; } = string.Empty; // имя животного для удобства
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }      // литры
        public decimal FatPercentage { get; set; } // жирность
        public decimal ProteinPercentage { get; set; } // белок
    }
}