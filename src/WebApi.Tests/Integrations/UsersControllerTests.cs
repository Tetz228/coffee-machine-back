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
    using WebApi.Dtos.Users;
    using WebApi.Enums;
    using WebApi.Services.Interfaces;

    /// <summary>
    ///     Тестирование контроллера для взаимодействия с пользователями.
    /// </summary>
    [TestFixture]
    public class UsersControllerTests
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

                    var userServiceDescriptor = collection.SingleOrDefault(descriptor =>
                        descriptor.ServiceType == typeof(IUserService));

                    collection.Remove(userServiceDescriptor);

                    _userServiceMock = new Mock<IUserService>();

                    collection.AddScoped<IUserService>(_ => _userServiceMock.Object);
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
        ///     Фиктивный сервис для работы с пользователем.
        /// </summary>
        private Mock<IUserService> _userServiceMock;

        /// <summary>
        ///     Клиент HTTP.
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        ///     Тестирование добавление нового пользователя.
        /// </summary>
        [Test]
        public async Task CreateUserAsync_ShouldCreateNewUser_ReturnHttpStatusCodeCreated()
        {
            // Arrange.
            var createUser = new CreateOrUpdateUserDto
            {
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(createUser), Encoding.UTF8,
                "application/json");

            _userServiceMock.Setup(service => service.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            });
            _userServiceMock.Setup(service => service.IsLoginUserUnique(It.IsAny<string>())).ReturnsAsync(true);

            // Act.
            var responseMessage = await _httpClient.PostAsync("api/Users", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        /// <summary>
        ///     Тестирование обновления или создания пользователя.
        /// </summary>
        [Test]
        public async Task UpdateOrCreateUserAsync_ShouldUpdateUser_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var updateUserDto = new CreateOrUpdateUserDto
            {
                Login = "Lord",
                Password = "123456789",
                Name = "Олег"
            };
            var foundUser = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(updateUserDto), Encoding.UTF8,
                "application/json");

            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(foundUser);
            _userServiceMock.Setup(service => service.UpdateUserAsync(It.IsAny<User>(), It.IsAny<User>()))
                .ReturnsAsync(new User
                {
                    Id = foundUser.Id,
                    Login = updateUserDto.Login,
                    Password = updateUserDto.Password,
                    Name = updateUserDto.Name,
                    Balance = foundUser.Balance
                });

            // Act.
            var responseMessage = await _httpClient.PutAsync($"api/Users/{foundUser.Id}", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование удаления пользователя.
        /// </summary>
        [Test]
        public async Task DeleteUserAsync_ShouldExistUser_ReturnHttpStatusCodeNoContent()
        {
            // Arrange.
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _userServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<User>()));

            // Act.
            var responseMessage = await _httpClient.DeleteAsync($"api/Users/{user.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///     Тестирование получения пользователя.
        /// </summary>
        [Test]
        public async Task GetUserAsync_ShouldExistUser_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Act.
            var responseMessage = await _httpClient.GetAsync($"api/Users/{user.Id}");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование обновления баланса пользователя.
        /// </summary>
        [Test]
        public async Task UpdateBalanceUserAsync_ShouldUpdateBalance_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            const decimal sum = 1500;
            var foundUser = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(sum), Encoding.UTF8,
                "application/json");

            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(foundUser);
            _userServiceMock.Setup(service => service.CheckReplenishmentSumForBills(It.IsAny<Bills>())).Returns(true);
            _userServiceMock.Setup(service =>
                service.UpdateBalanceUserAsync(It.IsAny<User>(), It.IsAny<decimal>(), true));

            // Act.
            var responseMessage = await _httpClient.PutAsync($"api/Users/{foundUser.Id}/balance", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Тестирование получения всех пользователей.
        /// </summary>
        [Test]
        public async Task GetUsers_ShouldExistUsers_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var users = new List<User>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Login = "Lord",
                    Password = "qwerty123",
                    Name = "Олег",
                    Balance = 1500
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Login = "Kobok",
                    Password = "123456789",
                    Name = "Дмитрий",
                    Balance = 2500
                }
            };
            var usersAsync = GetIAsyncEnumerable(users);

            _userServiceMock.Setup(service => service.GetAllUsers()).Returns(usersAsync);

            // Act.
            var responseMessage = await _httpClient.GetAsync("api/Users");

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Получение асинхронного перечисление пользователей.
        /// </summary>
        /// <param name="users">Список пользователей.</param>
        /// <returns>Асинхронное перечисление пользователей.</returns>
        private static async IAsyncEnumerable<User> GetIAsyncEnumerable(List<User> users)
        {
            foreach (var user in users)
            {
                yield return user;
            }
        }
    }
}