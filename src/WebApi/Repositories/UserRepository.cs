namespace WebApi.Repositories
{
    using Db;
    using Db.Models;

    using Interfaces;

    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc cref="IUserRepository" />
    public class UserRepository : IUserRepository
    {
        /// <inheritdoc cref="CoffeeMachineContext" />
        private readonly CoffeeMachineContext _context;

        /// <inheritdoc cref="IUserRepository" />
        /// <param name="context">Контекст базы данных.</param>
        public UserRepository(CoffeeMachineContext context)
        {
            _context = context;
        }

        /// <inheritdoc cref="IUserRepository.AddUserAsync" />
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        /// <inheritdoc cref="IUserRepository.UpdateUser" />
        public void UpdateUser(User existingUser)
        {
            _context.Users.Update(existingUser);
        }

        /// <inheritdoc cref="IUserRepository.DeleteUser" />
        public void DeleteUser(User existingUser)
        {
            _context.Users.Remove(existingUser);
        }

        /// <inheritdoc cref="IUserRepository.GetUserAsync(Guid)" />
        public async Task<User?> GetUserAsync(Guid id)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Id == id);
        }

        /// <inheritdoc cref="IUserRepository.GetUserAsync(string)" />
        public async Task<User?> GetUserAsync(string login)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Login == login);
        }

        /// <inheritdoc cref="IUserRepository.GetAllUsers" />
        public IAsyncEnumerable<User> GetAllUsers()
        {
            return _context.Users.AsAsyncEnumerable();
        }
    }
}