namespace WebApi.Dtos.Orders
{
    using System.ComponentModel.DataAnnotations;

    using Coffees;

    using ErrorMessages;

    using Users;

    /// <summary>
    ///     DTO заказа.
    /// </summary>
    public record OrderDto
    {
        /// <summary>
        ///     Идентификатор заказа.
        /// </summary>
        public Guid Id { get; init; }

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