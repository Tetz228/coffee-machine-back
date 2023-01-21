namespace WebApi.Dtos.Coffees
{
    using System.ComponentModel.DataAnnotations;

    using ErrorMessages;

    /// <summary>
    ///     DTO кофе.
    /// </summary>
    public record CoffeeDto
    {
        /// <summary>
        ///     Идентификатор кофе.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        ///     Название кофе.
        /// </summary>
        [Required(ErrorMessage = CoffeeErrorMessages.CoffeeNameRequired)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = CoffeeErrorMessages.CoffeeNameLength)]
        public string Name { get; init; }

        /// <summary>
        ///     Цена за кофе.
        /// </summary>
        [Required(ErrorMessage = CoffeeErrorMessages.CoffeePriceRequired)]
        public decimal Price { get; init; }
    }
}