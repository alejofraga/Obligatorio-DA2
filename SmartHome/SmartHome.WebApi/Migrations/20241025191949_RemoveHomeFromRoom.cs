using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class RemoveHomeFromRoom : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 25, 15, 3, 56, 659, DateTimeKind.Local).AddTicks(4336));

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
}
