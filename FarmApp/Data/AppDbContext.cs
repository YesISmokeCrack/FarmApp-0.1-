using Microsoft.EntityFrameworkCore;
using FarmApp.Models;

namespace FarmApp.Data
{
    public class AppDbContext : DbContext
    {
        // Конструктор, который принимает параметры конфигурации (например, какую базу использовать)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet для каждой модели — это аналог таблицы в базе данных
        public DbSet<Animal> Animals { get; set; }
        public DbSet<MilkRecord> MilkRecords { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedConsumption> FeedConsumptions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<Sale> Sales { get; set; }

        // Этот метод вызывается при создании модели базы данных.
        // Здесь мы можем настроить связи между таблицами более детально.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настраиваем связь между MilkRecord и Animal
            modelBuilder.Entity<MilkRecord>()
                .HasOne(m => m.Animal)               // у одной записи надоя есть одно животное
                .WithMany(a => a.MilkRecords)        // у одного животного может быть много записей надоя
                .HasForeignKey(m => m.AnimalId)      // внешний ключ — AnimalId
                .OnDelete(DeleteBehavior.Cascade);   // если удалить животное, удалятся и все его надои

            // Связь между FeedConsumption и Feed
            modelBuilder.Entity<FeedConsumption>()
                .HasOne(fc => fc.Feed)
                .WithMany()                           // у Feed нет коллекции FeedConsumptions (мы её не добавляли)
                .HasForeignKey(fc => fc.FeedId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между WarehouseItem и Product
            modelBuilder.Entity<WarehouseItem>()
                .HasOne(w => w.Product)
                .WithMany()                           // у Product может быть много записей на складе
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Sale и Product
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany()                           // у Product может быть много продаж
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // запрещаем удалять продукт, если есть продажи
        }
    }
}