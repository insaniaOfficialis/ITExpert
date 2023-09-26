using Data;
using Data.Models.Base.Request;
using Data.Models.Base.Response;
using Data.Models.Codes.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using CodesEntity = Data.Entites.Codes;

namespace Services.Codes.GetCode
{
    /// <summary>
    /// Сервис получения кодов
    /// </summary>
    public class GetCodes : IGetCodes
    {
        private readonly ApplicationContext _context; //контекст базы данных
        private readonly ILogger<GetCodes> _logger; //интерфейс для записи логов

        /// <summary>
        /// Конструктор сервиса получения кодов
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public GetCodes(ApplicationContext context, ILogger<GetCodes> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Метод получения кодов
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<GetCodesRespose> Get(List<BaseSortRequest?>? sort, int? skip, int? take, int? orderNumber, int? code, string? value)
        {
            try
            {
                //Строим запрос
                IQueryable<CodesEntity> codesQuery = _context.Codes;

                //Если передали поле сортировки
                if (sort?.Any() == true)
                {
                    //Сортируем по первому элементу сортировки
                    IOrderedQueryable<CodesEntity> codesOrderQuery = (sort.FirstOrDefault()!.SortKey, sort.FirstOrDefault()!.IsAscending) switch
                    {
                        ("code", true) => codesQuery.OrderBy(x => x.Code),
                        ("orderNumber", true) => codesQuery.OrderBy(x => x.OrderNumber),
                        ("value", true) => codesQuery.OrderBy(x => x.Value),
                        ("code", false) => codesQuery.OrderByDescending(x => x.Code),
                        ("orderNumber", false) => codesQuery.OrderByDescending(x => x.OrderNumber),
                        ("value", false) => codesQuery.OrderByDescending(x => x.Value),
                        _ => codesQuery.OrderBy(x => x.OrderNumber),
                    };

                    //Если есть ещё поля для сортировки
                    if (sort.Count > 1)
                    {
                        //Проходим по всем элементам сортировки кроме первой
                        foreach (var sortElement in sort.Skip(1))
                        {
                            //Сортируем по каждому элементу
                            codesOrderQuery = (sortElement!.SortKey, sortElement!.IsAscending) switch
                            {
                                ("code", true) => codesOrderQuery.ThenBy(x => x.Code),
                                ("orderNumber", true) => codesOrderQuery.ThenBy(x => x.OrderNumber),
                                ("value", true) => codesOrderQuery.ThenBy(x => x.Value),
                                ("code", false) => codesOrderQuery.ThenByDescending(x => x.Code),
                                ("orderNumber", false) => codesOrderQuery.ThenByDescending(x => x.OrderNumber),
                                ("value", false) => codesOrderQuery.ThenByDescending(x => x.Value),
                                _ => codesOrderQuery.ThenBy(x => x.Value),
                            };
                        }
                    }

                    //Приводим в список отсортированный список
                    codesQuery = codesOrderQuery;
                }

                //Если передали фильтры
                if (orderNumber != null)
                    codesQuery = codesQuery.Where(x => x.OrderNumber == orderNumber);
                if (code != null)
                    codesQuery = codesQuery.Where(x => x.Code == code);
                if (!String.IsNullOrEmpty(value))
                    codesQuery = codesQuery.Where(x => !String.IsNullOrEmpty(x.Value) && x.Value.ToLower().Contains(value.ToLower()));

                //Если передали параметры пагинации
                if (skip != null)
                    codesQuery = codesQuery.Skip(skip ?? 0);
                if (take != null)
                    codesQuery = codesQuery.Take(take ?? 10);

                //Получаем коды с базы
                var codesDb = await codesQuery.ToListAsync();

                //Формируем ответ
                return new GetCodesRespose(true, null, codesDb);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCodes. Ошибка: {0}" + ex);

                return new GetCodesRespose(false, new(400, String.Format("Ошибка получения кодов", ex)));
            }

        }
    }
}
