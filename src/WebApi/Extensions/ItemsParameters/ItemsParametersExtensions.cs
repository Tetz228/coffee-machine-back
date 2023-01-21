namespace WebApi.Extensions.ItemsParameters
{
    using Db.Models;

    using Dtos.Coffees;
    using Dtos.Statistics;

    using Models;

    using WebApi.ItemsParameters;

    /// <summary>
    ///     Расширения для параметров элементов.
    /// </summary>
    public static class ItemsParametersExtensions
    {
        /// <summary>
        ///     Маппинг модели кофе параметров элементов в DTO.
        /// </summary>
        /// <param name="itemsParametersCoffees">Параметры элементов кофе.</param>
        /// <returns>Параметры элементов кофе DTO.</returns>
        public static ItemsParameters<CoffeeDto> ToDto(this ItemsParameters<Coffee> itemsParametersCoffees)
        {
            return new ItemsParameters<CoffeeDto>(itemsParametersCoffees.Items.ToDto(),
                itemsParametersCoffees.TotalCountItems);
        }

        /// <summary>
        ///     Маппинг модели статистики параметров элементов в DTO.
        /// </summary>
        /// <param name="itemsParametersStatistics">Параметры элементов статистики</param>
        /// <returns>Параметры элементов статистики DTO.</returns>
        public static ItemsParameters<StatisticDto> ToDto(this ItemsParameters<Statistic> itemsParametersStatistics)
        {
            return new ItemsParameters<StatisticDto>(itemsParametersStatistics.Items.ToDto(),
                itemsParametersStatistics.TotalCountItems);
        }
    }
}