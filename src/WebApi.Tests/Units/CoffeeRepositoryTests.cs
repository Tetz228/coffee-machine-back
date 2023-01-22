namespace WebApiTests.Units
{
    using Base;

    using FluentAssertions;

    using NUnit.Framework;

    using WebApi.Db.Models;

    /// <summary>
    ///     Тестирование репозитория для взаимодействия с кофе.
    /// </summary>
    [TestFixture]
    public class CoffeeRepositoryTests : BaseRepositoryTests
    {
        /// <summary>
        ///     Настройка тестирования.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        /// <summary>
        ///     Освобождение ресурсов после тестирования.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context.Dispose();
        }

        /// <summary>
        ///     Тестирование добавления кофе.
        /// </summary>
        [Test]
        public async Task AddCoffeeAsync_ShouldAddNewCoffee_ReturnAddedCoffee()
        {
            // Arrange.
            var creatingCoffee = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };

            // Act.
            await UnitOfWork.CoffeeRepository.AddCoffeeAsync(creatingCoffee);

            await UnitOfWork.SaveChangesAsync();

            var foundCoffee = await UnitOfWork.CoffeeRepository.GetCoffeeAsync(creatingCoffee.Id);

            // Assert.
            foundCoffee.Id.Should().Be(creatingCoffee.Id);
        }

        /// <summary>
        ///     Тестирование обновления кофе.
        /// </summary>
        [Test]
        public async Task UpdateCoffee_ShouldUpdateCoffee_ReturnUpdatedCoffee()
        {
            // Arrange.
            var coffee = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };

            // Act.
            await UnitOfWork.CoffeeRepository.AddCoffeeAsync(coffee);

            await UnitOfWork.SaveChangesAsync();

            coffee.Price = 700;

            UnitOfWork.CoffeeRepository.UpdateCoffee(coffee);

            await UnitOfWork.SaveChangesAsync();

            var foundCoffee = await UnitOfWork.CoffeeRepository.GetCoffeeAsync(coffee.Id);

            // Assert.
            foundCoffee.Price.Should().Be(coffee.Price);
        }

        /// <summary>
        ///     Тестирование удаления кофе.
        /// </summary>
        [Test]
        public async Task DeleteCoffee_ShouldDeleteCoffee_ReturnNull()
        {
            // Arrange.
            var coffee = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };

            // Act.
            await UnitOfWork.CoffeeRepository.AddCoffeeAsync(coffee);

            await UnitOfWork.SaveChangesAsync();

            UnitOfWork.CoffeeRepository.DeleteCoffee(coffee);

            await UnitOfWork.SaveChangesAsync();

            var foundCoffee = await UnitOfWork.CoffeeRepository.GetCoffeeAsync(coffee.Id);

            // Assert.
            foundCoffee.Should().BeNull();
        }

        /// <summary>
        ///     Тестирование получения кофе.
        /// </summary>
        [Test]
        public async Task GetCoffeeAsync_ShouldExistCoffee_ReturnCoffee()
        {
            // Arrange.
            var creatingCoffee = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };

            // Act.
            await UnitOfWork.CoffeeRepository.AddCoffeeAsync(creatingCoffee);

            await UnitOfWork.SaveChangesAsync();

            var foundCoffee = await UnitOfWork.CoffeeRepository.GetCoffeeAsync(creatingCoffee.Id);

            // Assert.
            foundCoffee.Id.Should().Be(creatingCoffee.Id);
        }

        /// <summary>
        ///     Тестирование получения параметров видов кофе.
        /// </summary>
        [Test]
        public async Task GetParametersCoffees_ShouldExistCoffees_ReturnCoffeesParameters()
        {
            // Arrange.
            var cappuccino = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };
            var americano = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Американо",
                Price = 500
            };

            // Act.
            await UnitOfWork.CoffeeRepository.AddCoffeeAsync(cappuccino);
            await UnitOfWork.CoffeeRepository.AddCoffeeAsync(americano);

            await UnitOfWork.SaveChangesAsync();

            var foundCoffees = UnitOfWork.CoffeeRepository.GetCoffeesParameters("", 1, 50);
            var countCoffees = await foundCoffees.Items.CountAsync();

            // Assert.
            countCoffees.Should().Be(2);
        }
    }
}