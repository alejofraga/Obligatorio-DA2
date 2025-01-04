using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class RenameSensorModelNumberToDeviceModelNumber : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "SensorModelNumber",
            table: "Hardwares",
            newName: "DeviceModelNumber");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "DeviceModelNumber",
            table: "Hardwares",
            newName: "SensorModelNumber");
    }
}
