namespace WebApi.Dtos.Orders
{
    using System.ComponentModel.DataAnnotations;

    using Coffees;

    using ErrorMessages;

    using Users;

    /// <summary>
    ///     DTO добавленного или обновленного заказа.
    /// </summary>
    public record CreateOrUpdateOrderDto
    {
        /// <summary>
        ///     DTO кофе.
        /// </summary>
        [Required(ErrorMessage = OrderErrorMessages.OrderCoffeeRequired)]
        public CoffeeDto Coffee { get; init; }

        /// <summary>
        ///     DTO пользователя.
        /// </summary>
        public UserDto? User { get; init; }
    }
}