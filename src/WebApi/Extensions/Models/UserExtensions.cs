namespace WebApi.Extensions.Models
{
    using Db.Models;

    using Dtos.Users;

    /// <summary>
    ///     Расширения для пользователя.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        ///     Маппинг DTO нового или обновленного пользователя в модель.
        /// </summary>
        /// <param name="createdUserDto">DTO нового пользователя.</param>
        /// <returns>Модель пользователя.</returns>
        public static User ToModel(this CreateOrUpdateUserDto createdUserDto)
        {
            return new User
            {
                Login = createdUserDto.Login,
                Password = createdUserDto.Password,
                Name = createdUserDto.Name
            };
        }

        /// <summary>
        ///     Маппинг модели пользователя в DTO.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        /// <returns>DTO пользователя.</returns>
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Balance = user.Balance
            };
        }

        /// <summary>
        ///     Обновление модели пользователя на основании обновленной модели пользователя.
        /// </summary>
        /// <param name="existingUser">Существующий пользователь.</param>
        /// <param name="updatedUser">Модель обновленного пользователя.</param>
        public static void UpdateFromModel(this User existingUser, User updatedUser)
        {
            existingUser.Login = updatedUser.Login;
            existingUser.Password = updatedUser.Password;
            existingUser.Name = updatedUser.Name;
        }

        /// <summary>
        ///     Маппинг DTO пользователя в модель.
        /// </summary>
        /// <param name="userDto">DTO пользователя.</param>
        /// <returns>Модель пользователя.</returns>
        public static User ToModel(this UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Login = userDto.Login,
                Password = userDto.Password,
                Name = userDto.Name,
                Balance = userDto.Balance
            };
        }

        /// <summary>
        ///     Маппинг моделей перечисления пользователей в DTO.
        /// </summary>
        /// <param name="users">Перечисление моделей пользователей.</param>
        /// <returns>Перечисление DTO пользователей.</returns>
        public static IAsyncEnumerable<UserDto> ToDto(this IAsyncEnumerable<User> users)
        {
            return users.Select(user => user.ToDto());
        }
    }
}