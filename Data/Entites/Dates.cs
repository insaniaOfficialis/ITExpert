using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entites
{
    /// <summary>
    /// Даты клиента
    /// </summary>
    [Comment("Даты клиента")]
    public class Dates
    {
        /// <summary>
        /// Id клиента
        /// </summary>
        [Comment("Id клиента")]
        public long Id { get; private set; }

        /// <summary>
        /// Дата клиента
        /// </summary>
        [Comment("Дата клиента")]
        public DateTime Dt { get; private set; }

        /// <summary>
        /// Пустой конструктор дат клиентов
        /// </summary>
        public Dates()
        {

        }

        /// <summary>
        /// Конструктор дат клиентов с id
        /// </summary>
        /// <param name="id"></param>
        public Dates(long id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// Конструктор дат клиентов с id и датой
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        public Dates(long id, DateTime date) : this(id)
        {
            Dt = date;
        }
    }
}
