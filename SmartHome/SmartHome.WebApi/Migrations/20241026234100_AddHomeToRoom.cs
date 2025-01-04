using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class AddHomeToRoom : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 26, 20, 41, 0, 62, DateTimeKind.Local).AddTicks(7762));

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_HomeId",
            table: "Rooms",
            column: "HomeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Rooms_Homes_HomeId",
            table: "Rooms",
            column: "HomeId",
            principalTable: "Homes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Rooms_Homes_HomeId",
            table: "Rooms");

        migrationBuilder.DropIndex(
            name: "IX_Rooms_HomeId",
            table: "Rooms");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 25, 16, 19, 48, 234, DateTimeKind.Local).AddTicks(9504));
    }
}
