using Microsoft.EntityFrameworkCore;

using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Tests
{
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
            // Assert
            Assert.True(context.Database.CanConnect());
        }

        [Fact]
        public void TestDatabaseSelect()
        {
            // Arrange
            Car newCar = GetTestCar();

            // Act
            context.Cars.Add(newCar);
            context.SaveChanges();

            Car? selectedCar = context.Cars
                .FirstOrDefault(e => e.Id == newCar.Id);

            // Assert
            Assert.NotNull(selectedCar);
            Assert.Equal(newCar.Title, selectedCar.Title);
        }

        [Fact]
        public void TestDatabaseInsert()
        {
            // Arrange
            Car newCar = GetTestCar();

            // Act
            context.Cars.Add(newCar);
            context.SaveChanges();

            // Assert
            Assert.NotNull(context.Cars.First());
        }

        [Fact]
        public void TestDatabaseDelete()
        {
            // Arrange
            Car newCar = GetTestCar();

            // Act
            context.Cars.Add(newCar);
            context.SaveChanges();

            Car foundCar = context.Cars.First();

            context.Cars.Remove(foundCar);
            context.SaveChanges();

            // Assert
            Assert.Null(context.Cars.FirstOrDefault(c => c.Id == foundCar.Id));
        }

        [Fact]
        public void TestDatabaseEdit()
        {
            // Arrange
            Car newCar = GetTestCar();
            string startName = newCar.Title;
            string newName = "Edited New Car";

            // Act
            context.Cars.Add(newCar);
            context.SaveChanges();

            newCar.Title = newName;
            context.SaveChanges();

            // Assert
            Assert.False(context.Cars.Any(c => c.Title == startName));
            Assert.True(context.Cars.Any(c => c.Title == newName));
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
