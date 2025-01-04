using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class RefactorValidator : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Validators");

        migrationBuilder.AddColumn<string>(
            name: "ValidatorTypeName",
            table: "Companies",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 23, 14, 18, 22, 319, DateTimeKind.Local).AddTicks(7052));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ValidatorTypeName",
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
            value: new DateTime(2024, 10, 23, 13, 47, 36, 312, DateTimeKind.Local).AddTicks(3483));
    }
}
