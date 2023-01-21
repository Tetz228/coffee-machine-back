namespace WebApi.Services.Interfaces
{
    using Dtos.Users;

    using Jwt;

    /// <summary>
    ///     Сервис для работы с аутентификацией.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        ///     Авторизация пользователя.
        /// </summary>
        /// <param name="authenticatedUser">DTO авторизированного пользователя.</param>
        /// <returns>Задача с токенами доступа.</returns>
        public Task<Jwt?> LoginAsync(AuthenticatedUser authenticatedUser);
    }
}