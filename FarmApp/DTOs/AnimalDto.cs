namespace FarmApp.DTOs
{
    public class AnimalDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string HealthStatus { get; set; } = string.Empty;
        public DateTime? LastVaccinationDate { get; set; }
    }
}