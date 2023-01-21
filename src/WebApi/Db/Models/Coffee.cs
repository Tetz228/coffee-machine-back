namespace WebApi.Db.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     Кофе.
    /// </summary>
    [Table("Coffees")]
    public class Coffee
    {
        /// <summary>
        ///     Идентификатор кофе.
        /// </summary>
        [Column("id", TypeName = "UUID")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Название кофе.
        /// </summary>
        [Column("name", TypeName = "TEXT")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        ///     Цена за кофе.
        /// </summary>
        [Column("price", TypeName = "MONEY")]
        [Required]
        public decimal Price { get; set; }
    }
}