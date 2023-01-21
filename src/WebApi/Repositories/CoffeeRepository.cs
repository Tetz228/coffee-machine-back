namespace WebApi.Repositories
{
    using Db;
    using Db.Models;

    using Interfaces;

    using ItemsParameters;

    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc cref="ICoffeeRepository" />
    public class CoffeeRepository : ICoffeeRepository
    {
        /// <inheritdoc cref="CoffeeMachineContext" />
        private readonly CoffeeMachineContext _context;

        /// <inheritdoc cref="ICoffeeRepository" />
        /// <param name="context">Контекст базы данных.</param>
        public CoffeeRepository(CoffeeMachineContext context)
        {
            _context = context;
        }

        /// <inheritdoc cref="ICoffeeRepository.AddCoffeeAsync" />
        public async Task AddCoffeeAsync(Coffee coffee)
        {
            await _context.Coffees.AddAsync(coffee);
        }

        /// <inheritdoc cref="ICoffeeRepository.UpdateCoffee" />
        public void UpdateCoffee(Coffee existingCoffee)
        {
            _context.Coffees.Update(existingCoffee);
        }

        /// <inheritdoc cref="ICoffeeRepository.DeleteCoffee" />
        public void DeleteCoffee(Coffee existingCoffee)
        {
            _context.Coffees.Remove(existingCoffee);
        }

        /// <inheritdoc cref="ICoffeeRepository.GetCoffeeAsync" />
        public async Task<Coffee?> GetCoffeeAsync(Guid id)
        {
            return await _context.Coffees.SingleOrDefaultAsync(coffee => coffee.Id == id);
        }

        /// <inheritdoc cref="ICoffeeRepository.GetCoffeesParameters" />
        public ItemsParameters<Coffee> GetCoffeesParameters(string? filter, int currentNumberPage, int countItemsPage)
        {
            var queryCoffees = string.IsNullOrEmpty(filter)
                ? _context.Coffees.OrderByDescending(coffee => coffee.Price)
                : _context.Coffees.Where(coffee => EF.Functions.ILike(coffee.Name, $"%{filter}%"))
                    .OrderByDescending(coffee => coffee.Price);

            return ItemsParameters<Coffee>.FormationItemsParameters(queryCoffees, currentNumberPage, countItemsPage);
        }
    }
}