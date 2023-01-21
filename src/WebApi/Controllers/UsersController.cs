namespace WebApi.Controllers
{
    using Dtos.Users;

    using Enums;

    using ErrorMessages;

    using Extensions.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services.Interfaces;

    /// <summary>
    ///     Контроллер для взаимодействия с пользователями.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <inheritdoc cref="IUserService" />
        private readonly IUserService _userService;

        /// <inheritdoc cref="UsersController" />
        /// <param name="userService">Сервис для работы с пользователем.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Добавление нового пользователя.
        /// </summary>
        /// <param name="creatingUserDto">DTO нового пользователя.</param>
        /// <returns>DTO пользователя в формате JSON.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> CreateUserAsync(CreateOrUpdateUserDto creatingUserDto)
        {
            var isLoginUserUnique = await _userService.IsLoginUserUnique(creatingUserDto.Login);

            if (!isLoginUserUnique)
            {
                return BadRequest();
            }

            var createdUser = await _userService.CreateUserAsync(creatingUserDto.ToModel());

            return createdUser is null
                ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                : CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser.ToDto());
        }

        /// <summary>
        ///     Обновление или создание пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="updatingUserDto">DTO обновленного пользователя.</param>
        /// <returns>DTO пользователя в формате JSON.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> UpdateOrCreateUserAsync(Guid id, CreateOrUpdateUserDto updatingUserDto)
        {
            if (id == Guid.Empty)
            {
                var isLoginUserUnique = await _userService.IsLoginUserUnique(updatingUserDto.Login);

                if (!isLoginUserUnique)
                {
                    return BadRequest();
                }

                var createdUser = await _userService.CreateUserAsync(updatingUserDto.ToModel());

                return createdUser is null
                    ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                    : CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser.ToDto());
            }

            var foundUser = await _userService.GetUserAsync(id);
            if (foundUser is null)
            {
                throw new ArgumentNullException(DbErrorMessages.FoundData);
            }

            var updatedUser = await _userService.UpdateUserAsync(foundUser, updatingUserDto.ToModel());

            return updatedUser is null
                ? throw new DbUpdateException(DbErrorMessages.UpdateData)
                : Ok(updatedUser.ToDto());
        }

        /// <summary>
        ///     Удаление пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            var foundUser = await _userService.GetUserAsync(id);
            if (foundUser is null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(foundUser);

            return NoContent();
        }

        /// <summary>
        ///     Получение пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>DTO пользователя в формате JSON.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserAsync(Guid id)
        {
            var foundUser = await _userService.GetUserAsync(id);

            return foundUser is null ? NotFound() : Ok(foundUser.ToDto());
        }

        /// <summary>
        ///     Получение всех пользователей.
        /// </summary>
        /// <returns>Перечисление DTO пользователей в формате JSON.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IAsyncEnumerable<UserDto>> GetAllUsers()
        {
            var foundUsers = _userService.GetAllUsers();

            return Ok(foundUsers.ToDto());
        }

        /// <summary>
        ///     Пополнение баланса.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="sum">Сумма пополнения.</param>
        [HttpPut("{id}/balance")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> UpdateBalanceUserAsync([FromRoute] Guid id, [FromBody] decimal sum)
        {
            var isInteger = int.TryParse(sum.ToString(), out var bill);
            if (!isInteger)
            {
                return BadRequest();
            }

            var isReplenishmentSumForBills = _userService.CheckReplenishmentSumForBills((Bills)bill);
            if (!isReplenishmentSumForBills)
            {
                return BadRequest();
            }

            var foundUser = await _userService.GetUserAsync(id);
            if (foundUser is null)
            {
                throw new ArgumentNullException(DbErrorMessages.FoundData);
            }

            await _userService.UpdateBalanceUserAsync(foundUser, sum, true);

            return Ok(foundUser.ToDto());
        }
    }
}