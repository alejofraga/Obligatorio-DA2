namespace SmartHome.BusinessLogic.Users;

/// <summary>
/// Enum representing the valid roles that can be assigned to a role in the SmartHome system.
/// </summary>
public enum ValidSystemPermissions
{
    /// <summary>
    /// Permission to create an admin and company owner.
    /// </summary>
    CreateUserWithRole,

    /// <summary>
    /// Permission to create a home.
    /// </summary>
    CreateHome,

    /// <summary>
    /// Permission to get homes.
    /// </summary>
    GetHomes,

    /// <summary>
    /// Permission to get users.
    /// </summary>
    GetUsers,

    /// <summary>
    /// Permission to add a member.
    /// </summary>
    AddMember,

    /// <summary>
    /// Permission to be ahome member.
    /// </summary>
    BeHomeMember,

    /// <summary>
    /// Permission to get members.
    /// </summary>
    GetMembers,

    /// <summary>
    /// Permission to create a company.
    /// </summary>
    CreateCompany,

    /// <summary>
    /// Permission to get companies.
    /// </summary>
    GetCompanies,

    /// <summary>
    /// Permission to delete an admin.
    /// </summary>
    DeleteAdmin,

    /// <summary>
    /// Permission to create a device.
    /// </summary>
    CreateDevice,
}
