using Data;
using Data.Models.Base.Request;
using Data.Models.Codes.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.BD.Procedures.BaseExecute;
using CodesEntity = Data.Entites.Codes;

namespace Services.Codes.GetCodeBD
{
    /// <summary>
    /// Сервис получения кодов
    /// </summary>
    public class GetCodesBD : IGetCodesBD
    {
        private readonly IBaseExecute _execute; //интерфейс вызова скалярных процедур
        private readonly ILogger<GetCodesBD> _logger; //интерфейс для записи логов

        /// <summary>
        /// Конструктор сервиса получения кодов
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="logger"></param>
        public GetCodesBD(IBaseExecute execute, ILogger<GetCodesBD> logger)
        {
            _execute = execute;
            _logger = logger;
        }

        /// <summary>
        /// Метод получения кодов
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<GetCodesRespose> Get(BaseSortRequest? sort, int? skip, int? take, int? orderNumber, int? code, string? value)
        {
            try
            {
                var body = JsonConvert.SerializeObject(new { Sort = sort, Skip = skip, Take = take, OrderNumber = orderNumber, Code = code, Value = value });
                var result = await _execute.Execute("get_codes", body);

                if(result == null)
                    return new GetCodesRespose(false, new(500, String.Format("Ошибка получения кодов")));

                var codes = JsonConvert.DeserializeObject<List<CodesEntity>>(result);

                //Формируем ответ
                return new GetCodesRespose(true, null, codes);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get. Ошибка: {0}" + ex);

                return new GetCodesRespose(false, new(500, String.Format("Ошибка получения кодов: {0}", ex)));
            }

        }
    }
}
