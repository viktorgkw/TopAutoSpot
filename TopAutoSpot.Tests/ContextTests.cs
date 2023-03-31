namespace TopAutoSpot.Tests
{
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    public class ContextTests
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        private readonly ApplicationDbContext context;

        public ContextTests()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TopAutoSpot")
                .Options;
            context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
        }

        [Fact]
        public void TestDatabaseConnection()
        {
            Assert.True(context.Database.CanConnect());
        }

        [Fact]
        public void TestDatabaseInsert()
        {
            var newCar = GetTestCar();

            context.Cars.Add(newCar);
            context.SaveChanges();

            Assert.NotNull(newCar.Id);
        }

        [Fact]
        public void TestDatabaseSelect()
        {
            var newCar = GetTestCar();

            context.Cars.Add(newCar);
            context.SaveChanges();

            var selectedCar = context.Cars
                .FirstOrDefault(e => e.Id == newCar.Id);

            Assert.NotNull(selectedCar);
            Assert.Equal(newCar.Title, selectedCar.Title);
        }

        [Fact]
        public void TestDatabaseDelete()
        {
            var newCar = GetTestCar();

            context.Cars.Add(newCar);
            context.SaveChanges();

            var foundCar = context.Cars.First();

            context.Cars.Remove(foundCar);
            context.SaveChanges();

            Assert.Null(context.Cars.FirstOrDefault(c => c.Id == foundCar.Id));
        }

        private static Car GetTestCar()
            => new()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Title = "New Car",
                Location = "Europe",
                Price = 1,
                Make = "Test",
                Model = "Test",
                ManufactureDate = DateTime.Now,
                Transmission = "Some",
                EngineType = "Some",
                EuroStandart = "Some",
                Status = "Some",
            };
    }
}
