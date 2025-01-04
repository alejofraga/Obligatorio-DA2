using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class Refactor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7623));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "delete@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7611));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7617));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7425));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 36, 20, 545, DateTimeKind.Local).AddTicks(4653));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "delete@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 36, 20, 545, DateTimeKind.Local).AddTicks(4639));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 36, 20, 545, DateTimeKind.Local).AddTicks(4646));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 36, 20, 545, DateTimeKind.Local).AddTicks(4516));
    }
}
