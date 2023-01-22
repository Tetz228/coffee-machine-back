namespace WebApiTests.Integrations.MockAuthentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Options;

    /// <summary>
    ///     Фиктивная схема провайдера.
    /// </summary>
    public class MockSchemeProvider : AuthenticationSchemeProvider
    {
        /// <summary>
        ///     Фиктивная схема провайдера.
        /// </summary>
        /// <param name="options">Настройки аутентификации.</param>
        public MockSchemeProvider(IOptions<AuthenticationOptions> options) : base(options)
        {
        }

        /// <summary>
        ///     Фиктивная схема провайдера.
        /// </summary>
        /// <param name="options">Настройки аутентификации.</param>
        /// <param name="schemes">Схемы провайдеров.</param>
        protected MockSchemeProvider(IOptions<AuthenticationOptions> options,
            IDictionary<string, AuthenticationScheme> schemes) : base(options, schemes)
        {
        }

        /// <summary>
        ///     Получение схемы.
        /// </summary>
        /// <param name="name">Название схемы.</param>
        /// <returns>Новая аутентификационная схема.</returns>
        public override Task<AuthenticationScheme> GetSchemeAsync(string name)
        {
            if (name != "Test")
            {
                return base.GetSchemeAsync(name);
            }

            var scheme = new AuthenticationScheme(
                "Test",
                "Test",
                typeof(MockAuthenticationHandler)
            );

            return Task.FromResult(scheme);
        }
    }
}