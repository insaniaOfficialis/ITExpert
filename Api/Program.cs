using Api.Middleware;
using Data;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.BD.Procedures.BaseExecute;
using Services.Clients.GetClient;
using Services.Clients.GetClientBD;
using Services.Codes.AddCode;
using Services.Codes.GetCode;
using Services.Codes.GetCodeBD;
using Services.Initialization;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddControllersWithViews();

            //ƒобавл€ем параметры логировани€
            Log.Logger = new LoggerConfiguration()
                           .MinimumLevel.Verbose()
                           .WriteTo.File(path: config["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
                           .WriteTo.Debug()
                           .CreateLogger();

            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

            //ƒобавл€ем параметры дл€ контекста базы данных
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            //¬недр€ем зависимости дл€ сервисов
            builder.Services.AddScoped<IInitialization, Initialization>();
            builder.Services.AddScoped<IBaseExecute, BaseExecute>();
            builder.Services.AddScoped<IAddCodes, AddCodes>();
            builder.Services.AddScoped<IGetCodes, GetCodes>();
            builder.Services.AddScoped<IGetCodesBD, GetCodesBD>();
            builder.Services.AddScoped<IGetClients, GetClients>();
            builder.Services.AddScoped<IGetClientsBD, GetClientsBD>();

            //ƒобавл€ем параметры сериализации и десериализации json
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = false;
                options.SerializerOptions.PropertyNamingPolicy = null;
                options.SerializerOptions.WriteIndented = true;
            });

            var app = builder.Build();

            app.UseMiddleware<LoggingMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=CodeController}/{action=Add}");

            app.MapGet("/", () => "Hello World!");

            //ѕроводим первоначальную инициализацию
            using var scope = app.Services.CreateScope();
            var initialize = scope.ServiceProvider.GetService<IInitialization>();
            var success = await initialize!.InitializeDatabase();

            app.Run();
        }
    }
}