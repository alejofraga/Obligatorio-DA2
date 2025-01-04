using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.WebApi.Migrations;

/// <inheritdoc />
public partial class CameraDeviceHierarchy : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Name);
            });

        migrationBuilder.CreateTable(
            name: "SystemPermissions",
            columns: table => new
            {
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemPermissions", x => x.Name);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                AccountCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                ProfilePicturePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Email);
            });

        migrationBuilder.CreateTable(
            name: "RoleSystemPermission",
            columns: table => new
            {
                RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                SystemPermissionName = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleSystemPermission", x => new { x.RoleName, x.SystemPermissionName });
                table.ForeignKey(
                    name: "FK_RoleSystemPermission_Roles_RoleName",
                    column: x => x.RoleName,
                    principalTable: "Roles",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleSystemPermission_SystemPermissions_SystemPermissionName",
                    column: x => x.SystemPermissionName,
                    principalTable: "SystemPermissions",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                RUT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                OwnerEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.RUT);
                table.ForeignKey(
                    name: "FK_Companies_Users_OwnerEmail",
                    column: x => x.OwnerEmail,
                    principalTable: "Users",
                    principalColumn: "Email",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Homes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OwnerEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                MemberCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Homes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Homes_Users_OwnerEmail",
                    column: x => x.OwnerEmail,
                    principalTable: "Users",
                    principalColumn: "Email",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRole",
            columns: table => new
            {
                RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRole", x => new { x.RoleName, x.UserEmail });
                table.ForeignKey(
                    name: "FK_UserRole_Roles_RoleName",
                    column: x => x.RoleName,
                    principalTable: "Roles",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRole_Users_UserEmail",
                    column: x => x.UserEmail,
                    principalTable: "Users",
                    principalColumn: "Email",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                ModelNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CompanyRUT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                HasMovementDetection = table.Column<bool>(type: "bit", nullable: true),
                HasPersonDetection = table.Column<bool>(type: "bit", nullable: true),
                IsOutdoor = table.Column<bool>(type: "bit", nullable: true),
                IsIndoor = table.Column<bool>(type: "bit", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.ModelNumber);
                table.ForeignKey(
                    name: "FK_Devices_Companies_CompanyRUT",
                    column: x => x.CompanyRUT,
                    principalTable: "Companies",
                    principalColumn: "RUT",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Coordinates",
            columns: table => new
            {
                Latitude = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Longitude = table.Column<string>(type: "nvarchar(450)", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Coordinates", x => new { x.Latitude, x.Longitude });
                table.ForeignKey(
                    name: "FK_Coordinates_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Locations",
            columns: table => new
            {
                Address = table.Column<string>(type: "nvarchar(450)", nullable: false),
                DoorNumber = table.Column<int>(type: "int", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Locations", x => new { x.Address, x.DoorNumber });
                table.ForeignKey(
                    name: "FK_Locations_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Members",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Members", x => x.Id);
                table.ForeignKey(
                    name: "FK_Members_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Members_Users_UserEmail",
                    column: x => x.UserEmail,
                    principalTable: "Users",
                    principalColumn: "Email");
            });

        migrationBuilder.CreateTable(
            name: "Hardwares",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DeviceModelNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                Connected = table.Column<bool>(type: "bit", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Hardwares", x => x.Id);
                table.ForeignKey(
                    name: "FK_Hardwares_Devices_DeviceModelNumber",
                    column: x => x.DeviceModelNumber,
                    principalTable: "Devices",
                    principalColumn: "ModelNumber");
                table.ForeignKey(
                    name: "FK_Hardwares_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HomePermissions",
            columns: table => new
            {
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomePermissions", x => x.Name);
                table.ForeignKey(
                    name: "FK_HomePermissions_Members_MemberId",
                    column: x => x.MemberId,
                    principalTable: "Members",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                HardwareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notifications_Hardwares_HardwareId",
                    column: x => x.HardwareId,
                    principalTable: "Hardwares",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "NotiActions",
            columns: table => new
            {
                NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NotiActions", x => new { x.MemberId, x.NotificationId });
                table.ForeignKey(
                    name: "FK_NotiActions_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_NotiActions_Members_MemberId",
                    column: x => x.MemberId,
                    principalTable: "Members",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_NotiActions_Notifications_NotificationId",
                    column: x => x.NotificationId,
                    principalTable: "Notifications",
                    principalColumn: "Id");
            });

        migrationBuilder.InsertData(
            table: "Roles",
            column: "Name",
            values: new object[]
            {
                "admin",
                "companyOwner"
            });

        migrationBuilder.InsertData(
            table: "SystemPermissions",
            column: "Name",
            value: "createAdmin");

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Email", "AccountCreation", "Lastname", "Name", "Password", "ProfilePicturePath" },
            values: new object[] { "sa@smarthome.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "System", "sA$a1234", "path" });

        migrationBuilder.InsertData(
            table: "Companies",
            columns: new[] { "RUT", "LogoUrl", "Name", "OwnerEmail" },
            values: new object[] { "111111111111", "logo", "Cameras SA", "sa@smarthome.com" });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleName", "SystemPermissionName" },
            values: new object[] { "admin", "createAdmin" });

        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RoleName", "UserEmail" },
            values: new object[] { "admin", "sa@smarthome.com" });

        migrationBuilder.InsertData(
            table: "Devices",
            columns: new[] { "ModelNumber", "CompanyRUT", "Description", "Discriminator", "HasMovementDetection", "HasPersonDetection", "IsIndoor", "IsOutdoor", "Name", "Photos" },
            values: new object[] { "10", "111111111111", "Camera for living room", "Camera", false, false, true, false, "Camera1", "[\"photo1\",\"photo2\"]" });

        migrationBuilder.InsertData(
            table: "Devices",
            columns: new[] { "ModelNumber", "CompanyRUT", "Description", "Discriminator", "Name", "Photos" },
            values: new object[] { "77", "111111111111", "yard sensor 01", "Device", "Sensor1", "[\"photo13\",\"photo278\"]" });

        migrationBuilder.CreateIndex(
            name: "IX_Companies_OwnerEmail",
            table: "Companies",
            column: "OwnerEmail");

        migrationBuilder.CreateIndex(
            name: "IX_Coordinates_HomeId",
            table: "Coordinates",
            column: "HomeId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Devices_CompanyRUT",
            table: "Devices",
            column: "CompanyRUT");

        migrationBuilder.CreateIndex(
            name: "IX_Hardwares_DeviceModelNumber",
            table: "Hardwares",
            column: "DeviceModelNumber");

        migrationBuilder.CreateIndex(
            name: "IX_Hardwares_HomeId",
            table: "Hardwares",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_HomePermissions_MemberId",
            table: "HomePermissions",
            column: "MemberId");

        migrationBuilder.CreateIndex(
            name: "IX_Homes_OwnerEmail",
            table: "Homes",
            column: "OwnerEmail");

        migrationBuilder.CreateIndex(
            name: "IX_Locations_HomeId",
            table: "Locations",
            column: "HomeId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Members_HomeId",
            table: "Members",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_Members_UserEmail",
            table: "Members",
            column: "UserEmail");

        migrationBuilder.CreateIndex(
            name: "IX_NotiActions_HomeId",
            table: "NotiActions",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_NotiActions_NotificationId",
            table: "NotiActions",
            column: "NotificationId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_HardwareId",
            table: "Notifications",
            column: "HardwareId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleSystemPermission_SystemPermissionName",
            table: "RoleSystemPermission",
            column: "SystemPermissionName");

        migrationBuilder.CreateIndex(
            name: "IX_UserRole_UserEmail",
            table: "UserRole",
            column: "UserEmail");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Coordinates");

        migrationBuilder.DropTable(
            name: "HomePermissions");

        migrationBuilder.DropTable(
            name: "Locations");

        migrationBuilder.DropTable(
            name: "NotiActions");

        migrationBuilder.DropTable(
            name: "RoleSystemPermission");

        migrationBuilder.DropTable(
            name: "UserRole");

        migrationBuilder.DropTable(
            name: "Members");

        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropTable(
            name: "SystemPermissions");

        migrationBuilder.DropTable(
            name: "Roles");

        migrationBuilder.DropTable(
            name: "Hardwares");

        migrationBuilder.DropTable(
            name: "Devices");

        migrationBuilder.DropTable(
            name: "Homes");

        migrationBuilder.DropTable(
            name: "Companies");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
