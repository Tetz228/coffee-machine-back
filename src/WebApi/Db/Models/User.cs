namespace WebApi.Db.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     Пользователь.
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>
        ///     Идентификатор пользователя.
        /// </summary>
        [Column("id", TypeName = "UUID")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Логин пользователя.
        /// </summary>
        [Column("login", TypeName = "TEXT")]
        [Required]
        [MaxLength(30)]
        public string Login { get; set; }

        /// <summary>
        ///     Пароль пользователя.
        /// </summary>
        [Column("password", TypeName = "TEXT")]
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }

        /// <summary>
        ///     Имя пользователя.
        /// </summary>
        [Column("name", TypeName = "TEXT")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        ///     Баланс пользователя.
        /// </summary>
        [Column("balance", TypeName = "MONEY")]
        public decimal Balance { get; set; }

        /// <summary>
        ///     Токен обновления пользователя.
        /// </summary>
        [Column("refresh_token", TypeName = "TEXT")]
        public string? RefreshToken { get; set; }

        /// <summary>
        ///     Срок действия токена обновления пользователя.
        /// </summary>
        [Column("refresh_token_expiry_time", TypeName = "DATE")]
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}