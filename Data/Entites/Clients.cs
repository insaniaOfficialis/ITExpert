using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entites
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [Comment("Клиенты")]
    public class Clients
    {
        /// <summary>
        /// Id клиента
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Id клиента")]
        public long Id { get; private set; }

        /// <summary>
        /// Наименование клиента
        /// </summary>
        [MaxLength(200)]
        [Comment("Наименование клиента")]
        public string? ClientName { get; private set; }

        /// <summary>
        /// Пустой конструктор клиентов
        /// </summary>
        public Clients()
        {
            
        }

        /// <summary>
        /// Конструктор клиентов с наименованием
        /// </summary>
        /// <param name="clientName"></param>
        public Clients(string clientName) : this()
        {
            ClientName = clientName;
        }
    }
}
