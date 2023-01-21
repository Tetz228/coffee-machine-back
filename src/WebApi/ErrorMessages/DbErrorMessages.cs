namespace WebApi.ErrorMessages
{
    /// <summary>
    ///     Сообщения об ошибках в базе данных.
    /// </summary>
    public static class DbErrorMessages
    {
        /// <summary>
        ///     Сообщение о неудачном поиске кофе.
        /// </summary>
        public const string FoundCoffee = "Не удалось получить кофе для создания статистики.";

        /// <summary>
        ///     Сообщение о некорректных данных.
        /// </summary>
        public const string IncorrectData = "Были переданы некорректные данные.";

        /// <summary>
        ///     Сообщение о неудачном создании сущности.
        /// </summary>
        public const string CreateData = "Не удалось создать сущность.";

        /// <summary>
        ///     Сообщение о неудачном поиске сущности.
        /// </summary>
        public const string FoundData = "Не удалось найти сущность с заданным идентификатором.";

        /// <summary>
        ///     Сообщение о неудачном обновлении сущности.
        /// </summary>
        public const string UpdateData = "Не удалось обновить сущность.";
    }
}