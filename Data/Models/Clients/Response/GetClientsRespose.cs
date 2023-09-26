using Data.Models.Base.Response;

namespace Data.Models.Clients.Response
{
    /// <summary>
    /// Модель ответа за списком клиентов
    /// </summary>
    public class GetClientsRespose : BaseResponse
    {
        /// <summary>
        /// Простой конструктор ответа за списком клиентов
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public GetClientsRespose(bool success, BaseError? error) : base(success, error)
        {

        }

        /// <summary>
        /// Конструктор модели ответа за списком клиентов
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        /// <param name="items"></param>
        public GetClientsRespose(bool success, BaseError? error, List<GetClientsResposeItem>? items) : base(success, error)
        {
            Items = items;
        }

        public List<GetClientsResposeItem>? Items { get; set; }
    }

    /// <summary>
    /// Элемент списка клиентов
    /// </summary>
    public class GetClientsResposeItem 
    {
        /// <summary>
        /// Конструктор элемента списка клиентов
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="countContacts"></param>
        public GetClientsResposeItem(string? clientName, int? countContacts)
        {
            ClientName = clientName;
            CountContacts = countContacts;
        }

        /// <summary>
        /// Наименование клиента
        /// </summary>
        public string? ClientName { get; set; }

        /// <summary>
        /// Количество контактов
        /// </summary>
        public int? CountContacts { get; set; }
    }
}
