namespace SmartHome.BusinessLogic.Devices;

public class Camera() : Device
{
    private bool? _hasMovementDetection;
    private bool? _hasPersonDetection;
    private bool? _isOutdoor;
    private bool? _isIndoor;
    public required bool? HasMovementDetection
    {
        get => _hasMovementDetection;
        set
        {
            AssertIsNotEmpty(value, "HasMovementDetection", "Movement detection indicator");
            _hasMovementDetection = value;
        }
    }

    public required bool? HasPersonDetection
    {
        get => _hasPersonDetection;
        set
        {
            AssertIsNotEmpty(value, "HasPersonDetection", "Person detection indicator");
            _hasPersonDetection = value;
        }
    }

    public bool? IsOutdoor
    {
        get => _isOutdoor;
        set
        {
            AssertIsNotEmpty(value, "IsOutdoor", "Outdoor availability indicator");
            _isOutdoor = value;
        }
    }

    public bool? IsIndoor
    {
        get => _isIndoor;
        set
        {
            AssertIsNotEmpty(value, "IsIndoor", "Indoor availability indicator");
            _isIndoor = value;
        }
    }

    public Camera(bool? isOutdoor, bool? isIndoor)
        : this()
    {
        IsOutdoor = isOutdoor;
        IsIndoor = isIndoor;
        ValidateConsistency();
        DeviceTypeName = ValidDeviceTypes.Camera.ToString();
    }

    private void ValidateConsistency()
    {
        if (IsOutdoor == false && IsIndoor == false)
        {
            throw new ArgumentException("A camera must be at least indoor or outdoor");
        }
    }

    private static void AssertIsNotEmpty(bool? value, string attributeName, string attributeAlias)
    {
        if (value == null)
        {
            throw new ArgumentNullException(attributeName, $"{attributeAlias} cannot be empty");
        }
    }
}
