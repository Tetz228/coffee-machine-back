namespace WebApi.Controllers
{
    using Dtos.Orders;

    using Enums;

    using ErrorMessages;

    using Extensions.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services.Interfaces;

    /// <summary>
    ///     Контроллер для взаимодействия с заказами.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        /// <inheritdoc cref="ICoffeeService" />
        private readonly ICoffeeService _coffeeService;

        /// <inheritdoc cref="IOrderService" />
        private readonly IOrderService _orderService;

        /// <inheritdoc cref="OrdersController" />
        /// <param name="orderService">Сервис для работы с заказом.</param>
        /// <param name="coffeeService">Сервис для работы с кофе.</param>
        public OrdersController(IOrderService orderService, ICoffeeService coffeeService)
        {
            _orderService = orderService;
            _coffeeService = coffeeService;
        }

        /// <summary>
        ///     Добавление нового заказа.
        /// </summary>
        /// <param name="creatingOrderDto">DTO нового заказа.</param>
        /// <returns>Сдача в формате JSON.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<Bills, int>>> CreateOrderAsync(
            CreateOrUpdateOrderDto creatingOrderDto)
        {
            if (creatingOrderDto.User.Balance < creatingOrderDto.Coffee.Price)
            {
                return BadRequest();
            }

            var changes = await _orderService.MakeOrderAsync(creatingOrderDto.ToModel());

            return changes is null
                ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                : Ok(changes);
        }

        /// <summary>
        ///     Обновление или создание заказа.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <param name="updatingOrderDto">DTO обновленного заказа.</param>
        /// <returns>DTO заказа в формате JSON.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> UpdateOrCreateOrderAsync(Guid id,
            CreateOrUpdateOrderDto updatingOrderDto)
        {
            if (id == Guid.Empty)
            {
                var changes = await _orderService.MakeOrderAsync(updatingOrderDto.ToModel());

                return changes is null
                    ? throw new ArgumentNullException(DbErrorMessages.CreateData)
                    : Ok(changes);
            }

            var foundOrder = await _orderService.GetOrderAsync(id);
            if (foundOrder is null)
            {
                throw new ArgumentNullException(DbErrorMessages.FoundData);
            }

            var updatedOrder = await _orderService.UpdateOrderAsync(foundOrder, updatingOrderDto.ToModel());

            return updatedOrder is null
                ? throw new DbUpdateException(DbErrorMessages.UpdateData)
                : Ok(updatedOrder.ToDto());
        }

        /// <summary>
        ///     Удаление заказа.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrderAsync(Guid id)
        {
            var foundOrder = await _orderService.GetOrderAsync(id);
            if (foundOrder is null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrderAsync(foundOrder);

            return NoContent();
        }

        /// <summary>
        ///     Получение заказа.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <returns>DTO заказа в формате JSON.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> GetOrderAsync(Guid id)
        {
            var foundOrder = await _orderService.GetOrderAsync(id);

            return foundOrder is null ? NotFound() : Ok(foundOrder.ToDto());
        }

        /// <summary>
        ///     Получение всех заказов.
        /// </summary>
        /// <returns>Перечисление DTO заказов в формате JSON.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IAsyncEnumerable<OrderDto>> GetAllOrders()
        {
            var foundOrders = _orderService.GetAllOrders();

            return Ok(foundOrders.ToDto());
        }
    }
}