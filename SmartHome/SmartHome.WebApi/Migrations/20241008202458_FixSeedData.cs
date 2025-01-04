using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class FixSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Devices",
            keyColumn: "ModelNumber",
            keyValue: "10");

        migrationBuilder.DeleteData(
            table: "Devices",
            keyColumn: "ModelNumber",
            keyValue: "77");

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "admin", "delete@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "companyOwner", "co@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RoleName", "UserEmail" },
            keyValues: new object[] { "homeOwner", "ho@smarthome.com" });

        migrationBuilder.DeleteData(
            table: "Companies",
            keyColumn: "RUT",
            keyValue: "111111111111");

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "co@smarthome.com");

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "delete@smarthome.com");

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "ho@smarthome.com");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 17, 24, 58, 122, DateTimeKind.Local).AddTicks(5907));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Companies",
            columns: new[] { "RUT", "LogoUrl", "Name", "OwnerEmail" },
            values: new object[] { "111111111111", "logo", "Cameras SA", "sa@smarthome.com" });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Email",
            keyValue: "sa@smarthome.com",
            column: "AccountCreation",
            value: new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7425));

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Email", "AccountCreation", "Lastname", "Name", "Password", "ProfilePicturePath" },
            values: new object[,]
            {
                { "co@smarthome.com", new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7623), "owner", "company", "sA$a1234", "path" },
                { "delete@smarthome.com", new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7611), "me", "delete", "sA$a1234", "path" },
                { "ho@smarthome.com", new DateTime(2024, 10, 8, 11, 37, 21, 684, DateTimeKind.Local).AddTicks(7617), "owner", "home", "sA$a1234", "path" }
            });

        migrationBuilder.InsertData(
            table: "Devices",
            columns: new[] { "ModelNumber", "CompanyRUT", "Description", "Discriminator", "HasMovementDetection", "HasPersonDetection", "IsIndoor", "IsOutdoor", "Name", "Photos" },
            values: new object[] { "10", "111111111111", "Camera for living room", "Camera", false, false, true, false, "Camera1", "[\"photo1\",\"photo2\"]" });

        migrationBuilder.InsertData(
            table: "Devices",
            columns: new[] { "ModelNumber", "CompanyRUT", "Description", "Discriminator", "Name", "Photos" },
            values: new object[] { "77", "111111111111", "yard sensor 01", "Sensor", "Sensor1", "[\"photo13\",\"photo278\"]" });

        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RoleName", "UserEmail" },
            values: new object[,]
            {
                { "admin", "delete@smarthome.com" },
                { "companyOwner", "co@smarthome.com" },
                { "homeOwner", "ho@smarthome.com" }
            });
    }
}
