using Data.Models.Clients.Response;

namespace Services.Clients.GetClientBD
{
    /// <summary>
    /// Интерфейс получения клиентов
    /// </summary>
    public interface IGetClientsBD
    {
        /// <summary>
        /// Метод получения клиентов
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<GetClientsRespose> Get(int? count);
    }
}
