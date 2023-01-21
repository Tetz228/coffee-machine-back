namespace WebApi.Services.Interfaces
{
    using Db.Models;

    using Enums;

    /// <summary>
    ///     Сервис для работы со сдачей.
    /// </summary>
    public interface IChangeService
    {
        /// <summary>
        ///     Расчет сдачи.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        /// <returns>Задача со сдачей.</returns>
        public Task<Dictionary<Bills, int>> CalculateChange(User user);
    }
}