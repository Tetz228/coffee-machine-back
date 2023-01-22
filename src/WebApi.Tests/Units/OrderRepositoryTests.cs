namespace WebApiTests.Units
{
    using Base;

    using FluentAssertions;

    using NUnit.Framework;

    using WebApi.Db.Models;

    /// <summary>
    ///     Тестирование репозитория для взаимодействия с заказом.
    /// </summary>
    [TestFixture]
    public class OrderRepositoryTests : BaseRepositoryTests
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
        ///     Тестирование добавления заказа.
        /// </summary>
        [Test]
        public async Task AddOrderAsync_ShouldAddNewOrder_ReturnAddedOrder()
        {
            // Arrange.
            var creatingOrder = new Order
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                }
            };

            // Act.
            await UnitOfWork.OrderRepository.AddOrderAsync(creatingOrder);

            await UnitOfWork.SaveChangesAsync();

            var foundOrder = await UnitOfWork.OrderRepository.GetOrderAsync(creatingOrder.Id);

            // Assert.
            foundOrder.Id.Should().Be(foundOrder.Id);
        }

        /// <summary>
        ///     Тестирование обновления заказа.
        /// </summary>
        [Test]
        public async Task UpdateOrder_ShouldUpdateOrder_ReturnUpdatedOrder()
        {
            // Arrange.
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                }
            };

            // Act.
            await UnitOfWork.OrderRepository.AddOrderAsync(order);

            await UnitOfWork.SaveChangesAsync();

            order.Coffee.Price = 700;

            UnitOfWork.OrderRepository.UpdateOrder(order);

            await UnitOfWork.SaveChangesAsync();

            var foundOrder = await UnitOfWork.OrderRepository.GetOrderAsync(order.Id);

            // Assert.
            foundOrder.Coffee.Name.Should().Be(order.Coffee.Name);
        }

        /// <summary>
        ///     Тестирование удаления заказа.
        /// </summary>
        [Test]
        public async Task DeleteOrder_ShouldDeleteOrder_ReturnNull()
        {
            // Arrange.
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                }
            };

            // Act.
            await UnitOfWork.OrderRepository.AddOrderAsync(order);

            await UnitOfWork.SaveChangesAsync();

            UnitOfWork.OrderRepository.DeleteOrder(order);

            await UnitOfWork.SaveChangesAsync();

            var foundOrder = await UnitOfWork.OrderRepository.GetOrderAsync(order.Id);

            // Assert.
            foundOrder.Should().BeNull();
        }

        /// <summary>
        ///     Тестирование получения заказа.
        /// </summary>
        [Test]
        public async Task GetOrderAsync_ShouldExistOrder_ReturnOrder()
        {
            // Arrange.
            var creatingOrder = new Order
            {
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                }
            };

            // Act.
            await UnitOfWork.OrderRepository.AddOrderAsync(creatingOrder);

            await UnitOfWork.SaveChangesAsync();

            var foundOrder = await UnitOfWork.OrderRepository.GetOrderAsync(creatingOrder.Id);

            // Assert.
            foundOrder.Id.Should().Be(creatingOrder.Id);
        }

        /// <summary>
        ///     Тестирование получения всех заказов.
        /// </summary>
        [Test]
        public async Task GetOrders_ShouldExistOrders_ReturnOrders()
        {
            // Arrange.
            var cappuccino = new Order
            {
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                }
            };
            var americano = new Order
            {
                Coffee = new Coffee
                {
                    Id = Guid.NewGuid(),
                    Name = "Американо",
                    Price = 500
                },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Ghosht",
                    Password = "123456789",
                    Name = "Вадим",
                    Balance = 2500
                }
            };

            await UnitOfWork.OrderRepository.AddOrderAsync(americano);
            await UnitOfWork.OrderRepository.AddOrderAsync(cappuccino);

            await UnitOfWork.SaveChangesAsync();

            // Act.
            var foundOrders = UnitOfWork.OrderRepository.GetAllOrders();

            // Assert.
            foundOrders.Should().NotBeNull();
        }
    }
}