namespace WebApiTests.Integrations
{
    using System.Net;
    using System.Text;

    using FluentAssertions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    using MockAuthentication;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using WebApi;
    using WebApi.Db.Models;
    using WebApi.Dtos.Coffees;
    using WebApi.Dtos.Orders;
    using WebApi.Dtos.Users;
    using WebApi.Enums;
    using WebApi.Extensions.Models;
    using WebApi.Services.Interfaces;

    /// <summary>
    ///     Тестирование контроллера для взаимодействия с заказами.
    /// </summary>
    [TestFixture]
    public class OrdersControllerTests
    {
        /// <summary>
        ///     Настройка тестирования.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var appBuilder = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(collection =>
                {
                    collection.AddAuthentication(authenticationOptions =>
                    {
                        authenticationOptions.DefaultAuthenticateScheme = "Test";
                        authenticationOptions.DefaultChallengeScheme = "Test";
                    });

                    var orderServiceDescriptor = collection.SingleOrDefault(descriptor =>
                        descriptor.ServiceType == typeof(IOrderService));
                    var coffeeServiceDescriptor = collection.SingleOrDefault(descriptor =>
                        descriptor.ServiceType == typeof(ICoffeeService));

                    collection.Remove(coffeeServiceDescriptor);
                    collection.Remove(orderServiceDescriptor);

                    _coffeeServiceMock = new Mock<ICoffeeService>();
                    _orderServiceMock = new Mock<IOrderService>();

                    collection.AddScoped<IOrderService>(_ => _orderServiceMock.Object);
                    collection.AddScoped<ICoffeeService>(_ => _coffeeServiceMock.Object);
                    collection.AddTransient<IAuthenticationSchemeProvider, MockSchemeProvider>();
                });
            });

            _httpClient = appBuilder.CreateClient();
        }

        /// <summary>
        ///     Освобождение ресурсов после тестирования.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        ///     Фиктивный сервис для работы с заказом.
        /// </summary>
        private Mock<IOrderService> _orderServiceMock;

        /// <summary>
        ///     Фиктивный сервис для работы с кофе.
        /// </summary>
        private Mock<ICoffeeService> _coffeeServiceMock;

        /// <summary>
        ///     Клиент HTTP.
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        ///     Тестирование добавление нового заказа.
        /// </summary>
        [Test]
        public async Task CreateOrderAsync_ShouldCreateNewOrder_ReturnHttpStatusCodeOk()
        {
            // Arrange.
            var createOrder = new CreateOrUpdateOrderDto
            {
                Coffee = new CoffeeDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                User = new UserDto
                {
                    Id = Guid.NewGuid(),
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                }
            };
            var bills = Enum.GetValues(typeof(Bills)).Cast<Bills>().Reverse()
                .ToDictionary(bill => bill, _ => 0);
            bills[Bills.OneThousand] = 1;

            var stringContent = new StringContent(JsonConvert.SerializeObject(createOrder), Encoding.UTF8,
                "application/json");

            _orderServiceMock.Setup(service => service.MakeOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(bills);

            // Act.
            var responseMessage = await _httpClient.PostAsync("api/Orders", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование обновления или создания заказа.
        /// </summary>
        [Test]
        public async Task UpdateOrCreateOrderAsync_ShouldUpdateOrder_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var foundOrder = new Order
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
            var updateOrderDto = new CreateOrUpdateOrderDto
            {
                Coffee = new CoffeeDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Американо",
                    Price = 700
                },
                User = new UserDto
                {
                    Id = foundOrder.User.Id,
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                }
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(updateOrderDto), Encoding.UTF8,
                "application/json");

            _orderServiceMock.Setup(service => service.GetOrderAsync(It.IsAny<Guid>())).ReturnsAsync(foundOrder);
            _orderServiceMock.Setup(service => service.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<Order>()))
                .ReturnsAsync(new Order
                {
                    Id = foundOrder.Id,
                    Coffee = updateOrderDto.Coffee.ToModel(),
                    User = updateOrderDto.User.ToModel()
                });

            // Act.
            var responseMessage = await _httpClient.PutAsync($"api/Orders/{foundOrder.Id}", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование удаления заказа.
        /// </summary>
        [Test]
        public async Task DeleteOrderAsync_ShouldExistOrder_ReturnHttpStatusCodeNoContent()
        {
            // Arrange.
            var foundOrder = new Order
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

            _orderServiceMock.Setup(service => service.GetOrderAsync(It.IsAny<Guid>())).ReturnsAsync(foundOrder);
            _orderServiceMock.Setup(service => service.DeleteOrderAsync(It.IsAny<Order>()));

            // Act.
            var responseMessage = await _httpClient.DeleteAsync($"api/Orders/{foundOrder.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///     Тестирование получения заказа.
        /// </summary>
        [Test]
        public async Task GetOrderAsync_ShouldExistOrder_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var foundOrder = new Order
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

            _orderServiceMock.Setup(service => service.GetOrderAsync(It.IsAny<Guid>())).ReturnsAsync(foundOrder);

            // Act.
            var responseMessage = await _httpClient.GetAsync($"api/Orders/{foundOrder.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование получения всех заказов.
        /// </summary>
        [Test]
        public async Task GetOrders_ShouldExistOrders_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var orders = new List<Order>
            {
                new()
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
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Coffee = new Coffee
                    {
                        Id = Guid.NewGuid(),
                        Name = "Американо",
                        Price = 700
                    },
                    User = new User
                    {
                        Id = Guid.NewGuid(),
                        Login = "Kobok",
                        Password = "123456789",
                        Name = "Дмитрий",
                        Balance = 2500
                    }
                }
            };
            var ordersAsync = GetIAsyncEnumerable(orders);

            _orderServiceMock.Setup(service => service.GetAllOrders()).Returns(ordersAsync);

            // Act.
            var responseMessage = await _httpClient.GetAsync("api/Orders");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Получение асинхронного перечисление заказов.
        /// </summary>
        /// <param name="orders">Список заказов.</param>
        /// <returns>Асинхронное перечисление заказов.</returns>
        private static async IAsyncEnumerable<Order> GetIAsyncEnumerable(List<Order> orders)
        {
            foreach (var order in orders)
            {
                yield return order;
            }
        }
    }
}