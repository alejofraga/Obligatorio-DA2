using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class UpdateSytemPermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "admin", "createAdmin&CompanyOwner" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "admin", "deleteAdmin" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "admin", "getCompanies" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "admin", "getUsers" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "companyOwner", "createCompany" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "companyOwner", "createDevice" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "homeOwner", "addMember" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "homeOwner", "createHome" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "homeOwner", "getMembers" });

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "sendNotifications");

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "admin", "sa@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Name",
            keyValue: "admin");

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Name",
            keyValue: "companyOwner");

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Name",
            keyValue: "homeOwner");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "addMember");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "createAdmin&CompanyOwner");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "createCompany");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "createDevice");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "createHome");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "deleteAdmin");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "getCompanies");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "getMembers");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "getUsers");

        migrationBuilder.AlterColumn<string>(
            name: "Password",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Lastname",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "DeviceTypeName",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.InsertData(
            table: "Roles",
            column: "Name",
            values: new object[]
            {
                "Admin",
                "CompanyOwner",
                "HomeOwner"
            });

        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            values: new object[]
            {
                "AddMember",
                "BeHomeMember",
                "CreateCompany",
                "CreateDevice",
                "CreateHome",
                "CreateUserWithRole",
                "DeleteAdmin",
                "GetCompanies",
                "GetMembers",
                "GetUsers"
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 11, 11, 15, 19, 40, 544, DateTimeKind.Local).AddTicks(3545));

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[,]
            {
                { "Admin", "CreateUserWithRole" },
                { "Admin", "DeleteAdmin" },
                { "Admin", "GetCompanies" },
                { "Admin", "GetUsers" },
                { "CompanyOwner", "CreateCompany" },
                { "CompanyOwner", "CreateDevice" },
                { "HomeOwner", "AddMember" },
                { "HomeOwner", "BeHomeMember" },
                { "HomeOwner", "CreateHome" },
                { "HomeOwner", "GetMembers" }
            });

        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RoleName", "UserEmail" },
            values: new object[] { "Admin", "sa@smarthome.com" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "Admin", "CreateUserWithRole" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "Admin", "DeleteAdmin" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "Admin", "GetCompanies" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "Admin", "GetUsers" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "CompanyOwner", "CreateCompany" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "CompanyOwner", "CreateDevice" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "HomeOwner", "AddMember" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "HomeOwner", "BeHomeMember" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "HomeOwner", "CreateHome" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleName", "SystemPermissionName" },
            keyValues: new object[] { "HomeOwner", "GetMembers" });

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "Admin", "sa@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Name",
            keyValue: "Admin");

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Name",
            keyValue: "CompanyOwner");

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Name",
            keyValue: "HomeOwner");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "AddMember");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "BeHomeMember");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "CreateCompany");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "CreateDevice");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "CreateHome");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "CreateUserWithRole");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "DeleteAdmin");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "GetCompanies");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "GetMembers");

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Name",
            keyValue: "GetUsers");

        migrationBuilder.AlterColumn<string>(
            name: "Password",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Lastname",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "DeviceTypeName",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.InsertData(
            table: "Roles",
            column: "Name",
            values: new object[]
            {
                "admin",
                "companyOwner",
                "homeOwner"
            });

        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            values: new object[]
            {
                "addMember",
                "createAdmin&CompanyOwner",
                "createCompany",
                "createDevice",
                "createHome",
                "deleteAdmin",
                "getCompanies",
                "getMembers",
                "getUsers",
                "sendNotifications"
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 11, 7, 18, 53, 14, 405, DateTimeKind.Local).AddTicks(582));

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[,]
            {
                { "admin", "createAdmin&CompanyOwner" },
                { "admin", "deleteAdmin" },
                { "admin", "getCompanies" },
                { "admin", "getUsers" },
                { "companyOwner", "createCompany" },
                { "companyOwner", "createDevice" },
                { "homeOwner", "addMember" },
                { "homeOwner", "createHome" },
                { "homeOwner", "getMembers" }
            });

        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RoleName", "UserEmail" },
            values: new object[] { "admin", "sa@smarthome.com" });
    }
}
