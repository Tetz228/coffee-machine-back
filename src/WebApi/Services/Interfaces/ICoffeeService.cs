namespace WebApi.Services.Interfaces
{
    using Db.Models;

    using ItemsParameters;

    /// <summary>
    ///     Сервис для работы с кофе.
    /// </summary>
    public interface ICoffeeService
    {
        /// <summary>
        ///     Создание нового кофе.
        /// </summary>
        /// <param name="creatingCoffee">Модель нового кофе.</param>
        /// <returns>Задача с моделью созданного кофе.</returns>
        public Task<Coffee?> CreateCoffeeAsync(Coffee creatingCoffee);

        /// <summary>
        ///     Обновление кофе.
        /// </summary>
        /// <param name="coffee">Модель кофе.</param>
        /// <param name="updatingCoffee">Модель обновленного кофе.</param>
        /// <returns>Задача с моделью обновленного кофе.</returns>
        public Task<Coffee?> UpdateCoffeeAsync(Coffee coffee, Coffee updatingCoffee);

        /// <summary>
        ///     Удаление кофе.
        /// </summary>
        /// <param name="coffee">Модель кофе.</param>
        public Task DeleteCoffeeAsync(Coffee coffee);

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