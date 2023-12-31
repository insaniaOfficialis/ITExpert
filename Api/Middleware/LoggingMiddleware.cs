﻿using Data;
using Data.Entites;
using System.Text;

namespace Api.Middleware
{
    /// <summary>
    /// Сервис логгирования middleware
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger; //интерфейс для записи логов
        private readonly static List<string> _successCodes = new() { "200", "204" }; //успешные статус кодов

        /// <summary>
        /// Конструктор сервиса логгирования middleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Метод перехватывания запросов
        /// </summary>
        /// <param name="context"></param>
        /// <param name="applicationContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ApplicationContext applicationContext)
        {
            //Получаем параметры запроса
            var path = context.Request.Path; //адрес запроса
            var type = context.Request.Method; //тип запроса
            var request = await GetRequest(context.Request); //тело и query параметры запроса

            //Записываем в базу о начале выполнения
            Logs log = new(path, request, type);
            applicationContext.Logs.Add(log);
            await applicationContext.SaveChangesAsync();

            //Получаем оригинальный поток ответа
            var originalBodyStream = context.Response.Body;

            //Перехватываем тело ответа
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            await _next(context);
            var response = await GetResponse(context.Response);

            //Определяем успешность ответа
            var success = _successCodes.Any(x => x == context.Response.StatusCode.ToString());

            //Записываем реузльтат выполнения в лог
            log.SetEnd(success, response);
            applicationContext.Logs.Update(log);
            await applicationContext.SaveChangesAsync();

            //Возвращаем в ответ оригинальный поток
            await responseBody.CopyToAsync(originalBodyStream);
        }

        /// <summary>
        /// Метод получения запроса
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<string> GetRequest(HttpRequest request)
        {
            //Включаем возможност прочитать тело запроса несколько раз
            request.EnableBuffering();

            //Устанавливаем поток в начало
            request.Body.Position = 0;

            //Считываем поток
            var reader = new StreamReader(request.Body);
            var bodyString = await reader.ReadToEndAsync();

            //Снова устанавливаем поток в начало
            request.Body.Position = 0;

            //Возвращаем результат
            return string.Format("QueryString: {0}, Body: {1}", request.QueryString, bodyString);
        }

        /// <summary>
        /// Метод получения ответа
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> GetResponse(HttpResponse response)
        {
            //Устанавлвиаем поток в начало
            response.Body.Seek(0, SeekOrigin.Begin);

            //Считываем поток
            string bodyString = await new StreamReader(response.Body).ReadToEndAsync();

            //Снова устанавливаем поток в начало
            response.Body.Seek(0, SeekOrigin.Begin);
            
            //Возвращаем результат
            return string.Format("Body: {0}", bodyString);
        }
    }
}
