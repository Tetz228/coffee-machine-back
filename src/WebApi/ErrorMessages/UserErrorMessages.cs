namespace WebApi.ErrorMessages
{
    /// <summary>
    ///     Сообщения об ошибках в DTO пользователе.
    /// </summary>
    public static class UserErrorMessages
    {
        /// <summary>
        ///     Сообщение о пустом логине.
        /// </summary>
        public const string UserLoginRequired = "Логин не может быть пустым.";

        /// <summary>
        ///     Сообщение о пустом пароле.
        /// </summary>
        public const string UserPasswordRequired = "Пароль не может быть пустым.";

        /// <summary>
        ///     Сообщение о пустом имени.
        /// </summary>
        public const string UserNameRequired = "Имя не может быть пустым.";

        /// <summary>
        ///     Сообщение о длине логина.
        /// </summary>
        public const string UserLoginLength = "Длина логина должна быть в диапазоне от 2 до 30 символов.";

        /// <summary>
        ///     Сообщение о длине пароля.
        /// </summary>
        public const string UserPasswordLength = "Длина пароля должна быть в диапазоне от 2 до 30 символов.";

        /// <summary>
        ///     Сообщение о длине имени.
        /// </summary>
        public const string UserNameLength = "Длина имени должна быть в диапазоне от 2 до 30 символов.";
    }
}