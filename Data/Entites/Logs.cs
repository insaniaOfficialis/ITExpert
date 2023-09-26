using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entites
{
    /// <summary>
    /// Логи
    /// </summary>
    [Comment("Логи")]
    public class Logs
    {
        /// <summary>
        /// Пустой конструктор логов
        /// </summary>
        public Logs()
        {
            
        }

        /// <summary>
        /// Конструктор логов с наименованием и данными на вход
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataIn"></param>
        /// <param name="type"></param>
        public Logs(string name, string? dataIn, string type) : this()
        {
            Name = name;
            In = dataIn;
            Type = type;
            DateStart = DateTime.Now;
        }

        /// <summary>
        /// Первичный ключ
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Comment("Первичный ключ")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование вызываемого метода
        /// </summary>
        [Column("name")]
        [Comment("Наименование вызываемого метода")]
        public string Name { get; private set; }

        /// <summary>
        /// Тип вызываемого метода
        /// </summary>
        [Column("type")]
        [Comment("Тип вызываемого метода")]
        public string Type { get; private set; }

        /// <summary>
        /// Признак успешного выполнения
        /// </summary>
        [Column("success")]
        [Comment("Признак успешного выполнения")]
        public bool Success { get; private set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        [Column("dateStart")]
        [Comment("Дата начала")]
        public DateTime DateStart { get; private set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        [Column("dateEnd")]
        [Comment("Дата окончания")]
        public DateTime? DateEnd { get; private set; }

        /// <summary>
        /// Данные на вход
        /// </summary>
        [Column("in")]
        [Comment("Данные на вход")]
        public string? In { get; private set; }

        /// <summary>
        /// Данные на выход
        /// </summary>
        [Column("out")]
        [Comment("Данные на выход")]
        public string? Out { get; private set; }

        /// <summary>
        /// Метод записи завершения выполнения
        /// </summary>
        /// <param name="success"></param>
        /// <param name="dataOut"></param>
        public void SetEnd(bool success, string? dataOut)
        {
            Success = success;
            Out = dataOut;
            DateEnd = DateTime.Now;
        }
    }
}
