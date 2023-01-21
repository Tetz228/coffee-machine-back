namespace WebApi.Db
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    /// <summary>
    ///     Контекст базы данных.
    /// </summary>
    public class CoffeeMachineContext : DbContext
    {
        /// <summary>
        ///     Контекст базы данных.
        /// </summary>
        /// <param name="options">Настройки для контекста базы данных.</param>
        public CoffeeMachineContext(DbContextOptions<CoffeeMachineContext> options) : base(options)
        {
        }

        /// <summary>
        ///     Пользователи.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     Кофе.
        /// </summary>
        public DbSet<Coffee> Coffees { get; set; }

        /// <summary>
        ///     Заказы.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        ///     Статистики.
        /// </summary>
        public DbSet<Statistic> Statistics { get; set; }
    }
}