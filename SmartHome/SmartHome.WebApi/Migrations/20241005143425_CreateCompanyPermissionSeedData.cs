using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class CreateCompanyPermissionSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            value: "createCompany");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 11, 34, 24, 735, DateTimeKind.Local).AddTicks(240));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 11, 34, 24, 735, DateTimeKind.Local).AddTicks(230));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 11, 34, 24, 735, DateTimeKind.Local).AddTicks(46));

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[] { "companyOwner", "createCompany" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "companyOwner", "createCompany" });

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "createCompany");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 10, 53, 17, 515, DateTimeKind.Local).AddTicks(2591));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 10, 53, 17, 515, DateTimeKind.Local).AddTicks(2583));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 5, 10, 53, 17, 515, DateTimeKind.Local).AddTicks(2451));
    }
}
