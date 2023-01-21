namespace WebApi.Services
{
    using Db.Models;

    using Extensions.Models;

    using Interfaces;

    using ItemsParameters;

    using UoW.Interfaces;

    /// <inheritdoc cref="ICoffeeService" />
    public class CoffeeService : ICoffeeService
    {
        /// <inheritdoc cref="IUnitOfWork" />
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="ICoffeeService" />
        /// <param name="unitOfWork">Единица работы.</param>
        public CoffeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc cref="ICoffeeService.CreateCoffeeAsync" />
        public async Task<Coffee?> CreateCoffeeAsync(Coffee creatingCoffee)
        {
            await _unitOfWork.CoffeeRepository.AddCoffeeAsync(creatingCoffee);

            await _unitOfWork.SaveChangesAsync();

            return creatingCoffee;
        }

        /// <inheritdoc cref="ICoffeeService.UpdateCoffeeAsync" />
        public async Task<Coffee?> UpdateCoffeeAsync(Coffee coffee, Coffee updatingCoffee)
        {
            coffee.UpdateFromModel(updatingCoffee);

            _unitOfWork.CoffeeRepository.UpdateCoffee(coffee);

            await _unitOfWork.SaveChangesAsync();

            return coffee;
        }

        /// <inheritdoc cref="ICoffeeService.DeleteCoffeeAsync" />
        public async Task DeleteCoffeeAsync(Coffee coffee)
        {
            _unitOfWork.CoffeeRepository.DeleteCoffee(coffee);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc cref="ICoffeeService.GetCoffeeAsync" />
        public async Task<Coffee?> GetCoffeeAsync(Guid id)
        {
            return await _unitOfWork.CoffeeRepository.GetCoffeeAsync(id);
        }

        /// <inheritdoc cref="ICoffeeService.GetCoffeesParameters" />
        public ItemsParameters<Coffee> GetCoffeesParameters(string? filter, int currentNumberPage, int countItemsPage)
        {
            filter = filter?.Trim();

            return _unitOfWork.CoffeeRepository.GetCoffeesParameters(filter, currentNumberPage, countItemsPage);
        }
    }
}