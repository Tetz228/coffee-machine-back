namespace WebApi.Repositories.Interfaces
{
    using Db.Models;

    /// <summary>
    ///     Репозиторий для взаимодействия с пользователем.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        ///     Добавление нового пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        public Task AddUserAsync(User user);

        /// <summary>
        ///     Обновление пользователя.
        /// </summary>
        /// <param name="existingUser">Существующий пользователь.</param>
        public void UpdateUser(User existingUser);

        /// <summary>
        ///     Удаление пользователя.
        /// </summary>
        /// <param name="existingUser">Существующий пользователь.</param>
        public void DeleteUser(User existingUser);

        /// <summary>
        ///     Получение пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Задача с моделью пользователя.</returns>
        public Task<User?> GetUserAsync(Guid id);

        /// <summary>
        ///     Получение пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Задача с моделью пользователя.</returns>
        public Task<User?> GetUserAsync(string login);

        /// <summary>
        ///     Получение всех пользователей.
        /// </summary>
        /// <returns>Перечисление пользователей.</returns>
        public IAsyncEnumerable<User> GetAllUsers();
    }
}