namespace WebApi.Extensions.Services
{
    using Db;

    using Microsoft.EntityFrameworkCore;

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
        }
    }
}