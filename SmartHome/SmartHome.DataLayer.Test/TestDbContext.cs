using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;
public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<Coordinates> Coordinates { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Home> Homes { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<SystemPermission> SystemPermissions { get; set; } = null!;
    public DbSet<DummyEntity> DummyEntities { get; set; } = null!;
    public DbSet<Hardware> Hardwares { get; set; } = null!;
    public DbSet<HomePermission> HomePermissions { get; set; } = null!;
    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Camera> Cameras { get; set; } = null!;
    public DbSet<NotiAction> NotiActions { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
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
        modelBuilder.Entity<DummyEntity>()
            .HasKey(e => e.Value);
        modelBuilder.Entity<NotiAction>()
            .HasKey(na => new { na.MemberId, na.NotificationId });
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.ModelNumber)
            .IsUnique();
        modelBuilder.Entity<HomePermission>()
            .HasKey(hp => new { hp.Name, hp.MemberId });
    }

    public sealed class DummyEntity
    {
        public string Value { get; set; } = null!;
    }
}
