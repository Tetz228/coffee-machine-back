namespace WebApi.Repositories
{
    using Db;
    using Db.Models;

    using Interfaces;

    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc cref="IOrderRepository" />
    public class OrderRepository : IOrderRepository
    {
        /// <inheritdoc cref="CoffeeMachineContext" />
        private readonly CoffeeMachineContext _context;

        /// <inheritdoc cref="IOrderRepository" />
        /// <param name="context">Контекст базы данных.</param>
        public OrderRepository(CoffeeMachineContext context)
        {
            _context = context;
        }

        /// <inheritdoc cref="IOrderRepository.AddOrderAsync" />
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        /// <inheritdoc cref="IOrderRepository.UpdateOrder" />
        public void UpdateOrder(Order existingOrder)
        {
            _context.Orders.Update(existingOrder);
        }

        /// <inheritdoc cref="IOrderRepository.DeleteOrder" />
        public void DeleteOrder(Order existingOrder)
        {
            _context.Orders.Remove(existingOrder);
        }

        /// <inheritdoc cref="IOrderRepository.GetOrderAsync" />
        public async Task<Order?> GetOrderAsync(Guid id)
        {
            return await GetOrderQuery().SingleOrDefaultAsync(order => order.Id == id);
        }

        /// <inheritdoc cref="IOrderRepository.GetAllOrders" />
        public IAsyncEnumerable<Order> GetAllOrders()
        {
            return GetOrderQuery().AsAsyncEnumerable();
        }

        /// <summary>
        ///     Получение запроса заказа.
        /// </summary>
        /// <returns>Запрос заказа.</returns>
        private IQueryable<Order> GetOrderQuery()
        {
            return _context.Orders
                .Include(order => order.User)
                .Include(order => order.Coffee);
        }
    }
}