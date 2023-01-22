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
    using WebApi.Dtos.Statistics;
    using WebApi.Extensions.Models;
    using WebApi.ItemsParameters;
    using WebApi.Services.Interfaces;

    /// <summary>
    ///     Тестирование контроллера для взаимодействия со статистикой.
    /// </summary>
    [TestFixture]
    public class StatisticsControllerTests
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

                    var statisticServiceDescriptor = collection.SingleOrDefault(descriptor =>
                        descriptor.ServiceType == typeof(IStatisticService));

                    collection.Remove(statisticServiceDescriptor);

                    _statisticServiceMock = new Mock<IStatisticService>();

                    collection.AddScoped<IStatisticService>(_ => _statisticServiceMock.Object);
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
        ///     Фиктивный сервис для работы со статистикой.
        /// </summary>
        private Mock<IStatisticService> _statisticServiceMock;

        /// <summary>
        ///     Клиент HTTP.
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        ///     Тестирование добавления новой статистики.
        /// </summary>
        [Test]
        public async Task CreateStatisticAsync_ShouldCreateNewStatistic_ReturnHttpStatusCodeCreated()
        {
            // Arrange.
            var createStatistic = new CreateOrUpdateStatisticDto
            {
                Coffee = new CoffeeDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 500
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(createStatistic), Encoding.UTF8,
                "application/json");

            _statisticServiceMock.Setup(service => service.CreateStatisticAsync(It.IsAny<Statistic>())).ReturnsAsync(
                new Statistic
                {
                    Id = Guid.NewGuid(),
                    Coffee = createStatistic.Coffee.ToModel(),
                    Total = createStatistic.Total
                });

            // Act.
            var responseMessage = await _httpClient.PostAsync("api/Statistics", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        /// <summary>
        ///     Тестирование обновления или создания статистики.
        /// </summary>
        [Test]
        public async Task UpdateOrCreateStatisticAsync_ShouldUpdateStatistic_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var updateStatisticDto = new CreateOrUpdateStatisticDto
            {
                Coffee = new CoffeeDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Капучино",
                    Price = 500
                },
                Total = 1000
            };
            var foundStatistic = new Statistic
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
            var stringContent = new StringContent(JsonConvert.SerializeObject(updateStatisticDto), Encoding.UTF8,
                "application/json");

            _statisticServiceMock.Setup(service => service.GetStatisticAsync(It.IsAny<Guid>()))
                .ReturnsAsync(foundStatistic);
            _statisticServiceMock.Setup(service =>
                    service.UpdateStatisticAsync(It.IsAny<Statistic>(), It.IsAny<Statistic>()))
                .ReturnsAsync(new Statistic
                {
                    Id = foundStatistic.Id,
                    Coffee = updateStatisticDto.Coffee.ToModel(),
                    Total = updateStatisticDto.Total
                });

            // Act.
            var responseMessage = await _httpClient.PutAsync($"api/Statistics/{foundStatistic.Id}", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование удаления статистики.
        /// </summary>
        [Test]
        public async Task DeleteStatisticAsync_ShouldExistStatistic_ReturnHttpStatusCodeNoContent()
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

            _statisticServiceMock.Setup(service => service.GetStatisticAsync(It.IsAny<Guid>())).ReturnsAsync(statistic);
            _statisticServiceMock.Setup(service => service.DeleteStatisticAsync(It.IsAny<Statistic>()));

            // Act.
            var responseMessage = await _httpClient.DeleteAsync($"api/Statistics/{statistic.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///     Тестирование получения статистики.
        /// </summary>
        [Test]
        public async Task GetStatisticAsync_ShouldExistStatistic_ReturnHttpStatusCodeOK()
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

            _statisticServiceMock.Setup(service => service.GetStatisticAsync(It.IsAny<Guid>())).ReturnsAsync(statistic);

            // Act.
            var responseMessage = await _httpClient.GetAsync($"api/Statistics/{statistic.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование получения статистики по кофе.
        /// </summary>
        [Test]
        public async Task GetStatisticCoffeeAsync_ShouldExistStatisticCoffee_ReturnHttpStatusCodeOK()
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

            _statisticServiceMock.Setup(service => service.GetStatisticCoffeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(statistic);

            // Act.
            var responseMessage = await _httpClient.GetAsync($"api/Statistics/coffee/{statistic.Coffee.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование получения всех статистик.
        /// </summary>
        [Test]
        public async Task GetStatistics_ShouldExistStatistics_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var statistics = new List<Statistic>
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
                    Total = 500
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
                    Total = 700
                }
            };
            var statisticsAsyncEnumerable = statistics.ToAsyncEnumerable();
            var countStatisticsAsync = await statisticsAsyncEnumerable.CountAsync();
            var pageParametersCoffees = new ItemsParameters<Statistic>(statisticsAsyncEnumerable, countStatisticsAsync);

            _statisticServiceMock.Setup(service => service.GetStatisticsParameters(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>())).Returns(pageParametersCoffees);

            // Act.
            var responseMessage = await _httpClient.GetAsync("api/Statistics");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}