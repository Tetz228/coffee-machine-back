namespace WebApiTests.Units.Base
{
    using Microsoft.EntityFrameworkCore;

    using WebApi.Db;
    using WebApi.UoW;
    using WebApi.UoW.Interfaces;

    /// <summary>
    ///     Базовый класс для тестирования репозиториев.
    /// </summary>
    public abstract class BaseRepositoryTests
    {
        /// <inheritdoc cref="CoffeeMachineContext" />
        protected readonly CoffeeMachineContext Context;

        /// <inheritdoc cref="IUnitOfWork" />
        protected readonly IUnitOfWork UnitOfWork;

        /// <inheritdoc cref="BaseRepositoryTests" />
        protected BaseRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<CoffeeMachineContext>()
                .UseInMemoryDatabase("CoffeeMachineDbTest").Options;
            Context = new CoffeeMachineContext(dbContextOptions);
            UnitOfWork = new UnitOfWork(Context);
        }
    }
}