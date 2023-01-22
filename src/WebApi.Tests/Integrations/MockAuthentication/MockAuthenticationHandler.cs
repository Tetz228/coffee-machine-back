namespace WebApiTests.Integrations.MockAuthentication
{
    using System.Security.Claims;
    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    ///     Фиктивное аутентификационное событие.
    /// </summary>
    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        ///     Фиктивное аутентификационное событие.
        /// </summary>
        /// <param name="options">Настройки схемы аутентификации.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="encoder">Кодировщик.</param>
        /// <param name="clock">Системные часы.</param>
        public MockAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        ///     Обработка проверки подлинности.
        /// </summary>
        /// <returns>Аутентификационный тикет.</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim("Test", "Test")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}