using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class DeviceTypesAndSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DeviceTypes",
            columns: table => new
            {
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DeviceTypes", x => x.Name);
            });

        migrationBuilder.InsertData(
            table: "DeviceTypes",
            column: "Name",
            values: new object[]
            {
                "camera",
                "sensor"
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DeviceTypes");
    }
}
