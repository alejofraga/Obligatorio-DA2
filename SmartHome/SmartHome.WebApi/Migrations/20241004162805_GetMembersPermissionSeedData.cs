using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class GetMembersPermissionSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            value: "getMembers");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 13, 28, 4, 114, DateTimeKind.Local).AddTicks(9491));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 13, 28, 4, 114, DateTimeKind.Local).AddTicks(9482));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 13, 28, 4, 114, DateTimeKind.Local).AddTicks(9356));

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[] { "homeOwner", "getMembers" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "homeOwner", "getMembers" });

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "getMembers");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 39, 49, 341, DateTimeKind.Local).AddTicks(8318));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 39, 49, 341, DateTimeKind.Local).AddTicks(8310));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 12, 39, 49, 341, DateTimeKind.Local).AddTicks(8175));
    }
}
