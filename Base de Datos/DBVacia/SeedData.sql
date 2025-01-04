INSERT INTO master.dbo.DeviceTypes (Name) VALUES
	 (N'Camera'),
	 (N'Lamp'),
	 (N'MovementSensor'),
	 (N'Sensor');


INSERT INTO master.dbo.Roles (Name) VALUES
	 (N'Admin'),
	 (N'CompanyOwner'),
	 (N'HomeOwner');


INSERT INTO master.dbo.SystemPermissions (Name) VALUES
	 (N'AddMember'),
	 (N'BeHomeMember'),
	 (N'CreateCompany'),
	 (N'CreateDevice'),
	 (N'CreateHome'),
	 (N'CreateUserWithRole'),
	 (N'DeleteAdmin'),
	 (N'GetCompanies'),
	 (N'GetHomes'),
	 (N'GetMembers');
INSERT INTO master.dbo.SystemPermissions (Name) VALUES
	 (N'GetUsers');


INSERT INTO master.dbo.RoleSystemPermission (RoleName,SystemPermissionName) VALUES
	 (N'HomeOwner',N'AddMember'),
	 (N'HomeOwner',N'BeHomeMember'),
	 (N'CompanyOwner',N'CreateCompany'),
	 (N'CompanyOwner',N'CreateDevice'),
	 (N'HomeOwner',N'CreateHome'),
	 (N'Admin',N'CreateUserWithRole'),
	 (N'Admin',N'DeleteAdmin'),
	 (N'Admin',N'GetCompanies'),
	 (N'HomeOwner',N'GetHomes'),
	 (N'HomeOwner',N'GetMembers');
INSERT INTO master.dbo.RoleSystemPermission (RoleName,SystemPermissionName) VALUES
	 (N'Admin',N'GetUsers');


INSERT INTO master.dbo.Users (Email,Password,Name,Lastname,AccountCreation,ProfilePicturePath) VALUES
	 (N'sa@smarthome.com',N'sA$a1234',N'System',N'Admin','2024-11-12 12:18:42.9187525',N'path');


INSERT INTO master.dbo.UserRole (RoleName,UserEmail) VALUES
	 (N'Admin',N'sa@smarthome.com');

