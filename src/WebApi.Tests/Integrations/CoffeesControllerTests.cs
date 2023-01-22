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
    using WebApi.ItemsParameters;
    using WebApi.Services.Interfaces;

    /// <summary>
    ///     Тестирование контроллера для взаимодействия с кофе.
    /// </summary>
    [TestFixture]
    public class CoffeesControllerTests
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

                    var coffeeServiceDescriptor = collection.SingleOrDefault(descriptor =>
                        descriptor.ServiceType == typeof(ICoffeeService));

                    collection.Remove(coffeeServiceDescriptor);

                    _coffeeServiceMock = new Mock<ICoffeeService>();

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
        ///     Фиктивный сервис для работы с кофе.
        /// </summary>
        private Mock<ICoffeeService> _coffeeServiceMock;

        /// <summary>
        ///     Клиент HTTP.
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        ///     Тестирование добавление нового кофе.
        /// </summary>
        [Test]
        public async Task CreateCoffeeAsync_ShouldCreateNewCoffee_ReturnHttpStatusCodeCreated()
        {
            // Arrange.
            var createCoffee = new CreateOrUpdateCoffeeDto
            {
                Name = "Капучино",
                Price = 500
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(createCoffee), Encoding.UTF8,
                "application/json");

            _coffeeServiceMock.Setup(service => service.CreateCoffeeAsync(It.IsAny<Coffee>())).ReturnsAsync(new Coffee
            {
                Id = Guid.NewGuid(),
                Name = createCoffee.Name,
                Price = createCoffee.Price
            });

            // Act.
            var responseMessage = await _httpClient.PostAsync("api/Coffees", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        /// <summary>
        ///     Тестирование обновления или создания кофе.
        /// </summary>
        [Test]
        public async Task UpdateOrCreateCoffeeAsync_ShouldUpdateCoffee_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var updateCoffeeDto = new CreateOrUpdateCoffeeDto
            {
                Name = "Капучино",
                Price = 700
            };
            var foundCoffee = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(updateCoffeeDto), Encoding.UTF8,
                "application/json");

            _coffeeServiceMock.Setup(service => service.GetCoffeeAsync(It.IsAny<Guid>())).ReturnsAsync(foundCoffee);
            _coffeeServiceMock.Setup(service => service.UpdateCoffeeAsync(It.IsAny<Coffee>(), It.IsAny<Coffee>()))
                .ReturnsAsync(new Coffee
                {
                    Id = foundCoffee.Id,
                    Name = updateCoffeeDto.Name,
                    Price = updateCoffeeDto.Price
                });

            // Act.
            var responseMessage = await _httpClient.PutAsync($"api/Coffees/{foundCoffee.Id}", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование удаления кофе.
        /// </summary>
        [Test]
        public async Task DeleteCoffeeAsync_ShouldExistCoffee_ReturnHttpStatusCodeNoContent()
        {
            // Arrange.
            var coffee = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };

            _coffeeServiceMock.Setup(service => service.GetCoffeeAsync(It.IsAny<Guid>())).ReturnsAsync(coffee);
            _coffeeServiceMock.Setup(service => service.DeleteCoffeeAsync(It.IsAny<Coffee>()));

            // Act.
            var responseMessage = await _httpClient.DeleteAsync($"api/Coffees/{coffee.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///     Тестирование получения кофе.
        /// </summary>
        [Test]
        public async Task GetCoffeeAsync_ShouldExistCoffee_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var coffee = new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Капучино",
                Price = 500
            };

            _coffeeServiceMock.Setup(service => service.GetCoffeeAsync(It.IsAny<Guid>())).ReturnsAsync(coffee);

            // Act.
            var responseMessage = await _httpClient.GetAsync($"api/Coffees/{coffee.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование получения всех видов кофе.
        /// </summary>
        [Test]
        public async Task GetCoffees_ShouldExistCoffees_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var coffees = new List<Coffee>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Американо",
                    Price = 700
                }
            };
            var coffeesAsyncEnumerable = coffees.ToAsyncEnumerable();
            var countCoffeesAsync = await coffeesAsyncEnumerable.CountAsync();
            var pageParametersCoffees = new ItemsParameters<Coffee>(coffeesAsyncEnumerable, countCoffeesAsync);

            _coffeeServiceMock.Setup(service => service.GetCoffeesParameters(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>())).Returns(pageParametersCoffees);

            // Act.
            var responseMessage = await _httpClient.GetAsync("api/Coffees");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}