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
    using WebApi.Dtos.Users;
    using WebApi.Jwt;

    using IAuthenticationService = WebApi.Services.Interfaces.IAuthenticationService;

    /// <summary>
    ///     Тестирование контроллера для взаимодействия с аутентификацией.
    /// </summary>
    [TestFixture]
    public class AuthenticationControllerTests
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

                    var authenticationServiceDescriptor = collection.SingleOrDefault(descriptor =>
                        descriptor.ServiceType == typeof(IAuthenticationService));

                    collection.Remove(authenticationServiceDescriptor);

                    _authenticationServiceMock = new Mock<IAuthenticationService>();

                    collection.AddScoped<IAuthenticationService>(_ => _authenticationServiceMock.Object);
                    collection.AddTransient<IAuthenticationSchemeProvider, MockSchemeProvider>();
                });
            });

            _httpClient = appBuilder.CreateClient();
        }

        /// <summary>
        ///     Освобождение ресурсов после тестирования.
        /// </summary>
        [TearDown]
        public void OneTimeTearDown()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        ///     Фиктивный сервис для работы с аутентификацией.
        /// </summary>
        private Mock<IAuthenticationService> _authenticationServiceMock;

        /// <summary>
        ///     Клиент HTTP.
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        ///     Тестирование авторизации пользователя.
        /// </summary>
        [Test]
        public async Task LoginUserAsync_ShouldExistUser_ReturnHttpStatusCodeOK()
        {
            // Arrange.
            var createAuthentication = new AuthenticatedUser
            {
                Login = "Lord",
                Password = "qwerty123"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(createAuthentication), Encoding.UTF8,
                "application/json");

            _authenticationServiceMock.Setup(service => service.LoginAsync(It.IsAny<AuthenticatedUser>())).ReturnsAsync(
                new Jwt
                {
                    AccessToken = "Test",
                    RefreshToken = "Test"
                });

            // Act.
            var responseMessage = await _httpClient.PostAsync("api/Authentication", stringContent);

            // Assert.
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}