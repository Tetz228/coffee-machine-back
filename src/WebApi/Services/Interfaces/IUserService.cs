namespace WebApi.Services.Interfaces
{
    using Db.Models;

    using Enums;

    /// <summary>
    ///     Сервис для работы с пользователями.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     Создание нового пользователя.
        /// </summary>
        /// <param name="creatingUser">Модель нового пользователя.</param>
        /// <returns>Задача с моделью созданного пользователя.</returns>
        public Task<User?> CreateUserAsync(User creatingUser);

        /// <summary>
        ///     Обновление пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        /// <param name="updatingUser">Модель обновленного пользователя.</param>
        /// <returns>Задача с моделью обновленного пользователя.</returns>
        public Task<User?> UpdateUserAsync(User user, User updatingUser);

        /// <summary>
        ///     Обновление баланса пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        /// <param name="sum">Сумма пополнения или списания.</param>
        /// <param name="isReplenishment">True - если пополнение, иначе - false.</param>
        public Task UpdateBalanceUserAsync(User user, decimal sum, bool isReplenishment);

        /// <summary>
        ///     Проверка суммы пополнения на соответствие купюрам.
        /// </summary>
        /// <param name="replenishmentSum">Сумма пополнения.</param>
        /// <returns>True - если сумма пополнения соответствует купюрам, иначе - false.</returns>
        public bool CheckReplenishmentSumForBills(Bills replenishmentSum);

        /// <summary>
        ///     Удаление пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        public Task DeleteUserAsync(User user);

        /// <summary>
        ///     Получение пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Задача с моделью пользователя.</returns>
        public Task<User?> GetUserAsync(Guid id);

        /// <summary>
        ///     Является ли логин пользователя уникальным.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Задача с true - если логин уникален, иначе - false.</returns>
        public Task<bool> IsLoginUserUnique(string login);

        /// <summary>
        ///     Получение всех пользователей.
        /// </summary>
        /// <returns>Перечисление пользователей.</returns>
        public IAsyncEnumerable<User> GetAllUsers();
    }
}