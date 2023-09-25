using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entites
{
    /// <summary>
    /// Контакты клиента
    /// </summary>
    [Comment("Контакты клиента")]
    public class ClientContacts
    {
        /// <summary>
        /// Id контакта
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Id контакта")]
        public long Id { get; private set; }

        /// <summary>
        /// Id клиента
        /// </summary>
        [Comment("Id клиента")]
        public long? ClientId { get; private set; }

        /// <summary>
        /// Навигационное свойство клиента
        /// </summary>
        public Clients? Client { get; private set; }

        /// <summary>
        /// Тип контакта
        /// </summary>
        [MaxLength(255)]
        [Comment("Тип контакта")]
        public string? ContactType { get; private set; }

        /// <summary>
        /// Значение контакта
        /// </summary>
        [MaxLength(255)]
        [Comment("Значение контакта")]
        public string? ContactValue { get; private set; }

        /// <summary>
        /// Пустой конструктор контактов клиентов
        /// </summary>
        public ClientContacts()
        {

        }

        /// <summary>
        /// Конструктор контактов клиентов с клиентом
        /// </summary>
        /// <param name="client"></param>
        public ClientContacts(Clients client) : this()
        {
            Client = client;
            ClientId = client.Id;
        }

        /// <summary>
        /// Конструктор контактов клиентов с клиентом и типом контакта
        /// </summary>
        /// <param name="client"></param>
        /// <param name="type"></param>
        public ClientContacts(Clients client, string? type) : this(client)
        {
            ContactType = type;
        }

        /// <summary>
        /// Конструктор контактов клиентов с клиентом, типом и значением контакта
        /// </summary>
        /// <param name="client"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public ClientContacts(Clients client, string? type, string value) : this(client, type)
        {
            ContactValue = value;
        }
    }
}
