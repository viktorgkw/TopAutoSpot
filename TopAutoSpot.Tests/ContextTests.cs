namespace TopAutoSpot.Tests
{
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// This class contains Unit Tests to verify that the database context is functioning correctly.
    /// </summary>
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

        /// <summary>
        /// This test method verifies whether the context's database can be connected to or not.
        /// It uses the Assert.True method to ensure that the connection is successful.
        /// </summary>
        [Fact]
        public void TestDatabaseConnection()
        {
            // Assert
            Assert.True(context.Database.CanConnect());
        }

        /// <summary>
        /// This test method verifies that the database can add, save and retrieve entities successfully.
        /// </summary>
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

        /// <summary>
        /// This test method verifies that the database can insert and save entities successfully.
        /// </summary>
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

        /// <summary>
        /// This test method verifies that the database can delete entities successfully.
        /// </summary>
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

        /// <summary>
        /// This test method verifies that the database can edit entities successfully.
        /// </summary>
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

        /// <summary>
        /// This a static class that returns instance of a Car that is used for testing.
        /// </summary>
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
