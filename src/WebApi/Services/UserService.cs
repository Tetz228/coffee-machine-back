namespace WebApi.Services
{
    using BCrypt.Net;

    using Db.Models;

    using Enums;

    using Extensions.Models;

    using Interfaces;

    using UoW.Interfaces;

    /// <inheritdoc cref="IUserService" />
    public class UserService : IUserService
    {
        /// <inheritdoc cref="IUnitOfWork" />
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IUserService" />
        /// <param name="unitOfWork">Единица работы.</param>
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc cref="IUserService.CreateUserAsync" />
        public async Task<User?> CreateUserAsync(User creatingUser)
        {
            creatingUser.Password = BCrypt.HashPassword(creatingUser.Password);

            await _unitOfWork.UserRepository.AddUserAsync(creatingUser);

            await _unitOfWork.SaveChangesAsync();

            return creatingUser;
        }

        /// <inheritdoc cref="IUserService.UpdateUserAsync" />
        public async Task<User?> UpdateUserAsync(User user, User updatingUser)
        {
            user.UpdateFromModel(updatingUser);

            _unitOfWork.UserRepository.UpdateUser(user);

            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        /// <inheritdoc cref="IUserService.UpdateBalanceUserAsync" />
        public async Task UpdateBalanceUserAsync(User user, decimal sum, bool isReplenishment)
        {
            if (isReplenishment)
            {
                user.Balance += sum;
            }
            else
            {
                user.Balance -= sum;
            }

            _unitOfWork.UserRepository.UpdateUser(user);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc cref="IUserService.CheckReplenishmentSumForBills" />
        public bool CheckReplenishmentSumForBills(Bills replenishmentSum)
        {
            var bills = Enum.GetValues<Bills>();

            return bills.Any(bill => replenishmentSum == bill);
        }

        /// <inheritdoc cref="IUserService.DeleteUserAsync" />
        public async Task DeleteUserAsync(User user)
        {
            _unitOfWork.UserRepository.DeleteUser(user);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc cref="IUserService.GetUserAsync(Guid)" />
        public async Task<User?> GetUserAsync(Guid id)
        {
            return await _unitOfWork.UserRepository.GetUserAsync(id);
        }

        /// <inheritdoc cref="IUserService.IsLoginUserUnique" />
        public async Task<bool> IsLoginUserUnique(string login)
        {
            var foundUser = await _unitOfWork.UserRepository.GetUserAsync(login);

            return foundUser is null;
        }

        /// <inheritdoc cref="IUserService.GetAllUsers" />
        public IAsyncEnumerable<User> GetAllUsers()
        {
            return _unitOfWork.UserRepository.GetAllUsers();
        }
    }
}