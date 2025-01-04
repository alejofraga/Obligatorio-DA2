using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class RefactorDiscriminator : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Discriminator",
            table: "Devices");

        migrationBuilder.AddColumn<string>(
            name: "DeviceType",
            table: "Devices",
            type: "nvarchar(21)",
            maxLength: 21,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 24, 20, 28, 13, 891, DateTimeKind.Local).AddTicks(5419));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DeviceType",
            table: "Devices");

        migrationBuilder.AddColumn<string>(
            name: "Discriminator",
            table: "Devices",
            type: "nvarchar(8)",
            maxLength: 8,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 24, 20, 14, 41, 439, DateTimeKind.Local).AddTicks(9593));
    }
}
