using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class AddDeleteAdminPermissionAndAdminToDeleteToSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            value: "deleteAdmin");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2706));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2689));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2517));

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Email", "AccountCreation", "Lastname", "Name", "Password", "ProfilePicturePath" },
            values: new object[] { "delete@smarthome.com", new DateTime(2024, 10, 6, 1, 18, 35, 372, DateTimeKind.Local).AddTicks(2670), "me", "delete", "sA$a1234", "path" });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[] { "admin", "deleteAdmin" });

        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RoleName", "UserEmail" },
            values: new object[] { "admin", "delete@smarthome.com" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "admin", "deleteAdmin" });

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "admin", "delete@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "deleteAdmin");

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "delete@smarthome.com");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 16, 29, 2, 801, DateTimeKind.Local).AddTicks(8803));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 16, 29, 2, 801, DateTimeKind.Local).AddTicks(8792));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 16, 29, 2, 801, DateTimeKind.Local).AddTicks(8582));
    }
}
