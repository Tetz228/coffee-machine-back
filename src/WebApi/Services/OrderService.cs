namespace WebApi.Services
{
    using Db.Models;

    using Enums;

    using Interfaces;

    using UoW.Interfaces;

    /// <inheritdoc cref="IOrderService" />
    public class OrderService : IOrderService
    {
        /// <inheritdoc cref="IChangeService" />
        private readonly IChangeService _changeService;

        /// <inheritdoc cref="IStatisticService" />
        private readonly IStatisticService _statisticService;

        /// <inheritdoc cref="IUnitOfWork" />
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IUserService" />
        private readonly IUserService _userService;

        /// <inheritdoc cref="IOrderService" />
        /// <param name="unitOfWork">Единица работы.</param>
        /// <param name="statisticService">Сервис для работы со статистикой.</param>
        /// <param name="changeService">Сервис для работы со сдачей.</param>
        /// <param name="userService">Сервис для работы с пользователями.</param>
        public OrderService(IUnitOfWork unitOfWork, IStatisticService statisticService, IChangeService changeService,
            IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _statisticService = statisticService;
            _changeService = changeService;
            _userService = userService;
        }

        /// <inheritdoc cref="IOrderService.MakeOrderAsync" />
        public async Task<Dictionary<Bills, int>?> MakeOrderAsync(Order creatingOrder)
        {
            creatingOrder.Coffee = await _unitOfWork.CoffeeRepository.GetCoffeeAsync(creatingOrder.Coffee.Id);
            creatingOrder.User = await _unitOfWork.UserRepository.GetUserAsync(creatingOrder.User.Id);

            if (creatingOrder.Coffee is null || creatingOrder.User is null)
            {
                return null;
            }

            await _userService.UpdateBalanceUserAsync(creatingOrder.User, creatingOrder.Coffee.Price, false);

            await _unitOfWork.OrderRepository.AddOrderAsync(creatingOrder);

            await _unitOfWork.SaveChangesAsync();

            var foundStatistic = await _statisticService.FindOrCreateAndSaveStatisticAsync(creatingOrder.Coffee.Id);

            await _statisticService.IncreaseTotalStatisticAsync(foundStatistic, creatingOrder.Coffee);

            var change = await _changeService.CalculateChange(creatingOrder.User);

            return change;
        }

        /// <inheritdoc cref="IOrderService.UpdateOrderAsync" />
        public async Task<Order?> UpdateOrderAsync(Order order, Order updatingOrder)
        {
            order.Coffee = await _unitOfWork.CoffeeRepository.GetCoffeeAsync(updatingOrder.Coffee.Id);
            order.User = await _unitOfWork.UserRepository.GetUserAsync(updatingOrder.User.Id);

            if (order.Coffee is null)
            {
                return null;
            }

            _unitOfWork.OrderRepository.UpdateOrder(order);

            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        /// <inheritdoc cref="IOrderService.DeleteOrderAsync" />
        public async Task DeleteOrderAsync(Order order)
        {
            _unitOfWork.OrderRepository.DeleteOrder(order);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc cref="IOrderService.GetOrderAsync" />
        public async Task<Order?> GetOrderAsync(Guid id)
        {
            return await _unitOfWork.OrderRepository.GetOrderAsync(id);
        }

        /// <inheritdoc cref="IOrderService.GetAllOrders" />
        public IAsyncEnumerable<Order> GetAllOrders()
        {
            return _unitOfWork.OrderRepository.GetAllOrders();
        }
    }
}