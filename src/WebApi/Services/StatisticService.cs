namespace WebApi.Services
{
    using Db.Models;

    using ErrorMessages;

    using Interfaces;

    using ItemsParameters;

    using UoW.Interfaces;

    /// <inheritdoc cref="IStatisticService" />
    public class StatisticService : IStatisticService
    {
        /// <inheritdoc cref="IUnitOfWork" />
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IStatisticService" />
        /// <param name="unitOfWork">Единица работы.</param>
        public StatisticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc cref="IStatisticService.CreateStatisticAsync" />
        public async Task<Statistic?> CreateStatisticAsync(Statistic creatingStatistic)
        {
            creatingStatistic.Coffee = await _unitOfWork.CoffeeRepository.GetCoffeeAsync(creatingStatistic.Coffee.Id);

            if (creatingStatistic.Coffee is null)
            {
                return null;
            }

            await _unitOfWork.StatisticRepository.AddStatisticAsync(creatingStatistic);

            await _unitOfWork.SaveChangesAsync();

            return creatingStatistic;
        }

        /// <inheritdoc cref="IStatisticService.UpdateStatisticAsync" />
        public async Task<Statistic?> UpdateStatisticAsync(Statistic statistic, Statistic updatingStatistic)
        {
            statistic.Coffee = await _unitOfWork.CoffeeRepository.GetCoffeeAsync(updatingStatistic.Coffee.Id);

            if (statistic.Coffee is null)
            {
                return null;
            }

            statistic.Total = updatingStatistic.Total;

            _unitOfWork.StatisticRepository.UpdateStatistic(statistic);

            await _unitOfWork.SaveChangesAsync();

            return statistic;
        }

        /// <inheritdoc cref="IStatisticService.FindOrCreateAndSaveStatisticAsync" />
        public async Task<Statistic> FindOrCreateAndSaveStatisticAsync(Guid coffeeId)
        {
            var foundStatistic = await _unitOfWork.StatisticRepository.GetStatisticCoffeeAsync(coffeeId);
            if (foundStatistic is not null)
            {
                return foundStatistic;
            }

            var foundCoffee = await _unitOfWork.CoffeeRepository.GetCoffeeAsync(coffeeId);
            if (foundCoffee is null)
            {
                throw new ArgumentNullException(DbErrorMessages.FoundCoffee);
            }

            foundStatistic = new Statistic
            {
                Coffee = foundCoffee,
                Total = 0
            };

            await _unitOfWork.StatisticRepository.AddStatisticAsync(foundStatistic);

            await _unitOfWork.SaveChangesAsync();

            return foundStatistic;
        }

        /// <inheritdoc cref="IStatisticService.IncreaseTotalStatisticAsync" />
        public async Task IncreaseTotalStatisticAsync(Statistic statistic, Coffee coffee)
        {
            statistic.Total += coffee.Price;

            _unitOfWork.StatisticRepository.UpdateStatistic(statistic);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc cref="IStatisticService.DeleteStatisticAsync" />
        public async Task DeleteStatisticAsync(Statistic statistic)
        {
            _unitOfWork.StatisticRepository.DeleteStatistic(statistic);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc cref="IStatisticService.GetStatisticAsync" />
        public async Task<Statistic?> GetStatisticAsync(Guid id)
        {
            return await _unitOfWork.StatisticRepository.GetStatisticAsync(id);
        }

        /// <inheritdoc cref="IStatisticService.GetStatisticCoffeeAsync" />
        public async Task<Statistic?> GetStatisticCoffeeAsync(Guid id)
        {
            return await _unitOfWork.StatisticRepository.GetStatisticCoffeeAsync(id);
        }

        /// <inheritdoc cref="IStatisticService.GetStatisticsParameters" />
        public ItemsParameters<Statistic> GetStatisticsParameters(string? filter, int currentNumberPage,
            int countItemsPage)
        {
            filter = filter?.Trim();

            return _unitOfWork.StatisticRepository.GetStatisticsParameters(filter, currentNumberPage, countItemsPage);
        }
    }
}