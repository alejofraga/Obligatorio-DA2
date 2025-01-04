using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class HomeOwnerSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Roles",
            column: "Name",
            value: "homeOwner");

        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            value: "createHome");

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Email", "AccountCreation", "Lastname", "Name", "Password", "ProfilePicturePath" },
            values: new object[] { "ho@smarthome.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner", "home", "sA$a1234", "path" });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[] { "homeOwner", "createHome" });

        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RoleName", "UserEmail" },
            values: new object[] { "homeOwner", "ho@smarthome.com" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "homeOwner", "createHome" });

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "homeOwner", "ho@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Name",
            keyValue: "homeOwner");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "createHome");

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com");
    }
}
