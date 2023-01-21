namespace WebApi.Dtos.Users
{
    /// <summary>
    ///     DTO авторизованного пользователя.
    /// </summary>
    public record AuthenticatedUser
    {
        /// <summary>
        ///     Логин пользователя.
        /// </summary>
        public string Login { get; init; }

        /// <summary>
        ///     Пароль пользователя.
        /// </summary>
        public string Password { get; init; }
    }
}