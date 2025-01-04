using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class AddRoom : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "RoomId",
            table: "Hardwares",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rooms_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 25, 15, 3, 56, 659, DateTimeKind.Local).AddTicks(4336));

        migrationBuilder.CreateIndex(
            name: "IX_Hardwares_RoomId",
            table: "Hardwares",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_HomeId",
            table: "Rooms",
            column: "HomeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Hardwares_Rooms_RoomId",
            table: "Hardwares",
            column: "RoomId",
            principalTable: "Rooms",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Hardwares_Rooms_RoomId",
            table: "Hardwares");

        migrationBuilder.DropTable(
            name: "Rooms");

        migrationBuilder.DropIndex(
            name: "IX_Hardwares_RoomId",
            table: "Hardwares");

        migrationBuilder.DropColumn(
            name: "RoomId",
            table: "Hardwares");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 23, 17, 21, 37, 244, DateTimeKind.Local).AddTicks(4388));
    }
}
