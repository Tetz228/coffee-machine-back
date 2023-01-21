namespace WebApi.Extensions.Models
{
    using Db.Models;

    using Dtos.Statistics;

    /// <summary>
    ///     Расширения для статистики.
    /// </summary>
    public static class StatisticExtensions
    {
        /// <summary>
        ///     Маппинг DTO новой или обновленной статистики в модель.
        /// </summary>
        /// <param name="updatingStatisticDto">DTO новой статистики.</param>
        /// <returns>Модель статистики.</returns>
        public static Statistic ToModel(this CreateOrUpdateStatisticDto updatingStatisticDto)
        {
            return new Statistic
            {
                Coffee = updatingStatisticDto.Coffee.ToModel(),
                Total = updatingStatisticDto.Total
            };
        }

        /// <summary>
        ///     Маппинг модели статистики в DTO.
        /// </summary>
        /// <param name="statistic">Модель статистики.</param>
        /// <returns>DTO статистики.</returns>
        public static StatisticDto ToDto(this Statistic statistic)
        {
            return new StatisticDto
            {
                Id = statistic.Id,
                Coffee = statistic.Coffee.ToDto(),
                Total = statistic.Total
            };
        }

        /// <summary>
        ///     Маппинг моделей перечисления статистики в DTO.
        /// </summary>
        /// <param name="statistics">Перечисление моделей кофе.</param>
        /// <returns>Перечисление DTO кофе.</returns>
        public static IAsyncEnumerable<StatisticDto> ToDto(this IAsyncEnumerable<Statistic> statistics)
        {
            return statistics.Select(statistic => statistic.ToDto());
        }
    }
}