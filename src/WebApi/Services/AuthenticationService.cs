namespace WebApi.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using BCrypt.Net;

    using Db.Models;

    using Dtos.Users;

    using Interfaces;

    using Jwt;

    using Microsoft.IdentityModel.Tokens;

    using UoW.Interfaces;

    /// <inheritdoc cref="IAuthenticationService" />
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        ///     Конфигурация приложения.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <inheritdoc cref="IUnitOfWork" />
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IAuthenticationService" />
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <param name="unitOfWork">Единица работы.</param>
        public AuthenticationService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc cref="IAuthenticationService.LoginAsync" />
        public async Task<Jwt?> LoginAsync(AuthenticatedUser authenticatedUser)
        {
            var foundedUser =
                await _unitOfWork.UserRepository.GetUserAsync(authenticatedUser.Login);
            if (foundedUser is null || !BCrypt.Verify(authenticatedUser.Password, foundedUser.Password))
            {
                return null;
            }

            var tokens = new Jwt
            {
                AccessToken = GenerateAccessToken(foundedUser)
            };

            return tokens;
        }

        /// <summary>
        ///     Генерация токена доступа.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        /// <returns>Токен доступа.</returns>
        private string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Options:Jwt:Key"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Login", user.Login)
            };

            var tokenOptions = new JwtSecurityToken(_configuration["Options:Jwt:Issuer"],
                _configuration["Options:Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(int.Parse(_configuration["Options:Jwt:LifetimeAccessToken"])),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}