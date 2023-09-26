using Data;
using Data.Models.Base.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using CodesEnity = Data.Entites.Codes;

namespace Services.Codes.AddCode
{
    /// <summary>
    /// Сервис добавления кодов
    /// </summary>
    public class AddCodes : IAddCodes
    {
        private readonly ApplicationContext _context; //контекст базы данных
        private readonly ILogger<AddCodes> _logger; //интерфейс для записи логов

        /// <summary>
        /// Конструктор сервиса добавления кодов
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public AddCodes(ApplicationContext context, ILogger<AddCodes> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Метод добавления кодов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse> AddCode(JsonArray? request)
        {
            try
            {
                if (request == null)
                    return new BaseResponse(false, new(400, "Пустой запрос"));

                //Десериализуем 
                var codes = (JsonConvert.DeserializeObject<List<Dictionary<int, string>>>(request.ToString()));
                
                if (codes == null || codes.Count == 0)
                    return new BaseResponse(false, new(400, "Не удалось десериализовать запрос"));

                //Формируем коллекцию кодов
                List<CodesEnity> codesEntity = codes
                    .Select(x => new CodesEnity(x.Keys.FirstOrDefault(), x.Values.FirstOrDefault() ?? String.Empty))
                    .OrderBy(x => x.Code)
                    .ToList();
                //Открываем транзакцию
                using var transaction = _context.Database.BeginTransaction();

                try
                {
                    //Очищаем таблицу
                    string? sql = "TRUNCATE TABLE Codes";
                    _context.Database.ExecuteSqlRaw(sql);
                    await _context.SaveChangesAsync();
                    sql = null;

                    //Обнуляем автоинкермент таблицы
                    sql = "DBCC CHECKIDENT('Codes', RESEED, 1)";
                    _context.Database.ExecuteSqlRaw(sql);
                    await _context.SaveChangesAsync();
                    sql = null;

                    //Добавляем коды в базу
                    await _context.Codes.AddRangeAsync(codesEntity);
                    await _context.SaveChangesAsync();

                    //Фиксируем транзакцию
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Ошибка добавления кодов в базу", ex);
                }

                return new BaseResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddCode. Ошибка: {0}" + ex);

                return new BaseResponse(false, new(400, String.Format("Ошибка добавления кодов", ex)));
            }
        }
    }
}
