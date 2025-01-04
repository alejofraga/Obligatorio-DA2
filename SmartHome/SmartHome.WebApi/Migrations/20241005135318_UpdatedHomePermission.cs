using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class UpdatedHomePermission : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomePermissions_Members_MemberId",
            table: "HomePermissions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_HomePermissions",
            table: "HomePermissions");

        migrationBuilder.AlterColumn<Guid>(
            name: "MemberId",
            table: "HomePermissions",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_HomePermissions",
            table: "HomePermissions",
            columns: new[] { "Name", "MemberId" });

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

        migrationBuilder.AddForeignKey(
            name: "FK_HomePermissions_Members_MemberId",
            table: "HomePermissions",
            column: "MemberId",
            principalTable: "Members",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomePermissions_Members_MemberId",
            table: "HomePermissions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_HomePermissions",
            table: "HomePermissions");

        migrationBuilder.AlterColumn<Guid>(
            name: "MemberId",
            table: "HomePermissions",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddPrimaryKey(
            name: "PK_HomePermissions",
            table: "HomePermissions",
            column: "Name");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 18, 53, 10, 976, DateTimeKind.Local).AddTicks(3912));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 18, 53, 10, 976, DateTimeKind.Local).AddTicks(3905));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 4, 18, 53, 10, 976, DateTimeKind.Local).AddTicks(3769));

        migrationBuilder.AddForeignKey(
            name: "FK_HomePermissions_Members_MemberId",
            table: "HomePermissions",
            column: "MemberId",
            principalTable: "Members",
            principalColumn: "Id");
    }
}
