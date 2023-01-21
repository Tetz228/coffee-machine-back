namespace WebApi.Db.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     Статистика.
    /// </summary>
    [Table("Statistics")]
    public class Statistic
    {
        /// <summary>
        ///     Идентификатор статистики.
        /// </summary>
        [Column("id", TypeName = "UUID")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Кофе.
        /// </summary>
        [Column("coffee_id", TypeName = "UUID")]
        [Required]
        [ForeignKey("FK_Statistics_Coffees")]
        public Coffee Coffee { get; set; }

        /// <summary>
        ///     Общая сумма.
        /// </summary>
        [Column("total", TypeName = "MONEY")]
        [Required]
        public decimal Total { get; set; }
    }
}