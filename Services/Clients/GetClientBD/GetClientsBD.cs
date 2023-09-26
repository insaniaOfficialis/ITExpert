using Data.Models.Clients.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.BD.Procedures.BaseExecute;

namespace Services.Clients.GetClientBD
{
    /// <summary>
    /// Сервис получения клиентов
    /// </summary>
    public class GetClientsBD : IGetClientsBD
    {
        private readonly IBaseExecute _execute; //интерфейс вызова скалярных процедур
        private readonly ILogger<GetClientsBD> _logger; //интерфейс для записи логов

        /// <summary>
        /// Конструктор сервиса получения клиентов
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="logger"></param>
        public GetClientsBD(IBaseExecute execute, ILogger<GetClientsBD> logger)
        {
            _execute = execute;
            _logger = logger;
        }

        /// <summary>
        /// Метод получения клиентов
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<GetClientsRespose> Get(int? count)
        {
            try
            {
                var result = await _execute.Execute("get_clients", JsonConvert.SerializeObject(new { Count = count }));

                if (result == null)
                    return new GetClientsRespose(false, new(500, String.Format("Ошибка получения кодов")));

                var cliets = JsonConvert.DeserializeObject<List<GetClientsResposeItem>>(result);

                //Формируем ответ
                return new GetClientsRespose(true, null, cliets);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get. Ошибка: {0}" + ex);

                return new GetClientsRespose(false, new(500, String.Format("Ошибка получения кодов: {0}", ex)));
            }

        }
    }
}
