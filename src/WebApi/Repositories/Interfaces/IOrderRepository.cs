namespace WebApi.Repositories.Interfaces
{
    using Db.Models;

    /// <summary>
    ///     Репозиторий для взаимодействия с заказом.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        ///     Добавление нового заказа.
        /// </summary>
        /// <param name="order">Модель заказа.</param>
        public Task AddOrderAsync(Order order);

        /// <summary>
        ///     Обновление заказа.
        /// </summary>
        /// <param name="existingOrder">Существующий заказ.</param>
        public void UpdateOrder(Order existingOrder);

        /// <summary>
        ///     Удаление заказа.
        /// </summary>
        /// <param name="existingOrder">Существующий заказ.</param>
        public void DeleteOrder(Order existingOrder);

        /// <summary>
        ///     Получение заказа.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <returns>Задача с моделью заказа.</returns>
        public Task<Order?> GetOrderAsync(Guid id);

        /// <summary>
        ///     Получение всех заказов.
        /// </summary>
        /// <returns>Перечисление заказов.</returns>
        public IAsyncEnumerable<Order> GetAllOrders();
    }
}