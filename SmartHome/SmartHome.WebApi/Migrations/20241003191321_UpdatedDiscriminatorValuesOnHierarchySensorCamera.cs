using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class UpdatedDiscriminatorValuesOnHierarchySensorCamera : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Hardwares_Devices_DeviceModelNumber",
            table: "Hardwares");

        migrationBuilder.DeleteData(
            table: "Devices",
            keyColumn: "ModelNumber",
            keyValue: "77");

        migrationBuilder.RenameColumn(
            name: "DeviceModelNumber",
            table: "Hardwares",
            newName: "SensorModelNumber");

        migrationBuilder.RenameIndex(
            name: "IX_Hardwares_DeviceModelNumber",
            table: "Hardwares",
            newName: "IX_Hardwares_SensorModelNumber");

        migrationBuilder.InsertData(
            table: "Devices",
            columns: new[] { "ModelNumber", "CompanyRUT", "Description", "Discriminator", "Name", "Photos" },
            values: new object[] { "77", "111111111111", "yard sensor 01", "Sensor", "Sensor1", "[\"photo13\",\"photo278\"]" });

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

        migrationBuilder.DeleteData(
            table: "Devices",
            keyColumn: "ModelNumber",
            keyValue: "77");

        migrationBuilder.RenameColumn(
            name: "SensorModelNumber",
            table: "Hardwares",
            newName: "DeviceModelNumber");

        migrationBuilder.RenameIndex(
            name: "IX_Hardwares_SensorModelNumber",
            table: "Hardwares",
            newName: "IX_Hardwares_DeviceModelNumber");

        migrationBuilder.InsertData(
            table: "Devices",
            columns: new[] { "ModelNumber", "CompanyRUT", "Description", "Discriminator", "Name", "Photos" },
            values: new object[] { "77", "111111111111", "yard sensor 01", "Device", "Sensor1", "[\"photo13\",\"photo278\"]" });

        migrationBuilder.AddForeignKey(
            name: "FK_Hardwares_Devices_DeviceModelNumber",
            table: "Hardwares",
            column: "DeviceModelNumber",
            principalTable: "Devices",
            principalColumn: "ModelNumber");
    }
}
