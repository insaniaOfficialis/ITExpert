using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Services.BD.Procedures.BaseExecute
{
    /// <summary>
    /// Базовый сервис вызова хранимых процедур
    /// </summary>
    public class BaseExecute : IBaseExecute
    {
        private readonly ILogger<BaseExecute> _logger; //интерфейс для записи логов
        private readonly IConfiguration _configuration; //интерфейс конфигурации

        /// <summary>
        /// Конструктор сервиса вызова хранимых процедур
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public BaseExecute(ILogger<BaseExecute> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Метод вызова хранимых процедур
        /// </summary>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string?> Execute(string name, string body)
        {
            try
            {
                //Формируем строку запроса
                if (String.IsNullOrEmpty(body))
                    body = "{}";
                string sql = "exec " + name + " '" + body + "'";

                //Открываем соединение, выполняем запрос и закрываем соединение
                using SqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                using SqlCommand command = new(sql, connection);
                connection.Open();
                var result = await command.ExecuteScalarAsync();
                connection.Close();

                return result?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("Execute. Ошибка: {0}" + ex);

                throw new Exception("Ошибка выполнения запроса", ex);
            }

        }
    }
}
