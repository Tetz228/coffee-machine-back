namespace WebApi.Services.Interfaces
{
    using Db.Models;

    using ItemsParameters;

    /// <summary>
    ///     Сервис для работы со статистикой.
    /// </summary>
    public interface IStatisticService
    {
        /// <summary>
        ///     Создание новой статистики.
        /// </summary>
        /// <param name="creatingStatistic">DTO новой статистики.</param>
        /// <returns>Задача с моделью созданной статистики.</returns>
        public Task<Statistic?> CreateStatisticAsync(Statistic creatingStatistic);

        /// <summary>
        ///     Обновление статистики.
        /// </summary>
        /// <param name="statistic">Модель статистики.</param>
        /// <param name="updatingStatistic">Модель обновленной статистики.</param>
        /// <returns>Задача с моделью обновленной статистики.</returns>
        public Task<Statistic?> UpdateStatisticAsync(Statistic statistic, Statistic updatingStatistic);

        /// <summary>
        ///     Найти или создать и сохранить статистику.
        /// </summary>
        /// <param name="coffeeId">Идентификатор кофе.</param>
        /// <returns>Задача с моделью статистики.</returns>
        public Task<Statistic> FindOrCreateAndSaveStatisticAsync(Guid coffeeId);

        /// <summary>
        ///     Увеличить общую сумму статистики.
        /// </summary>
        /// <param name="statistic">Модель статистики.</param>
        /// <param name="coffee">Модель кофе.</param>
        public Task IncreaseTotalStatisticAsync(Statistic statistic, Coffee coffee);

        /// <summary>
        ///     Удаление статистики.
        /// </summary>
        /// <param name="statistic">Модель статистики.</param>
        public Task DeleteStatisticAsync(Statistic statistic);

        /// <summary>
        ///     Получение статистики.
        /// </summary>
        /// <param name="id">Идентификатор статистики.</param>
        /// <returns>Задача с моделью cтатистики.</returns>
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