using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Init_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id клиента")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Наименование клиента")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                },
                comment: "Клиенты");

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    order_number = table.Column<long>(type: "bigint", nullable: false, comment: "Порядковый номер")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<int>(type: "int", nullable: false, comment: "Код"),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Значение кода")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.order_number);
                },
                comment: "Коды");

            migrationBuilder.CreateTable(
                name: "Dates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id клиента"),
                    Dt = table.Column<DateTime>(type: "date", nullable: false, comment: "Дата клиента")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dates", x => new { x.Id, x.Dt });
                },
                comment: "Даты клиента");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование вызываемого метода"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Тип вызываемого метода"),
                    success = table.Column<bool>(type: "bit", nullable: false, comment: "Признак успешного выполнения"),
                    dateStart = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата начала"),
                    dateEnd = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата окончания"),
                    @in = table.Column<string>(name: "in", type: "nvarchar(max)", nullable: true, comment: "Данные на вход"),
                    @out = table.Column<string>(name: "out", type: "nvarchar(max)", nullable: true, comment: "Данные на выход")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.id);
                },
                comment: "Логи");

            migrationBuilder.CreateTable(
                name: "ClientContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id контакта")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<long>(type: "bigint", nullable: true, comment: "Id клиента"),
                    ContactType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "Тип контакта"),
                    ContactValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "Значение контакта")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientContacts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                },
                comment: "Контакты клиента");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContacts_ClientId",
                table: "ClientContacts",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientContacts");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "Dates");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
