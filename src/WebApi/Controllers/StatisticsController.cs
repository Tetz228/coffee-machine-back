namespace WebApi.Controllers
{
    using Dtos.Statistics;

    using ErrorMessages;

    using Extensions.ItemsParameters;
    using Extensions.Models;

    using ItemsParameters;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services.Interfaces;

    /// <summary>
    ///     Контроллер для взаимодействия со статистик.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        /// <inheritdoc cref="IStatisticService" />
        private readonly IStatisticService _statisticService;

        /// <inheritdoc cref="StatisticsController" />
        /// <param name="statisticService">Сервис для работы с статистикой.</param>
        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        /// <summary>
        ///     Добавление новой статистики.
        /// </summary>
        /// <param name="creatingStatisticDto">DTO новой статистики.</param>
        /// <returns>DTO статистики в формате JSON.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<StatisticDto>> CreateStatisticAsync(
            CreateOrUpdateStatisticDto creatingStatisticDto)
        {
            var createdStatistic = await _statisticService.CreateStatisticAsync(creatingStatisticDto.ToModel());

            return createdStatistic is null
                ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                : CreatedAtAction("GetStatistic", new { id = createdStatistic.Id }, createdStatistic.ToDto());
        }

        /// <summary>
        ///     Обновление или создание статистики.
        /// </summary>
        /// <param name="id">Идентификатор статистики.</param>
        /// <param name="updatingStatisticDto">DTO обновленной статистики.</param>
        /// <returns>DTO статистики в формате JSON.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StatisticDto>> UpdateOrCreateStatisticAsync(Guid id,
            CreateOrUpdateStatisticDto updatingStatisticDto)
        {
            if (id == Guid.Empty)
            {
                var createdStatistic = await _statisticService.CreateStatisticAsync(updatingStatisticDto.ToModel());

                return createdStatistic is null
                    ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                    : CreatedAtAction("GetStatistic", new { id = createdStatistic.Id }, createdStatistic.ToDto());
            }

            var foundStatistic = await _statisticService.GetStatisticAsync(id);
            if (foundStatistic is null)
            {
                throw new ArgumentNullException(DbErrorMessages.FoundData);
            }

            var updatedStatistic =
                await _statisticService.UpdateStatisticAsync(foundStatistic, updatingStatisticDto.ToModel());

            return updatedStatistic is null
                ? throw new DbUpdateException(DbErrorMessages.UpdateData)
                : Ok(updatedStatistic.ToDto());
        }

        /// <summary>
        ///     Удаление статистики.
        /// </summary>
        /// <param name="id">Идентификатор статистики.</param>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStatisticAsync(Guid id)
        {
            var foundStatistic = await _statisticService.GetStatisticAsync(id);
            if (foundStatistic is null)
            {
                return NotFound();
            }

            await _statisticService.DeleteStatisticAsync(foundStatistic);

            return NoContent();
        }

        /// <summary>
        ///     Получение статистики.
        /// </summary>
        /// <param name="id">Идентификатор статистики.</param>
        /// <returns>DTO статистики в формате JSON.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StatisticDto>> GetStatisticAsync(Guid id)
        {
            var foundStatistic = await _statisticService.GetStatisticAsync(id);

            return foundStatistic is null ? NotFound() : Ok(foundStatistic.ToDto());
        }

        /// <summary>
        ///     Получение статистики по кофе.
        /// </summary>
        /// <param name="id">Идентификатор кофе.</param>
        /// <returns>DTO статистики по кофе в формате JSON.</returns>
        [HttpGet("coffee/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StatisticDto>> GetStatisticCoffeeAsync(Guid id)
        {
            var foundStatistic = await _statisticService.GetStatisticCoffeeAsync(id);

            return foundStatistic is null ? NotFound() : Ok(foundStatistic.ToDto());
        }

        /// <summary>
        ///     Получение параметров статистик.
        /// </summary>
        /// <param name="filter">Фильтр для поиска.</param>
        /// <param name="currentNumberPage">Текущий номер страницы.</param>
        /// <param name="countItemsPage">Количество элементов на странице.</param>
        /// <returns>Параметры DTO статистик в формате JSON.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ItemsParameters<StatisticDto>> GetStatisticsParameters(string? filter,
            int currentNumberPage,
            int countItemsPage)
        {
            var statisticsParameters =
                _statisticService.GetStatisticsParameters(filter, currentNumberPage, countItemsPage);

            return Ok(statisticsParameters.ToDto());
        }
    }
}