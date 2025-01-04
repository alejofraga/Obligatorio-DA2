using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class HardwareSensorRelationMapped : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Hardwares_Devices_SensorModelNumber",
            table: "Hardwares");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 8, 32, 895, DateTimeKind.Local).AddTicks(7188));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 8, 32, 895, DateTimeKind.Local).AddTicks(7180));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 8, 32, 895, DateTimeKind.Local).AddTicks(7051));

        migrationBuilder.AddForeignKey(
            name: "FK_Hardwares_Devices_SensorModelNumber",
            table: "Hardwares",
            column: "SensorModelNumber",
            principalTable: "Devices",
            principalColumn: "ModelNumber");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Hardwares_Devices_SensorModelNumber",
            table: "Hardwares");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 6, 49, 588, DateTimeKind.Local).AddTicks(3221));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 6, 49, 588, DateTimeKind.Local).AddTicks(3213));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 6, 49, 588, DateTimeKind.Local).AddTicks(3068));

        migrationBuilder.AddForeignKey(
            name: "FK_Hardwares_Devices_SensorModelNumber",
            table: "Hardwares",
            column: "SensorModelNumber",
            principalTable: "Devices",
            principalColumn: "ModelNumber",
            onDelete: ReferentialAction.SetNull);
    }
}
