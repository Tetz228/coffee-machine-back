namespace WebApi.Services.Interfaces
{
    using Db.Models;

    using Enums;

    /// <summary>
    ///     Сервис для работы с заказами.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        ///     Создание нового заказа.
        /// </summary>
        /// <param name="creatingOrder">Модель нового заказа.</param>
        /// <returns>Задача со сдачей.</returns>
        public Task<Dictionary<Bills, int>?> MakeOrderAsync(Order creatingOrder);

        /// <summary>
        ///     Обновление заказа.
        /// </summary>
        /// <param name="order">Модель заказа.</param>
        /// <param name="updatingOrder">Модель обновленного заказа.</param>
        /// <returns>Задача с моделью обновленного заказа.</returns>
        public Task<Order?> UpdateOrderAsync(Order order, Order updatingOrder);

        /// <summary>
        ///     Удаление заказа.
        /// </summary>
        /// <param name="order">Модель заказа.</param>
        public Task DeleteOrderAsync(Order order);

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