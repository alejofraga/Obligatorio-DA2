using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class RefactorDeviceHierarchy : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "DeviceType",
            table: "Devices",
            type: "nvarchar(8)",
            maxLength: 8,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(21)",
            oldMaxLength: 21);

        migrationBuilder.AddColumn<string>(
            name: "DeviceTypeName",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 25, 12, 4, 7, 619, DateTimeKind.Local).AddTicks(37));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DeviceTypeName",
            table: "Devices");

        migrationBuilder.AlterColumn<string>(
            name: "DeviceType",
            table: "Devices",
            type: "nvarchar(21)",
            maxLength: 21,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(8)",
            oldMaxLength: 8);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 24, 22, 13, 15, 249, DateTimeKind.Local).AddTicks(3119));
    }
}
