using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class UpdatedSeedDataWithNewDeviceTypes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "DeviceTypes",
            keyColumn: "Name",
            keyValue: "camera");

        migrationBuilder.DeleteData(
            table: "DeviceTypes",
            keyColumn: "Name",
            keyValue: "sensor");

        migrationBuilder.InsertData(
            table: "DeviceTypes",
            column: "Name",
            values: new object[]
            {
                "Camera",
                "Lamp",
                "MovementSensor",
                "Sensor"
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 26, 19, 3, 15, 404, DateTimeKind.Local).AddTicks(9340));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "DeviceTypes",
            keyColumn: "Name",
            keyValue: "Camera");

        migrationBuilder.DeleteData(
            table: "DeviceTypes",
            keyColumn: "Name",
            keyValue: "Lamp");

        migrationBuilder.DeleteData(
            table: "DeviceTypes",
            keyColumn: "Name",
            keyValue: "MovementSensor");

        migrationBuilder.DeleteData(
            table: "DeviceTypes",
            keyColumn: "Name",
            keyValue: "Sensor");

        migrationBuilder.InsertData(
            table: "DeviceTypes",
            column: "Name",
            values: new object[]
            {
                "camera",
                "sensor"
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 25, 15, 25, 43, 161, DateTimeKind.Local).AddTicks(9360));
    }
}
