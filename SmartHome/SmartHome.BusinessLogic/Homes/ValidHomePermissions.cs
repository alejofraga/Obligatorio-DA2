namespace SmartHome.BusinessLogic.Homes;

/// <summary>
/// Enum representing the valid permissions that can be granted to a home in the SmartHome system.
/// </summary>
public enum ValidHomePermissions
{
    /// <summary>
    /// Permission to list devices in a home.
    /// </summary>
    ListDevices,

    /// <summary>
    /// Permission to add a new device to a home.
    /// </summary>
    AddDevice,

    /// <summary>
    /// Permission to receive notifications.
    /// </summary>
    ReceiveNotifications,

    /// <summary>
    /// Permission to grant permissions.
    /// </summary>
    GrantHomePermissions,

    /// <summary>
    /// Permission to add rooms.
    /// </summary>
    CreateRoom,

    /// <summary>
    /// Permission to set a room to a device.
    /// </summary>
    AddDeviceToRoom,

    /// <summary>
    /// Permission to set a name to a device.
    /// </summary>
    ChangeDeviceName,

    /// <summary>
    /// Permission to change home name.
    /// </summary>
    ChangeHomeName,
}
