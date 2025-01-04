using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class ValidatorUpdate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Validator_Id",
            table: "Companies");

        migrationBuilder.CreateTable(
            name: "Validators",
            columns: table => new
            {
                CompanyRUT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Validators", x => x.CompanyRUT);
                table.ForeignKey(
                    name: "FK_Validators_Companies_CompanyRUT",
                    column: x => x.CompanyRUT,
                    principalTable: "Companies",
                    principalColumn: "RUT",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 23, 13, 25, 43, 343, DateTimeKind.Local).AddTicks(9861));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Validators");

        migrationBuilder.AddColumn<Guid>(
            name: "Validator_Id",
            table: "Companies",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 23, 13, 15, 31, 587, DateTimeKind.Local).AddTicks(4623));
    }
}
