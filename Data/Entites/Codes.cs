using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entites
{
    /// <summary>
    /// Коды
    /// </summary>
    [Comment("Коды")]
    public class Codes
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("order_number")]
        [Comment("Порядковый номер")]
        public long OrderNumber { get; private set; }

        /// <summary>
        /// Код
        /// </summary>
        [Column("code")]
        [Comment("Код")]
        public int Code { get; private set; }

        /// <summary>
        /// Значение
        /// </summary>
        [Column("value")]
        [Comment("Значение кода")]
        public string? Value { get; private set; }

        /// <summary>
        /// Пустой конструктор сущности кодов
        /// </summary>
        public Codes()
        {

        }

        /// <summary>
        /// Конструктор сущности кодов с кодом
        /// </summary>
        /// <param name="code"></param>
        public Codes(int code) : this()
        {
            Code = code;
        }

        /// <summary>
        /// Конструктор сущности кодов с кодом и значением
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        public Codes(int code, string value) : this(code)
        {
            Value = value;
        }
    }
}
