namespace WebApi.ErrorMessages
{
    /// <summary>
    ///     Сообщения об ошибках в DTO статистике.
    /// </summary>
    public static class StatisticErrorMessages
    {
        /// <summary>
        ///     Сообщение о пустом DTO кофе.
        /// </summary>
        public const string StatisticCoffeeRequired = "Кофе не может быть пустым.";

        /// <summary>
        ///     Сообщение о пустой общей сумме.
        /// </summary>
        public const string StatisticTotalRequired = "Общая сумма не может быть пустой.";
    }
}