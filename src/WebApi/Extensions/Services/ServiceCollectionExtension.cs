namespace WebApi.Extensions.Services
{
    using Db;

    using Microsoft.EntityFrameworkCore;

    using Repositories;
    using Repositories.Interfaces;

    using UoW;
    using UoW.Interfaces;

    /// <summary>
    ///     Расширения для коллекции сервисов.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        ///     Добавление репозиториев и контекста базы данных в коллекцию сервисов.
        /// </summary>
        /// <param name="serviceCollection">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        public static void AddRepositoriesAndDbContext(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddDbContext<CoffeeMachineContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
            serviceCollection.AddScoped<ICoffeeRepository, CoffeeRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
            serviceCollection.AddScoped<IStatisticRepository, StatisticRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}