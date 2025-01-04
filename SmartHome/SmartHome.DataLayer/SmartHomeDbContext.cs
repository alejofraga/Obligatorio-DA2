using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer;

[ExcludeFromCodeCoverage]
public class SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options) : DbContext(options)
{
    public DbSet<Coordinates> Coordinates { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Home> Homes { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<SystemPermission> SystemPermissions { get; set; } = null!;
    public DbSet<HomePermission> HomePermissions { get; set; } = null!;
    public DbSet<NotiAction> NotiActions { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Hardware> Hardwares { get; set; } = null!;
    public DbSet<LampHardware> LampHardwares { get; set; } = null!;
    public DbSet<SensorHardware> SensorHardwares { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Camera> Cameras { get; set; } = null!;
    public DbSet<DeviceType> DeviceTypes { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigSchema(modelBuilder);
        ConfigSeedData(modelBuilder);
    }

    private void ConfigSeedData(ModelBuilder modelBuilder)
    {
        var initialAdmin = new User()
        {
            Email = "sa@smarthome.com",
            Name = "System",
            Lastname = "Admin",
            Password = "sA$a1234",
            ProfilePicturePath = "path"
        };

        var roleAdmin = new Role() { Name = nameof(ValidUserRoles.Admin) };
        var roleHomeOwner = new Role() { Name = nameof(ValidUserRoles.HomeOwner) };
        var roleCompanyOwner = new Role() { Name = nameof(ValidUserRoles.CompanyOwner) };

        var createUserWithRolePermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.CreateUserWithRole) };
        var beHomeMemberPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.BeHomeMember) };
        var createHomePermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.CreateHome) };
        var getUsersPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.GetUsers) };
        var addMemberPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.AddMember) };
        var getMembersPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.GetMembers) };
        var createCompanyPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.CreateCompany) };
        var getCompaniesPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.GetCompanies) };
        var deleteAdminPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.DeleteAdmin) };
        var createDevice = new SystemPermission() { Name = nameof(ValidSystemPermissions.CreateDevice) };
        var getHomesPermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.GetHomes) };

        modelBuilder.Entity<User>().HasData(initialAdmin);

        modelBuilder.Entity<Role>().HasData(roleAdmin);
        modelBuilder.Entity<Role>().HasData(roleHomeOwner);
        modelBuilder.Entity<Role>().HasData(roleCompanyOwner);

        modelBuilder.Entity<SystemPermission>().HasData(getUsersPermission);
        modelBuilder.Entity<SystemPermission>().HasData(beHomeMemberPermission);
        modelBuilder.Entity<SystemPermission>().HasData(createUserWithRolePermission);
        modelBuilder.Entity<SystemPermission>().HasData(createHomePermission);
        modelBuilder.Entity<SystemPermission>().HasData(addMemberPermission);
        modelBuilder.Entity<SystemPermission>().HasData(getMembersPermission);
        modelBuilder.Entity<SystemPermission>().HasData(createCompanyPermission);
        modelBuilder.Entity<SystemPermission>().HasData(getCompaniesPermission);
        modelBuilder.Entity<SystemPermission>().HasData(deleteAdminPermission);
        modelBuilder.Entity<SystemPermission>().HasData(createDevice);
        modelBuilder.Entity<SystemPermission>().HasData(getHomesPermission);

        var initialAdminWithRoleAdmin = new Dictionary<string, object>
        {
            { "RoleName", roleAdmin.Name }, { "UserEmail", initialAdmin.Email }
        };

        modelBuilder.Entity("UserRole").HasData(initialAdminWithRoleAdmin);

        var adminWithCreateAdminPermission = new Dictionary<string, object>
        {
            { "RoleName", roleAdmin.Name }, { "SystemPermissionName", createUserWithRolePermission.Name }
        };
        var homeOwnerWithCreateHomePermission = new Dictionary<string, object>
        {
            { "RoleName", roleHomeOwner.Name }, { "SystemPermissionName", createHomePermission.Name },
        };
        var homeOwnerWithBeHomeMemberPermission = new Dictionary<string, object>
        {
            { "RoleName", roleHomeOwner.Name }, { "SystemPermissionName", beHomeMemberPermission.Name },
        };
        var homeOwnerWithGetHomesPermission = new Dictionary<string, object>
        {
            { "RoleName", roleHomeOwner.Name }, { "SystemPermissionName", getHomesPermission.Name },
        };
        var homeOwnerWithAddMemberPermission = new Dictionary<string, object>
        {
            { "RoleName", roleHomeOwner.Name }, { "SystemPermissionName", addMemberPermission.Name }
        };
        var homeOwnerWithGetMembersPermission = new Dictionary<string, object>
        {
            { "RoleName", roleHomeOwner.Name }, { "SystemPermissionName", getMembersPermission.Name }
        };
        var adminWithGetUsersPermission = new Dictionary<string, object>
        {
            { "RoleName", roleAdmin.Name }, { "SystemPermissionName", getUsersPermission.Name }
        };
        var companyOwnerWithCreateDevicePermission = new Dictionary<string, object>
        {
            { "RoleName", roleCompanyOwner.Name }, { "SystemPermissionName", createDevice.Name }
        };
        var companyOwnerWithCreateCompanyPermission = new Dictionary<string, object>
        {
            { "RoleName", roleCompanyOwner.Name }, { "SystemPermissionName", createCompanyPermission.Name }
        };
        var adminWithGetCompaniesPermission = new Dictionary<string, object>
        {
            { "RoleName", roleAdmin.Name }, { "SystemPermissionName", getCompaniesPermission.Name }
        };
        var adminWithDeleteAdminPermission = new Dictionary<string, object>
        {
            { "RoleName", roleAdmin.Name }, { "SystemPermissionName", deleteAdminPermission.Name }
        };

        modelBuilder.Entity("RoleSystemPermission").HasData(homeOwnerWithCreateHomePermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(homeOwnerWithBeHomeMemberPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(homeOwnerWithAddMemberPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(adminWithCreateAdminPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(adminWithGetUsersPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(homeOwnerWithGetMembersPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(companyOwnerWithCreateCompanyPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(adminWithGetCompaniesPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(adminWithDeleteAdminPermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(companyOwnerWithCreateDevicePermission);
        modelBuilder.Entity("RoleSystemPermission").HasData(homeOwnerWithGetHomesPermission);

        var cameraType = new DeviceType() { Name = ValidDeviceTypes.Camera.ToString() };
        var sensorType = new DeviceType() { Name = ValidDeviceTypes.Sensor.ToString() };
        var movementSensorType = new DeviceType() { Name = ValidDeviceTypes.MovementSensor.ToString() };
        var lampType = new DeviceType() { Name = ValidDeviceTypes.Lamp.ToString() };

        modelBuilder.Entity<DeviceType>().HasData(cameraType);
        modelBuilder.Entity<DeviceType>().HasData(sensorType);
        modelBuilder.Entity<DeviceType>().HasData(movementSensorType);
        modelBuilder.Entity<DeviceType>().HasData(lampType);
    }

    private void ConfigSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleName"),
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserEmail"));

        modelBuilder.Entity<Role>()
            .HasMany(r => r.SystemPermissions)
            .WithMany(s => s.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "RoleSystemPermission",
                j => j
                    .HasOne<SystemPermission>()
                    .WithMany()
                    .HasForeignKey(
                        "SystemPermissionName"),
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleName"));

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Coordinates>()
            .HasKey(c => new { c.Latitude, c.Longitude });
        modelBuilder.Entity<Location>()
            .HasKey(l => new { l.Address, l.DoorNumber });
        modelBuilder.Entity<NotiAction>()
            .HasKey(na => new { na.MemberId, na.NotificationId });
        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany(u => u.Members)
            .HasForeignKey(m => m.UserEmail);
        modelBuilder.Entity<Hardware>()
            .HasOne(h => h.Device)
            .WithMany(d => d.Hardwares)
            .HasForeignKey(h => h.DeviceModelNumber);
        modelBuilder.Entity<Hardware>()
            .HasOne(h => h.Room)
            .WithMany(r => r.Hardwares)
            .HasForeignKey(h => h.RoomId);
        modelBuilder.Entity<NotiAction>()
            .HasOne(na => na.Member)
            .WithMany(m => m.NotiActions)
            .HasForeignKey(na => na.MemberId);
        modelBuilder.Entity<NotiAction>()
            .HasOne(na => na.Notification)
            .WithMany(n => n.NotiActions)
            .HasForeignKey(na => na.NotificationId);
        modelBuilder.Entity<HomePermission>()
            .HasKey(hp => new { hp.Name, hp.MemberId });
        modelBuilder.Entity<NotiAction>()
            .HasKey(na => new { na.MemberId, na.NotificationId });
        modelBuilder.Entity<Session>()
            .HasOne(m => m.User)
            .WithOne(s => s.Session)
            .HasForeignKey<Session>(s => s.UserEmail);
        modelBuilder.Entity<Device>()
            .HasDiscriminator<string>("DeviceType")
            .HasValue<Device>(nameof(Device))
            .HasValue<Camera>(nameof(Camera));
    }
}
