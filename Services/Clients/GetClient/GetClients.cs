using Data.Models.Codes.Response;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Models.Clients.Response;
using Data.Entites;

namespace Services.Clients.GetClient
{
    /// <summary>
    /// Сервис получения клиентов
    /// </summary>
    public class GetClients : IGetClients
    {
        private readonly ApplicationContext _context; //контекст базы данных
        private readonly ILogger<GetClients> _logger; //интерфейс для записи логов

        /// <summary>
        /// Конструктор сервиса получения клиентов
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public GetClients(ApplicationContext context, ILogger<GetClients> logger)
        {
            _context = context;
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
                //Строим запрос
                var codesQuery = _context
                    .ClientContacts
                    .Include(x => x.Client)
                    .GroupBy(x => x.Client!.ClientName);

                //Если передали поле выбора количества
                if(count != null)
                    codesQuery = codesQuery.Where(x => x.Count() > count);

                //Получаем клиентов с базы
                var codesBd = await codesQuery.Select(x => new GetClientsResposeItem(x.Key, x.Count())).ToListAsync();

                //Формируем ответ
                return new GetClientsRespose(true, null, codesBd);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCodes. Ошибка: {0}" + ex);

                return new GetClientsRespose(false, new(400, String.Format("Ошибка получения кодов", ex)));
            }

        }
    }
}
