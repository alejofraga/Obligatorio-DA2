using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class AddCompanyOwnerToSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            value: "createDevice");

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Email", "AccountCreation", "Lastname", "Name", "Password", "ProfilePicturePath" },
            values: new object[] { "co@smarthome.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner", "company", "sA$a1234", "path" });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[] { "companyOwner", "createDevice" });

        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RoleName", "UserEmail" },
            values: new object[] { "companyOwner", "co@smarthome.com" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "companyOwner", "createDevice" });

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "companyOwner", "co@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "createDevice");

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com");
    }
}
