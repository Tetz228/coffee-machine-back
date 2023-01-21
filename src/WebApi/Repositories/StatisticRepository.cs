namespace WebApi.Repositories
{
    using Db;
    using Db.Models;

    using Interfaces;

    using ItemsParameters;

    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc cref="IStatisticRepository" />
    public class StatisticRepository : IStatisticRepository
    {
        /// <inheritdoc cref="CoffeeMachineContext" />
        private readonly CoffeeMachineContext _context;

        /// <inheritdoc cref="IStatisticRepository" />
        /// <param name="context">Контекст базы данных.</param>
        public StatisticRepository(CoffeeMachineContext context)
        {
            _context = context;
        }

        /// <inheritdoc cref="IStatisticRepository.AddStatisticAsync" />
        public async Task AddStatisticAsync(Statistic statistic)
        {
            await _context.Statistics.AddAsync(statistic);
        }

        /// <inheritdoc cref="IStatisticRepository.UpdateStatistic" />
        public void UpdateStatistic(Statistic existingStatistic)
        {
            _context.Statistics.Update(existingStatistic);
        }

        /// <inheritdoc cref="IStatisticRepository.DeleteStatistic" />
        public void DeleteStatistic(Statistic existingStatistic)
        {
            _context.Statistics.Remove(existingStatistic);
        }

        /// <inheritdoc cref="IStatisticRepository.GetStatisticAsync" />
        public async Task<Statistic?> GetStatisticAsync(Guid id)
        {
            return await GetStatisticQuery().SingleOrDefaultAsync(statistic => statistic.Id == id);
        }

        /// <inheritdoc cref="IStatisticRepository.GetStatisticCoffeeAsync" />
        public async Task<Statistic?> GetStatisticCoffeeAsync(Guid id)
        {
            return await GetStatisticQuery().SingleOrDefaultAsync(statistic => statistic.Coffee.Id == id);
        }

        /// <inheritdoc cref="IStatisticRepository.GetStatisticsParameters" />
        public ItemsParameters<Statistic> GetStatisticsParameters(string? filter, int currentNumberPage,
            int countItemsPage)
        {
            var queryStatistics = string.IsNullOrEmpty(filter)
                ? GetStatisticQuery().OrderByDescending(statistic => statistic.Total)
                : GetStatisticQuery().Where(statistic =>
                        EF.Functions.ILike(statistic.Coffee.Name, $"%{filter}%") ||
                        EF.Functions.ILike(statistic.Total.ToString(), $"%{filter}%"))
                    .OrderByDescending(statistic => statistic.Total);

            return ItemsParameters<Statistic>.FormationItemsParameters(queryStatistics, currentNumberPage,
                countItemsPage);
        }

        /// <summary>
        ///     Получение запроса статистики.
        /// </summary>
        /// <returns>Запрос статистики.</returns>
        private IQueryable<Statistic> GetStatisticQuery()
        {
            return _context.Statistics.Include(statistic => statistic.Coffee);
        }
    }
}