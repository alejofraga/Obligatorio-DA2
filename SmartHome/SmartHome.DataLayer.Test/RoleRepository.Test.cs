using FluentAssertions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class RoleRepository_Test
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private Repository<SystemPermission> _systemPermissionRepository = null!;
    private RoleRepository _roleRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _roleRepository = new RoleRepository(_context);
        _systemPermissionRepository = new Repository<SystemPermission>(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetRoleByName_WhenRoleExists_ShouldGetRole()
    {
        var expectedName = "Administrator";
        var role = new Role() { Name = expectedName };
        _roleRepository.Add(role);

        var roleAdded = _roleRepository.GetOrDefault(r => r.Name == expectedName);

        roleAdded.Should().NotBeNull();
    }

    [TestMethod]
    public void AddPermissionToRole_WhenRoleExists_ShouldAddPermissionToRole()
    {
        var expectedName = "Administrator";
        var role = new Role() { Name = expectedName };
        _roleRepository.Add(role);
        var permission = new SystemPermission() { Name = "CreateHomeOwnerAccount" };
        _systemPermissionRepository.Add(permission);
        _roleRepository.AddPermissionToRole(role, permission);

        var rolePermissions = _roleRepository.GetRolePermissions(role);

        rolePermissions.Should().NotBeNull();
        rolePermissions.Should().Contain(permission);
    }
}
