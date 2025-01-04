using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class RefactorSensorHardware : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Discriminator",
            table: "Hardwares",
            type: "nvarchar(21)",
            maxLength: 21,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(13)",
            oldMaxLength: 13);

        migrationBuilder.AddColumn<bool>(
            name: "IsOpen",
            table: "Hardwares",
            type: "bit",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 25, 15, 25, 43, 161, DateTimeKind.Local).AddTicks(9360));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsOpen",
            table: "Hardwares");

        migrationBuilder.AlterColumn<string>(
            name: "Discriminator",
            table: "Hardwares",
            type: "nvarchar(13)",
            maxLength: 13,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(21)",
            oldMaxLength: 21);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 25, 15, 13, 38, 222, DateTimeKind.Local).AddTicks(8865));
    }
}
