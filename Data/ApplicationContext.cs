using Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    /// <summary>
    /// Контекст проекта
    /// </summary>
    public class ApplicationContext : DbContext
    {
        public DbSet<Codes> Codes { get; set; } //коды
        public DbSet<Clients> Clients { get; set; } //клиенты
        public DbSet<ClientContacts> ClientContacts { get; set; } //контакты клиентов
        public DbSet<Dates> Dates { get; set; } //контакты клиентов
        public DbSet<Logs> Logs { get; set; } //логи

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dates>().HasKey(x => new { x.Id, x.Dt });
            modelBuilder.Entity<Dates>().Property(u => u.Dt).HasColumnType("date");
        }

        /// <summary>
        /// Конструктор контекста
        /// </summary>
        /// <param name="options"></param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Создаём базу и накатываем миграции
            Database.Migrate();
        }
    }
}