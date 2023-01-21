namespace WebApi.Dtos.Users
{
    using System.ComponentModel.DataAnnotations;

    using ErrorMessages;

    /// <summary>
    ///     DTO добавленного или обновленного пользователя.
    /// </summary>
    public record CreateOrUpdateUserDto
    {
        /// <summary>
        ///     Логин пользователя.
        /// </summary>
        [Required(ErrorMessage = UserErrorMessages.UserLoginRequired)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = UserErrorMessages.UserLoginLength)]
        public string Login { get; init; }

        /// <summary>
        ///     Пароль пользователя.
        /// </summary>
        [Required(ErrorMessage = UserErrorMessages.UserPasswordRequired)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = UserErrorMessages.UserPasswordLength)]
        public string Password { get; init; }

        /// <summary>
        ///     Имя пользователя.
        /// </summary>
        [Required(ErrorMessage = UserErrorMessages.UserNameRequired)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = UserErrorMessages.UserNameLength)]
        public string Name { get; init; }
    }
}