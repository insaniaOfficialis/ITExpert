using Data;
using Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Client = Data.Entites.Clients;

namespace Services.Initialization
{
    /// <summary>
    /// Сервис инициализации
    /// </summary>
    public class Initialization : IInitialization
    {
        private readonly ApplicationContext _context; //контекст базы данных
        private readonly ILogger<Initialization> _logger; //интерфейс для записи логов

        /// <summary>
        /// Конструктор сервиса инициализации
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public Initialization(ApplicationContext context, ILogger<Initialization> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Метод инициализации базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitializeDatabase()
        {
            try
            {
                //Накатываем миграции
                await _context.Database.MigrateAsync();

                //Открываем транзакцию
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    //Даты клиентов
                    if (!_context.Dates.Any(x => x.Id == 1 && x.Dt == new DateTime(2021, 1, 1)))
                    {
                        Dates dates = new(1, new DateTime(2021, 1, 1));
                        _context.Dates.Add(dates);
                        await _context.SaveChangesAsync();
                    }
                    if (!_context.Dates.Any(x => x.Id == 1 && x.Dt == new DateTime(2021, 1, 10)))
                    {
                        Dates dates = new(1, new DateTime(2021, 1, 10));
                        _context.Dates.Add(dates);
                        await _context.SaveChangesAsync();
                    }
                    if (!_context.Dates.Any(x => x.Id == 1 && x.Dt == new DateTime(2021, 1, 30)))
                    {
                        Dates dates = new(1, new DateTime(2021, 1, 30));
                        _context.Dates.Add(dates);
                        await _context.SaveChangesAsync();
                    }
                    if (!_context.Dates.Any(x => x.Id == 2 && x.Dt == new DateTime(2021, 1, 15)))
                    {
                        Dates dates = new(2, new DateTime(2021, 1, 15));
                        _context.Dates.Add(dates);
                        await _context.SaveChangesAsync();
                    }
                    if (!_context.Dates.Any(x => x.Id == 2 && x.Dt == new DateTime(2021, 1, 30)))
                    {
                        Dates dates = new(2, new DateTime(2021, 1, 30));
                        _context.Dates.Add(dates);
                        await _context.SaveChangesAsync();
                    }

                    //Клиенты
                    if (!_context.Clients.Any(x => x.ClientName == "Иванов"))
                    {
                        Client clients = new("Иванов");
                        _context.Clients.Add(clients);
                        await _context.SaveChangesAsync();
                    }
                    if (!_context.Clients.Any(x => x.ClientName == "Петров"))
                    {
                        Client clients = new("Петров");
                        _context.Clients.Add(clients);
                        await _context.SaveChangesAsync();

                        //Контакты клиента
                        if(!_context.ClientContacts.Any(x => x.ClientId == clients.Id && x.ContactType == "Телефон" && x.ContactValue == "+79999999999"))
                        {
                            ClientContacts clientContacts = new(clients, "Телефон", "+79999999999");
                            _context.ClientContacts.Add(clientContacts);
                            await _context.SaveChangesAsync();
                        }
                    }
                    if (!_context.Clients.Any(x => x.ClientName == "Васильев"))
                    {
                        Client clients = new("Васильев");
                        _context.Clients.Add(clients);
                        await _context.SaveChangesAsync();

                        //Контакты клиента
                        if (!_context.ClientContacts.Any(x => x.ClientId == clients.Id && x.ContactType == "Телефон" && x.ContactValue == "+71111111111"))
                        {
                            ClientContacts clientContacts = new(clients, "Телефон", "+71111111111");
                            _context.ClientContacts.Add(clientContacts);
                            await _context.SaveChangesAsync();
                        }
                        if (!_context.ClientContacts.Any(x => x.ClientId == clients.Id && x.ContactType == "Почта" && x.ContactValue == "a@a.a"))
                        {
                            ClientContacts clientContacts = new(clients, "Почта", "a@a.a");
                            _context.ClientContacts.Add(clientContacts);
                            await _context.SaveChangesAsync();
                        }
                    }
                    if (!_context.Clients.Any(x => x.ClientName == "Сидоров"))
                    {
                        Client clients = new("Сидоров");
                        _context.Clients.Add(clients);
                        await _context.SaveChangesAsync();

                        //Контакты клиента
                        if (!_context.ClientContacts.Any(x => x.ClientId == clients.Id && x.ContactType == "Телефон" && x.ContactValue == "+72222222222"))
                        {
                            ClientContacts clientContacts = new(clients, "Телефон", "+72222222222");
                            _context.ClientContacts.Add(clientContacts);
                            await _context.SaveChangesAsync();
                        }
                        if (!_context.ClientContacts.Any(x => x.ClientId == clients.Id && x.ContactType == "Почта" && x.ContactValue == "b@b.b"))
                        {
                            ClientContacts clientContacts = new(clients, "Почта", "b@b.b");
                            _context.ClientContacts.Add(clientContacts);
                            await _context.SaveChangesAsync();
                        }
                        if (!_context.ClientContacts.Any(x => x.ClientId == clients.Id && x.ContactType == "Адрес" && x.ContactValue == "Москва, Красная, 1"))
                        {
                            ClientContacts clientContacts = new(clients, "Адрес", "Москва, Красная, 1");
                            _context.ClientContacts.Add(clientContacts);
                            await _context.SaveChangesAsync();
                        }
                    }

                    //Создаём процедуру получения кодов
                    string? sql = "CREATE OR ALTER PROCEDURE dbo.get_codes (@json_in NVARCHAR(MAX))\r\nAS\r\nBEGIN\r\n\r\n  BEGIN TRY\r\n    DECLARE @sortKey NVARCHAR(MAX)\r\n           ,@isAscending BIT\r\n           ;\r\n    \r\n    SELECT @sortKey = j.sortKey\r\n          ,@isAscending = j.isAscending\r\n    FROM OPENJSON(@json_in, '$')\r\n    WITH (\r\n            SortKey NVARCHAR(MAX)\r\n           ,IsAscending BIT\r\n          ) j;\r\n\r\n    DECLARE @json_out NVARCHAR(MAX) = (\r\n                                       SELECT c.order_number 'orderNumber'\r\n                                             ,c.code\r\n                                             ,c.[value]\r\n                                       FROM Codes c\r\n                                       ORDER BY CASE WHEN @sortKey = 'orderNumber' AND @isAscending = 1 THEN c.order_number END\r\n                                               ,CASE WHEN @sortKey = 'code' AND @isAscending = 1 THEN code END\r\n                                               ,CASE WHEN @sortKey = 'value' AND @isAscending = 1 THEN [value] END\r\n                                               ,CASE WHEN @sortKey = 'orderNumber' AND @isAscending = 0 THEN order_number END DESC\r\n                                               ,CASE WHEN @sortKey = 'code' AND @isAscending = 0 THEN code END DESC\r\n                                               ,CASE WHEN @sortKey = 'value' AND @isAscending = 0 THEN [value] END DESC\r\n                                       FOR JSON PATH\r\n                                      );\r\n\r\n    SELECT @json_out;\r\n    \r\n  END TRY\r\n  BEGIN CATCH\r\n    IF @@trancount > 0\r\n\t\t  ROLLBACK;\r\n\t\tTHROW;\r\n  END CATCH;\r\n\r\nEND;";
                    _context.Database.ExecuteSqlRaw(sql);
                    await _context.SaveChangesAsync();
                    sql = null;

                    //Создаём процедуру получения клиентов
                    sql = "CREATE OR ALTER PROCEDURE dbo.get_clients (@json_in NVARCHAR(MAX))\r\nAS\r\nBEGIN\r\n\r\n  BEGIN TRY\r\n    DECLARE @count INT\r\n           ;\r\n    \r\n    SELECT @count = j.[Count]\r\n    FROM OPENJSON(@json_in, '$')\r\n    WITH (\r\n            [Count] INT\r\n          ) j;\r\n\r\n    DECLARE @json_out NVARCHAR(MAX) = (\r\n                                       SELECT c.ClientName\r\n                                             ,COUNT(cc.Id) 'CountContacts'\r\n                                       FROM Clients c\r\n                                       LEFT JOIN ClientContacts cc ON c.Id = cc.ClientId\r\n                                       GROUP BY c.ClientName\r\n                                       HAVING COUNT(cc.Id) > ISNULL(@count, 0)\r\n                                       FOR JSON PATH\r\n                                      );\r\n\r\n    SELECT @json_out;\r\n    \r\n  END TRY\r\n  BEGIN CATCH\r\n    IF @@trancount > 0\r\n\t\t  ROLLBACK;\r\n\t\tTHROW;\r\n  END CATCH;\r\n\r\nEND;";
                    _context.Database.ExecuteSqlRaw(sql);
                    await _context.SaveChangesAsync();
                    sql = null;

                    //Создаём процедуру интервалов клиентов
                    sql = "CREATE OR ALTER FUNCTION dbo.get_dates ()\r\nRETURNS @table TABLE (Id BIGINT, Sd DATE, Ed DATE)\r\nAS\r\nBEGIN\r\n\r\n  INSERT @table\r\n  SELECT dt.Id\r\n        ,dt.Dt 'Sd'\r\n        ,di.Dt 'Ed'\r\n  FROM (\r\n          SELECT dt.Id, dt.Dt, ROW_NUMBER() OVER(PARTITION BY dt.Id ORDER BY dt.Dt) 'Number' \r\n          FROM Dates dt\r\n       ) dt\r\n  JOIN (\r\n          SELECT dt.Id, dt.Dt, ROW_NUMBER() OVER(PARTITION BY dt.Id ORDER BY dt.Dt) 'Number' \r\n          FROM Dates dt\r\n       ) di ON di.Id = dt.Id\r\n  WHERE di.Number = dt.Number + 1\r\n  ORDER BY dt.Id, dt.Dt, di.Dt\r\n\r\n  RETURN;\r\n\r\nEND;";
                    _context.Database.ExecuteSqlRaw(sql);
                    await _context.SaveChangesAsync();
                    sql = null;

                    //Фиксируем транзакцию
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError("InitializeDatabase. Ошибка: {0}" + ex);
                    await transaction.RollbackAsync();
                    throw new Exception("Ошибка инициализации базы данных", ex);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("InitializeDatabase. Ошибка: {0}" + ex);

                return false;
            }
        }
    }
}