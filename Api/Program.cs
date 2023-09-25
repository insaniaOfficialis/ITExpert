using Data;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            //Добавляем параметры для контекста базы данных
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            //Накатываем миграции
            await using var scope = app.Services.CreateAsyncScope();
            using var db = scope.ServiceProvider.GetService<ApplicationContext>();

            if(db != null)
                await db.Database.MigrateAsync();

            app.Run();
        }
    }
}