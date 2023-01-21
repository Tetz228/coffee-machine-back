namespace WebApi.Dtos.Statistics
{
    using System.ComponentModel.DataAnnotations;

    using Coffees;

    using ErrorMessages;

    /// <summary>
    ///     DTO добавленной или обновленной статистики.
    /// </summary>
    public record CreateOrUpdateStatisticDto
    {
        /// <summary>
        ///     DTO кофе.
        /// </summary>
        [Required(ErrorMessage = StatisticErrorMessages.StatisticCoffeeRequired)]
        public CoffeeDto Coffee { get; init; }

        /// <summary>
        ///     Общая сумма.
        /// </summary>
        [Required(ErrorMessage = StatisticErrorMessages.StatisticTotalRequired)]
        public decimal Total { get; init; }
    }
}