using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class AddSessionsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Sessions",
            columns: table => new
            {
                SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sessions", x => x.SessionId);
                table.ForeignKey(
                    name: "FK_Sessions_Users_UserEmail",
                    column: x => x.UserEmail,
                    principalTable: "Users",
                    principalColumn: "Email",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 29, 15, 23, 9, 642, DateTimeKind.Local).AddTicks(3389));

        migrationBuilder.CreateIndex(
            name: "IX_Sessions_UserEmail",
            table: "Sessions",
            column: "UserEmail",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Sessions");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 23, 17, 21, 37, 244, DateTimeKind.Local).AddTicks(4388));
    }
}
