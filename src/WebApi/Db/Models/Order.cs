namespace WebApi.Db.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     Заказы.
    /// </summary>
    [Table("Orders")]
    public class Order
    {
        /// <summary>
        ///     Идентификатор заказа.
        /// </summary>
        [Column("id", TypeName = "UUID")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Кофе.
        /// </summary>
        [Column("coffee_id", TypeName = "UUID")]
        [Required]
        [ForeignKey("FK_Orders_Coffees")]
        public Coffee Coffee { get; set; }

        /// <summary>
        ///     Пользователь.
        /// </summary>
        [Column("user_id", TypeName = "UUID")]
        [ForeignKey("FK_Orders_Users")]
        public User? User { get; set; }
    }
}