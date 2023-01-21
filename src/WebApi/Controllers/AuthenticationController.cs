namespace WebApi.Controllers
{
    using Dtos.Users;

    using Jwt;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Interfaces;

    /// <summary>
    ///     Контроллер для взаимодействия с аутентификацией.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <inheritdoc cref="IAuthenticationService" />
        private readonly IAuthenticationService _authenticationService;

        /// <inheritdoc cref="AuthenticationController" />
        /// <param name="authenticationService">Сервис для работы с аутентификацией.</param>
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        ///     Авторизация пользователя.
        /// </summary>
        /// <param name="authenticatedUser">DTO авторизированного пользователя.</param>
        /// <returns>JWT в формате JSON.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Jwt>> LoginUserAsync(AuthenticatedUser authenticatedUser)
        {
            var tokens = await _authenticationService.LoginAsync(authenticatedUser);

            return tokens is null ? Unauthorized() : Ok(tokens);
        }
    }
}