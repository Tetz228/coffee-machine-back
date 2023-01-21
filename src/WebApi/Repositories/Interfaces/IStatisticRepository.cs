namespace WebApi.Repositories.Interfaces
{
    using Db.Models;

    using ItemsParameters;

    /// <summary>
    ///     Репозиторий для взаимодействия со статистикой.
    /// </summary>
    public interface IStatisticRepository
    {
        /// <summary>
        ///     Добавление новой статистики.
        /// </summary>
        /// <param name="statistic">Модель статистики.</param>
        public Task AddStatisticAsync(Statistic statistic);

        /// <summary>
        ///     Обновление статистики.
        /// </summary>
        /// <param name="existingStatistic">Существующая статистика.</param>
        public void UpdateStatistic(Statistic existingStatistic);

        /// <summary>
        ///     Удаление статистики.
        /// </summary>
        /// <param name="existingStatistic">Существующая статистика.</param>
        public void DeleteStatistic(Statistic existingStatistic);

        /// <summary>
        ///     Получение статистики.
        /// </summary>
        /// <param name="id">Идентификатор статистики.</param>
        /// <returns>Задача с моделью статистики.</returns>
        public Task<Statistic?> GetStatisticAsync(Guid id);

        /// <summary>
        ///     Получение статистики по кофе.
        /// </summary>
        /// <param name="id">Идентификатор кофе.</param>
        /// <returns>Задача с моделью cтатистики.</returns>
        public Task<Statistic?> GetStatisticCoffeeAsync(Guid id);

        /// <summary>
        ///     Получение параметров статистик.
        /// </summary>
        /// <param name="filter">Фильтр для поиска.</param>
        /// <param name="currentNumberPage">Номер текущей страницы.</param>
        /// <param name="countItemsPage">Количество элементов на странице.</param>
        /// <returns>Параметры статистик.</returns>
        public ItemsParameters<Statistic> GetStatisticsParameters(string? filter, int currentNumberPage,
            int countItemsPage);
    }
}