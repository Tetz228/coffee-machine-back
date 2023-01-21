namespace WebApi.UoW
{
    using Db;

    using Interfaces;

    using Repositories;
    using Repositories.Interfaces;

    /// <inheritdoc cref="IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        /// <inheritdoc cref="CoffeeMachineContext" />
        private readonly CoffeeMachineContext _context;

        /// <inheritdoc cref="IUnitOfWork" />
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="coffeeRepository">Репозиторий для взаимодействия с кофе.</param>
        /// <param name="orderRepository">Репозиторий для взаимодействия с заказом.</param>
        /// <param name="statisticRepository">Репозиторий для взаимодействия со статистикой.</param>
        /// <param name="userRepository">Репозиторий для взаимодействия с пользователем.</param>
        public UnitOfWork(CoffeeMachineContext context, ICoffeeRepository coffeeRepository,
            IOrderRepository orderRepository, IStatisticRepository statisticRepository, IUserRepository userRepository)
        {
            _context = context;
            CoffeeRepository = coffeeRepository;
            OrderRepository = orderRepository;
            StatisticRepository = statisticRepository;
            UserRepository = userRepository;
        }

        /// <inheritdoc cref="IUnitOfWork" />
        /// <param name="context">Контекст базы данных.</param>
        public UnitOfWork(CoffeeMachineContext context)
        {
            _context = context;
            CoffeeRepository = new CoffeeRepository(context);
            OrderRepository = new OrderRepository(context);
            StatisticRepository = new StatisticRepository(context);
            UserRepository = new UserRepository(context);
        }

        /// <inheritdoc cref="IUnitOfWork.CoffeeRepository" />
        public ICoffeeRepository CoffeeRepository { get; }

        /// <inheritdoc cref="IUnitOfWork.OrderRepository" />
        public IOrderRepository OrderRepository { get; }

        /// <inheritdoc cref="IUnitOfWork.StatisticRepository" />
        public IStatisticRepository StatisticRepository { get; }

        /// <inheritdoc cref="IUnitOfWork.UserRepository" />
        public IUserRepository UserRepository { get; }

        /// <inheritdoc cref="IUnitOfWork.SaveChangesAsync" />
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}