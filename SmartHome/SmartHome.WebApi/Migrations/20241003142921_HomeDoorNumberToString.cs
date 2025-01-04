using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class HomeDoorNumberToString : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Locations",
            table: "Locations");

        migrationBuilder.AlterColumn<string>(
            name: "DoorNumber",
            table: "Locations",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Locations",
            table: "Locations",
            columns: new[] { "Address", "DoorNumber" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Locations",
            table: "Locations");

        migrationBuilder.AlterColumn<int>(
            name: "DoorNumber",
            table: "Locations",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Locations",
            table: "Locations",
            columns: new[] { "Address", "DoorNumber" });
    }
}
