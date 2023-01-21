namespace WebApi.ErrorMessages
{
    /// <summary>
    ///     Сообщения об ошибках в DTO кофе.
    /// </summary>
    public static class CoffeeErrorMessages
    {
        /// <summary>
        ///     Сообщение о пустом названии.
        /// </summary>
        public const string CoffeeNameRequired = "Название не может быть пустым.";

        /// <summary>
        ///     Сообщение о пустой цене.
        /// </summary>
        public const string CoffeePriceRequired = "Цена не может быть пустой.";

        /// <summary>
        ///     Сообщение о длине названия.
        /// </summary>
        public const string CoffeeNameLength = "Длина названия должна быть в диапазоне от 2 до 30 символов.";
    }
}