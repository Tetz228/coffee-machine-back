namespace WebApi.Repositories.Interfaces
{
    using Db.Models;

    using ItemsParameters;

    /// <summary>
    ///     Репозиторий для взаимодействия с кофе.
    /// </summary>
    public interface ICoffeeRepository
    {
        /// <summary>
        ///     Добавление нового кофе.
        /// </summary>
        /// <param name="coffee">Модель кофе.</param>
        public Task AddCoffeeAsync(Coffee coffee);

        /// <summary>
        ///     Обновление кофе.
        /// </summary>
        /// <param name="existingCoffee">Существующий кофе.</param>
        public void UpdateCoffee(Coffee existingCoffee);

        /// <summary>
        ///     Удаление кофе.
        /// </summary>
        /// <param name="existingCoffee">Существующий кофе.</param>
        public void DeleteCoffee(Coffee existingCoffee);

        /// <summary>
        ///     Получение кофе.
        /// </summary>
        /// <param name="id">Идентификатор кофе.</param>
        /// <returns>Задача с моделью кофе.</returns>
        public Task<Coffee?> GetCoffeeAsync(Guid id);

        /// <summary>
        ///     Получение параметров видов кофе.
        /// </summary>
        /// <param name="filter">Фильтр для поиска.</param>
        /// <param name="currentNumberPage">Текущий номер страницы.</param>
        /// <param name="countItemsPage">Количество элементов на странице.</param>
        /// <returns>Параметры видов кофе.</returns>
        public ItemsParameters<Coffee> GetCoffeesParameters(string? filter, int currentNumberPage, int countItemsPage);
    }
}