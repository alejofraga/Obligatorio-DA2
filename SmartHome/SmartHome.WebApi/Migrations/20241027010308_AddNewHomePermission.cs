using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class AddNewHomePermission : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 26, 22, 3, 7, 330, DateTimeKind.Local).AddTicks(8085));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 26, 20, 41, 0, 62, DateTimeKind.Local).AddTicks(7762));
    }
}
