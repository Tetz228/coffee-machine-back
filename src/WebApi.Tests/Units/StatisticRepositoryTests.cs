namespace WebApiTests.Units
{
    using Base;

    using FluentAssertions;

    using NUnit.Framework;

    using WebApi.Db.Models;

    /// <summary>
    ///     Тестирование репозитория для взаимодействия со статистикой.
    /// </summary>
    [TestFixture]
    public class StatisticRepositoryTests : BaseRepositoryTests
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
        ///     Тестирование добавления статистики.
        /// </summary>
        [Test]
        public async Task AddStatisticAsync_ShouldAddNewStatistic_ReturnAddedStatistic()
        {
            // Arrange.
            var creatingStatistic = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 500
            };

            // Act.
            await UnitOfWork.StatisticRepository.AddStatisticAsync(creatingStatistic);

            await UnitOfWork.SaveChangesAsync();

            var foundStatistic = await UnitOfWork.StatisticRepository.GetStatisticAsync(creatingStatistic.Id);

            // Assert.
            foundStatistic.Id.Should().Be(creatingStatistic.Id);
        }

        /// <summary>
        ///     Тестирование обновления статистики.
        /// </summary>
        [Test]
        public async Task UpdateStatistic_ShouldUpdateStatistic_ReturnUpdatedStatistic()
        {
            // Arrange.
            var statistic = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 500
            };

            // Act.
            await UnitOfWork.StatisticRepository.AddStatisticAsync(statistic);

            await UnitOfWork.SaveChangesAsync();

            statistic.Total = 700;

            UnitOfWork.StatisticRepository.UpdateStatistic(statistic);

            await UnitOfWork.SaveChangesAsync();

            var foundStatistic = await UnitOfWork.StatisticRepository.GetStatisticAsync(statistic.Id);

            // Assert.
            foundStatistic.Total.Should().Be(statistic.Total);
        }

        /// <summary>
        ///     Тестирование удаления статистики.
        /// </summary>
        [Test]
        public async Task DeleteStatistic_ShouldDeleteStatistic_ReturnNull()
        {
            // Arrange.
            var statistic = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 500
            };

            // Act.
            await UnitOfWork.StatisticRepository.AddStatisticAsync(statistic);

            await UnitOfWork.SaveChangesAsync();

            UnitOfWork.StatisticRepository.DeleteStatistic(statistic);

            await UnitOfWork.SaveChangesAsync();

            var foundStatistic = await UnitOfWork.StatisticRepository.GetStatisticAsync(statistic.Id);

            // Assert.
            foundStatistic.Should().BeNull();
        }

        /// <summary>
        ///     Тестирование получения статистики.
        /// </summary>
        [Test]
        public async Task GetStatisticAsync_ShouldExistStatistic_ReturnStatistic()
        {
            // Arrange.
            var creatingStatistic = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 500
            };

            // Act.

            await UnitOfWork.StatisticRepository.AddStatisticAsync(creatingStatistic);

            await UnitOfWork.SaveChangesAsync();

            var foundStatistic = await UnitOfWork.StatisticRepository.GetStatisticAsync(creatingStatistic.Id);

            // Assert.
            foundStatistic.Id.Should().Be(creatingStatistic.Id);
        }

        /// <summary>
        ///     Тестирование получения статистики по кофе.
        /// </summary>
        [Test]
        public async Task GetStatisticCoffeeAsync_ShouldExistStatisticCoffee_ReturnStatisticCoffee()
        {
            // Arrange.
            var statisticCappuccino = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 500
            };
            var statisticAmericano = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Американо",
                    Price = 500
                },
                Total = 500
            };

            // Act.
            await UnitOfWork.StatisticRepository.AddStatisticAsync(statisticAmericano);
            await UnitOfWork.StatisticRepository.AddStatisticAsync(statisticCappuccino);

            await UnitOfWork.SaveChangesAsync();

            var foundStatisticCoffee =
                await UnitOfWork.StatisticRepository.GetStatisticCoffeeAsync(statisticAmericano.Coffee.Id);

            // Assert.
            foundStatisticCoffee.Coffee.Id.Should().Be(statisticAmericano.Coffee.Id);
        }

        /// <summary>
        ///     Тестирование получения параметров статистик.
        /// </summary>
        [Test]
        public async Task GetParametersStatistics_ShouldExistStatistics_ReturnStatisticsParameters()
        {
            // Arrange.
            var statisticCappuccino = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 500
            };
            var statisticAmericano = new Statistic
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Американо",
                    Price = 500
                },
                Total = 500
            };

            // Act.
            await UnitOfWork.StatisticRepository.AddStatisticAsync(statisticAmericano);
            await UnitOfWork.StatisticRepository.AddStatisticAsync(statisticCappuccino);

            await UnitOfWork.SaveChangesAsync();

            var foundStatistics = UnitOfWork.StatisticRepository.GetStatisticsParameters("", 1, 50);
            var countStatistics = await foundStatistics.Items.CountAsync();

            // Assert.
            countStatistics.Should().Be(2);
        }
    }
}