using FarmApp.Models;

namespace FarmApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Проверяем, есть ли уже животные в базе
            if (context.Animals.Any())
            {
                return; // База уже заполнена
            }

            // Добавляем животных
            var animals = new Animal[]
            {
                new Animal { Name = "Бурёнка", Type = "Cow", Breed = "Голштинская", BirthDate = new DateTime(2020, 5, 10), Gender = "Female", HealthStatus = "Healthy", LastVaccinationDate = new DateTime(2025, 1, 15) },
                new Animal { Name = "Зорька", Type = "Cow", Breed = "Айрширская", BirthDate = new DateTime(2021, 3, 12), Gender = "Female", HealthStatus = "Healthy", LastVaccinationDate = new DateTime(2025, 2, 10) },
                new Animal { Name = "Милка", Type = "Cow", Breed = "Джерсейская", BirthDate = new DateTime(2019, 11, 5), Gender = "Female", HealthStatus = "Recovering", LastVaccinationDate = new DateTime(2024, 12, 20) }
            };
            context.Animals.AddRange(animals);
            context.SaveChanges();

            // Добавляем надои молока
            var milkRecords = new MilkRecord[]
            {
                new MilkRecord { AnimalId = 1, Date = new DateTime(2025, 2, 1), Quantity = 25.5m, FatPercentage = 3.8m, ProteinPercentage = 3.2m },
                new MilkRecord { AnimalId = 1, Date = new DateTime(2025, 2, 2), Quantity = 26.0m, FatPercentage = 3.7m, ProteinPercentage = 3.1m },
                new MilkRecord { AnimalId = 2, Date = new DateTime(2025, 2, 1), Quantity = 22.0m, FatPercentage = 4.0m, ProteinPercentage = 3.4m },
            };
            context.MilkRecords.AddRange(milkRecords);
            context.SaveChanges();

            // Добавляем корма
            var feeds = new Feed[]
            {
                new Feed { Name = "Сено луговое", Type = "Hay", Quantity = 5000, Unit = "kg", ExpiryDate = new DateTime(2025, 12, 31) },
                new Feed { Name = "Комбикорм КК-60", Type = "Concentrate", Quantity = 2000, Unit = "kg", ExpiryDate = new DateTime(2025, 6, 1) }
            };
            context.Feeds.AddRange(feeds);
            context.SaveChanges();

            // Добавляем расход кормов
            var consumptions = new FeedConsumption[]
            {
                new FeedConsumption { FeedId = 1, Date = new DateTime(2025, 2, 1), Quantity = 200, Notes = "Утренняя дача" },
                new FeedConsumption { FeedId = 2, Date = new DateTime(2025, 2, 1), Quantity = 150, Notes = "Вечерняя дача" }
            };
            context.FeedConsumptions.AddRange(consumptions);
            context.SaveChanges();

            // Добавляем продукцию
            var products = new Product[]
            {
                new Product { Name = "Молоко пастеризованное 3.2%", Type = "Milk", Quantity = 100, Unit = "l", ProductionDate = new DateTime(2025, 2, 10), ExpiryDate = new DateTime(2025, 2, 17), AnimalId = null },
                new Product { Name = "Мясо говядина охлаждённая", Type = "Meat", Quantity = 50, Unit = "kg", ProductionDate = new DateTime(2025, 2, 5), ExpiryDate = new DateTime(2025, 3, 5), AnimalId = 1 }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            // Добавляем складские остатки
            var warehouseItems = new WarehouseItem[]
            {
                new WarehouseItem { ProductId = 1, Quantity = 100, Location = "Холодильник №1" },
                new WarehouseItem { ProductId = 2, Quantity = 50, Location = "Морозильная камера" }
            };
            context.WarehouseItems.AddRange(warehouseItems);
            context.SaveChanges();

            // Добавляем продажи
            var sales = new Sale[]
            {
                new Sale { ProductId = 1, Customer = "ИП Иванов", Quantity = 20, PricePerUnit = 60, SaleDate = new DateTime(2025, 2, 11) }
            };
            context.Sales.AddRange(sales);
            context.SaveChanges();
        }
    }
}