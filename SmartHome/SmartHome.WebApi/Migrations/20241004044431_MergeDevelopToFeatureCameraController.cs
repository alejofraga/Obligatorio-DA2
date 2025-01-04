using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class MergeDevelopToFeatureCameraController : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 1, 44, 30, 839, DateTimeKind.Local).AddTicks(3348));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 1, 44, 30, 839, DateTimeKind.Local).AddTicks(3338));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 1, 44, 30, 839, DateTimeKind.Local).AddTicks(3223));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 1, 7, 35, 637, DateTimeKind.Local).AddTicks(3716));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 1, 7, 35, 637, DateTimeKind.Local).AddTicks(3706));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 1, 7, 35, 637, DateTimeKind.Local).AddTicks(3599));
    }
}
