

CREATE TABLE master.dbo.DeviceTypes (
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_DeviceTypes PRIMARY KEY (Name)
);

CREATE TABLE master.dbo.Roles (
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Roles PRIMARY KEY (Name)
);


CREATE TABLE master.dbo.SystemPermissions (
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_SystemPermissions PRIMARY KEY (Name)
);


CREATE TABLE master.dbo.Users (
	Email nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Lastname nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	AccountCreation datetime2 NOT NULL,
	ProfilePicturePath nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Email)
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Users_Email ON dbo.Users (  Email ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);

CREATE TABLE master.dbo.Companies (
	RUT nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LogoUrl nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	OwnerEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ValidatorTypeName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'' NOT NULL,
	CONSTRAINT PK_Companies PRIMARY KEY (RUT),
	CONSTRAINT FK_Companies_Users_OwnerEmail FOREIGN KEY (OwnerEmail) REFERENCES master.dbo.Users(Email) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Companies_OwnerEmail ON dbo.Companies (  OwnerEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.Devices (
	ModelNumber nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Photos nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CompanyRUT nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HasMovementDetection bit NULL,
	HasPersonDetection bit NULL,
	IsOutdoor bit NULL,
	IsIndoor bit NULL,
	DeviceType nvarchar(8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DeviceTypeName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Devices PRIMARY KEY (ModelNumber),
	CONSTRAINT FK_Devices_Companies_CompanyRUT FOREIGN KEY (CompanyRUT) REFERENCES master.dbo.Companies(RUT) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Devices_CompanyRUT ON dbo.Devices (  CompanyRUT ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;



CREATE TABLE master.dbo.Homes (
	Id uniqueidentifier NOT NULL,
	OwnerEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	MemberCount int NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Homes PRIMARY KEY (Id),
	CONSTRAINT FK_Homes_Users_OwnerEmail FOREIGN KEY (OwnerEmail) REFERENCES master.dbo.Users(Email) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Homes_OwnerEmail ON dbo.Homes (  OwnerEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.Locations (
	Address nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DoorNumber nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HomeId uniqueidentifier NOT NULL,
	CONSTRAINT PK_Locations PRIMARY KEY (Address,DoorNumber),
	CONSTRAINT FK_Locations_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(Id) ON DELETE CASCADE
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Locations_HomeId ON dbo.Locations (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;



CREATE TABLE master.dbo.Members (
	Id uniqueidentifier NOT NULL,
	UserEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HomeId uniqueidentifier NOT NULL,
	CONSTRAINT PK_Members PRIMARY KEY (Id),
	CONSTRAINT FK_Members_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Members_Users_UserEmail FOREIGN KEY (UserEmail) REFERENCES master.dbo.Users(Email)
);
 CREATE NONCLUSTERED INDEX IX_Members_HomeId ON dbo.Members (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Members_UserEmail ON dbo.Members (  UserEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.RoleSystemPermission (
	RoleName nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	SystemPermissionName nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_RoleSystemPermission PRIMARY KEY (RoleName,SystemPermissionName),
	CONSTRAINT FK_RoleSystemPermission_Roles_RoleName FOREIGN KEY (RoleName) REFERENCES master.dbo.Roles(Name) ON DELETE CASCADE,
	CONSTRAINT FK_RoleSystemPermission_SystemPermissions_SystemPermissionName FOREIGN KEY (SystemPermissionName) REFERENCES master.dbo.SystemPermissions(Name) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_RoleSystemPermission_SystemPermissionName ON dbo.RoleSystemPermission (  SystemPermissionName ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.Rooms (
	Id uniqueidentifier NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HomeId uniqueidentifier NOT NULL,
	CONSTRAINT PK_Rooms PRIMARY KEY (Id),
	CONSTRAINT FK_Rooms_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Rooms_HomeId ON dbo.Rooms (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.Sessions (
	SessionId uniqueidentifier NOT NULL,
	UserEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Sessions PRIMARY KEY (SessionId),
	CONSTRAINT FK_Sessions_Users_UserEmail FOREIGN KEY (UserEmail) REFERENCES master.dbo.Users(Email) ON DELETE CASCADE
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Sessions_UserEmail ON dbo.Sessions (  UserEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.UserRole (
	RoleName nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UserEmail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_UserRole PRIMARY KEY (RoleName,UserEmail),
	CONSTRAINT FK_UserRole_Roles_RoleName FOREIGN KEY (RoleName) REFERENCES master.dbo.Roles(Name) ON DELETE CASCADE,
	CONSTRAINT FK_UserRole_Users_UserEmail FOREIGN KEY (UserEmail) REFERENCES master.dbo.Users(Email) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_UserRole_UserEmail ON dbo.UserRole (  UserEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.Coordinates (
	Latitude nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Longitude nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HomeId uniqueidentifier NOT NULL,
	CONSTRAINT PK_Coordinates PRIMARY KEY (Latitude,Longitude),
	CONSTRAINT FK_Coordinates_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(Id) ON DELETE CASCADE
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Coordinates_HomeId ON dbo.Coordinates (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.Hardwares (
	Id uniqueidentifier NOT NULL,
	DeviceModelNumber nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Connected bit NOT NULL,
	HomeId uniqueidentifier NOT NULL,
	Discriminator nvarchar(21) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	IsOn bit NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	RoomId uniqueidentifier NULL,
	IsOpen bit NULL,
	CONSTRAINT PK_Hardwares PRIMARY KEY (Id),
	CONSTRAINT FK_Hardwares_Devices_SensorModelNumber FOREIGN KEY (DeviceModelNumber) REFERENCES master.dbo.Devices(ModelNumber),
	CONSTRAINT FK_Hardwares_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Hardwares_Rooms_RoomId FOREIGN KEY (RoomId) REFERENCES master.dbo.Rooms(Id)
);
 CREATE NONCLUSTERED INDEX IX_Hardwares_HomeId ON dbo.Hardwares (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Hardwares_RoomId ON dbo.Hardwares (  RoomId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Hardwares_SensorModelNumber ON dbo.Hardwares (  DeviceModelNumber ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;

CREATE TABLE master.dbo.HomePermissions (
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	MemberId uniqueidentifier DEFAULT '00000000-0000-0000-0000-000000000000' NOT NULL,
	CONSTRAINT PK_HomePermissions PRIMARY KEY (Name,MemberId),
	CONSTRAINT FK_HomePermissions_Members_MemberId FOREIGN KEY (MemberId) REFERENCES master.dbo.Members(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_HomePermissions_MemberId ON dbo.HomePermissions (  MemberId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;

CREATE TABLE master.dbo.Notifications (
	Id uniqueidentifier NOT NULL,
	Message nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Date] datetime2 NOT NULL,
	HardwareId uniqueidentifier NOT NULL,
	CONSTRAINT PK_Notifications PRIMARY KEY (Id),
	CONSTRAINT FK_Notifications_Hardwares_HardwareId FOREIGN KEY (HardwareId) REFERENCES master.dbo.Hardwares(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Notifications_HardwareId ON dbo.Notifications (  HardwareId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE master.dbo.NotiActions (
	NotificationId uniqueidentifier NOT NULL,
	MemberId uniqueidentifier NOT NULL,
	IsRead bit NOT NULL,
	HomeId uniqueidentifier NOT NULL,
	CONSTRAINT PK_NotiActions PRIMARY KEY (MemberId,NotificationId),
	CONSTRAINT FK_NotiActions_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(Id) ON DELETE CASCADE,
	CONSTRAINT FK_NotiActions_Members_MemberId FOREIGN KEY (MemberId) REFERENCES master.dbo.Members(Id),
	CONSTRAINT FK_NotiActions_Notifications_NotificationId FOREIGN KEY (NotificationId) REFERENCES master.dbo.Notifications(Id)
);
 CREATE NONCLUSTERED INDEX IX_NotiActions_HomeId ON dbo.NotiActions (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_NotiActions_NotificationId ON dbo.NotiActions (  NotificationId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
