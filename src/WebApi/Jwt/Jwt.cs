namespace WebApi.Jwt
{
    /// <summary>
    ///     JSON Web Token.
    /// </summary>
    public record Jwt
    {
        /// <summary>
        ///     Токен доступа.
        /// </summary>
        public string? AccessToken { get; init; }

        /// <summary>
        ///     Токен обновления доступа.
        /// </summary>
        public string? RefreshToken { get; init; }
    }
}