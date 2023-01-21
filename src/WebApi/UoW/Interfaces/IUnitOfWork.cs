namespace WebApi.UoW.Interfaces
{
    using Repositories.Interfaces;

    /// <summary>
    ///     Единица работы.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <inheritdoc cref="ICoffeeRepository" />
        public ICoffeeRepository CoffeeRepository { get; }

        /// <inheritdoc cref="IOrderRepository" />
        public IOrderRepository OrderRepository { get; }

        /// <inheritdoc cref="IStatisticRepository" />
        public IStatisticRepository StatisticRepository { get; }

        /// <inheritdoc cref="IUserRepository" />
        public IUserRepository UserRepository { get; }

        /// <summary>
        ///     Сохранение изменений.
        /// </summary>
        public Task SaveChangesAsync();
    }
}