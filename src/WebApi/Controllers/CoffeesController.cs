namespace WebApi.Controllers
{
    using Dtos.Coffees;

    using ErrorMessages;

    using Extensions.ItemsParameters;
    using Extensions.Models;

    using ItemsParameters;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services.Interfaces;

    /// <summary>
    ///     Контроллер для взаимодействия с кофе.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeesController : ControllerBase
    {
        /// <inheritdoc cref="ICoffeeService" />
        private readonly ICoffeeService _coffeeService;

        /// <inheritdoc cref="CoffeesController" />
        /// <param name="coffeeService">Сервис для работы с кофе.</param>
        public CoffeesController(ICoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        /// <summary>
        ///     Добавление нового кофе.
        /// </summary>
        /// <param name="creatingCoffeeDto">DTO нового кофе.</param>
        /// <returns>DTO кофе в формате JSON.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CoffeeDto>> CreateCoffeeAsync(CreateOrUpdateCoffeeDto creatingCoffeeDto)
        {
            var createdCoffee = await _coffeeService.CreateCoffeeAsync(creatingCoffeeDto.ToModel());

            return createdCoffee is null
                ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                : CreatedAtAction("GetCoffee", new { id = createdCoffee.Id }, createdCoffee.ToDto());
        }

        /// <summary>
        ///     Обновление или создание кофе.
        /// </summary>
        /// <param name="id">Идентификатор кофе.</param>
        /// <param name="updatingCoffeeDto">DTO обновленного кофе.</param>
        /// <returns>DTO кофе в формате JSON.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CoffeeDto>> UpdateOrCreateCoffeeAsync(Guid id,
            CreateOrUpdateCoffeeDto updatingCoffeeDto)
        {
            if (id == Guid.Empty)
            {
                var createdCoffee = await _coffeeService.CreateCoffeeAsync(updatingCoffeeDto.ToModel());

                return createdCoffee is null
                    ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                    : CreatedAtAction("GetCoffee", new { id = createdCoffee.Id }, createdCoffee.ToDto());
            }

            var foundCoffee = await _coffeeService.GetCoffeeAsync(id);
            if (foundCoffee is null)
            {
                throw new ArgumentNullException(DbErrorMessages.FoundData);
            }

            var updatedCoffee = await _coffeeService.UpdateCoffeeAsync(foundCoffee, updatingCoffeeDto.ToModel());

            return updatedCoffee is null
                ? throw new DbUpdateException(DbErrorMessages.UpdateData)
                : Ok(updatedCoffee.ToDto());
        }

        /// <summary>
        ///     Удаление кофе.
        /// </summary>
        /// <param name="id">Идентификатор кофе.</param>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteCoffeeAsync(Guid id)
        {
            var foundCoffee = await _coffeeService.GetCoffeeAsync(id);
            if (foundCoffee is null)
            {
                return NotFound();
            }

            await _coffeeService.DeleteCoffeeAsync(foundCoffee);

            return NoContent();
        }

        /// <summary>
        ///     Получение кофе.
        /// </summary>
        /// <param name="id">Идентификатор кофе.</param>
        /// <returns>DTO кофе в формате JSON.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CoffeeDto>> GetCoffeeAsync(Guid id)
        {
            var foundCoffee = await _coffeeService.GetCoffeeAsync(id);

            return foundCoffee is null ? NotFound() : Ok(foundCoffee.ToDto());
        }

        /// <summary>
        ///     Получение параметров видов кофе.
        /// </summary>
        /// <param name="filter">Фильтр для поиска.</param>
        /// <param name="currentNumberPage">Текущий номер страницы.</param>
        /// <param name="countItemsPage">Количество элементов на странице.</param>
        /// <returns>Параметры DTO видов кофе в формате JSON.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ItemsParameters<CoffeeDto>> GetCoffeesParameters(string? filter, int currentNumberPage,
            int countItemsPage)
        {
            var coffeesParameters = _coffeeService.GetCoffeesParameters(filter, currentNumberPage, countItemsPage);

            return Ok(coffeesParameters.ToDto());
        }
    }
}