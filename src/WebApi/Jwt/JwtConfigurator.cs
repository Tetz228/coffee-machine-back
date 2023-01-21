namespace WebApi.Jwt
{
    using System.Text;

    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///     JWT конфигуратор.
    /// </summary>
    public static class JwtConfigurator
    {
        /// <summary>
        ///     Создания набора параметров для проверки токена.
        /// </summary>
        /// <param name="issuer">Создатель токена.</param>
        /// <param name="audience">Получатель токена.</param>
        /// <param name="key">Ключ подписи.</param>
        /// <returns>Набор параметров для проверки токена.</returns>
        public static TokenValidationParameters CreateTokenValidationParameters(string issuer, string audience,
            string key)
        {
            return new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        }
    }
}