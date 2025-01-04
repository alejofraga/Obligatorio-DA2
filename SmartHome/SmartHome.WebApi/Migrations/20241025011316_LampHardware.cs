using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class LampHardware : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Discriminator",
            table: "Hardwares",
            type: "nvarchar(13)",
            maxLength: 13,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<bool>(
            name: "IsOn",
            table: "Hardwares",
            type: "bit",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 24, 22, 13, 15, 249, DateTimeKind.Local).AddTicks(3119));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Discriminator",
            table: "Hardwares");

        migrationBuilder.DropColumn(
            name: "IsOn",
            table: "Hardwares");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 24, 22, 4, 22, 922, DateTimeKind.Local).AddTicks(5576));
    }
}
