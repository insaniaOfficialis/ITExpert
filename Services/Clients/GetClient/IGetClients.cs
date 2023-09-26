using Data.Models.Clients.Response;

namespace Services.Clients.GetClient
{
    /// <summary>
    /// Интерфейс получения клиентов
    /// </summary>
    public interface IGetClients
    {
        /// <summary>
        /// Метод получения клиентов
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<GetClientsRespose> Get(int? count);
    }
}
