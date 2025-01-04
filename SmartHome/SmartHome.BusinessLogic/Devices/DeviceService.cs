using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Devices;

public class DeviceService(IDeviceRepository deviceRepository, ICompanyRepository companyRepository, IModelValidator modelValidator, IDeviceImporter deviceImporter) : IDeviceService
{
    public Device AddDevice(CreateBasicDeviceArgs args, User user)
    {
        var userCompany = companyRepository.GetOrDefault(c => c.OwnerEmail.ToUpper() == user.Email!.ToUpper());
        if (userCompany == null)
        {
            throw new InvalidOperationException("User does not have a company");
        }

        if (args.ModelNumber == null)
        {
            throw new ArgumentNullException("ModelNumber", $"Model number cannot be empty");
        }

        var modelValidatorType = userCompany.ValidatorTypeName;

        if (!modelValidator.IsValid(args.ModelNumber!, modelValidatorType))
        {
            throw new ArgumentException($"Model number is not valid according to {modelValidatorType}");
        }

        var device = new Device()
        {
            Name = args!.Name!,
            Description = args.Description!,
            ModelNumber = args.ModelNumber!,
            Photos = args.Photos!,
            CompanyRUT = userCompany.RUT!,
            DeviceTypeName = args.DeviceTypeName!
        };
        if (IsDeviceDuplicated(device))
        {
            throw new InvalidOperationException("Model number already in use");
        }

        deviceRepository.Add(device);
        return device;
    }

    public Camera AddCamera(CreateCameraArgs args, User user)
    {
        var userCompany = companyRepository.GetOrDefault(c => c.OwnerEmail.ToUpper() == user.Email.ToUpper());
        if (userCompany == null)
        {
            throw new InvalidOperationException("User does not have a company");
        }

        if (args.ModelNumber == null)
        {
            throw new ArgumentNullException("ModelNumber", $"Model number cannot be empty");
        }

        var modelValidatorType = userCompany.ValidatorTypeName;

        if (!modelValidator.IsValid(args.ModelNumber!, modelValidatorType))
        {
            throw new ArgumentException($"Model number is not valid according to {modelValidatorType}");
        }

        var camera = new Camera(args!.IsOutdoor, args.IsIndoor)
        {
            Name = args.Name,
            Description = args.Description,
            ModelNumber = args.ModelNumber,
            Photos = args.Photos,
            CompanyRUT = userCompany.RUT,
            HasMovementDetection = args.HasMovementDetection,
            HasPersonDetection = args.HasPersonDetection,
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };
        if (IsDeviceDuplicated(camera))
        {
            throw new InvalidOperationException("Model number already in use");
        }

        deviceRepository.Add(camera);
        return camera;
    }

    private void AddDeviceWithoutValidating(CreateBasicDeviceArgs args, User user)
    {
        var userCompany = companyRepository.GetOrDefault(c => c.OwnerEmail.ToUpper() == user.Email!.ToUpper());
        var device = new Device()
        {
            Name = args!.Name!,
            Description = args.Description!,
            ModelNumber = args.ModelNumber!,
            Photos = args.Photos!,
            CompanyRUT = userCompany.RUT!,
            DeviceTypeName = args.DeviceTypeName!
        };
        deviceRepository.Add(device);
    }

    private void AddCameraWithoutValidating(CreateCameraArgs args, User user)
    {
        var userCompany = companyRepository.GetOrDefault(c => c.OwnerEmail.ToUpper() == user.Email.ToUpper());
        var camera = new Camera(args.IsOutdoor, args.IsIndoor)
        {
            Name = args.Name,
            Description = args.Description,
            ModelNumber = args.ModelNumber,
            Photos = args.Photos,
            CompanyRUT = userCompany.RUT,
            HasMovementDetection = args.HasMovementDetection,
            HasPersonDetection = args.HasPersonDetection,
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };
        deviceRepository.Add(camera);
    }

    private bool IsDeviceDuplicated(Device newDevice)
    {
        return deviceRepository.Exist(s => s.ModelNumber.ToUpper() == newDevice.ModelNumber.ToUpper());
    }

    private bool IsDeviceDuplicated(string modelNumber)
    {
        return deviceRepository.Exist(s => s.ModelNumber.ToUpper() == modelNumber.ToUpper());
    }

    public Device? Get(string modelNumber)
    {
        return deviceRepository.GetOrDefault(device => device.ModelNumber == modelNumber);
    }

    public List<Device> GetDevicesWithFilters(GetDevicesArgs getDevicesArgs)
    {
        var devices = deviceRepository.GetDevicesWithFilters(getDevicesArgs);
        return devices;
    }

    public Dictionary<string, string> GetImporterParams(string importerName)
    {
        return deviceImporter.GetImporterParams(importerName);
    }

    public List<string> GetImporterNames()
    {
        return deviceImporter.GetImporterNames();
    }

    public void ImportDevices(string importerName, Dictionary<string, string> parameters, User user)
    {
        var deviceDtos = deviceImporter.Import(importerName, parameters, user);
        foreach (var deviceDto in deviceDtos)
        {
            while (IsDeviceDuplicated(deviceDto.ModelNumber!))
            {
                deviceDto.ModelNumber += Guid.NewGuid().ToString();
            }

            if (deviceDto.DeviceType == ValidDeviceTypes.Camera.ToString())
            {
                var args = new CreateCameraArgs()
                {
                    Description = "-",
                    HasMovementDetection = deviceDto.HasMovementDetection,
                    HasPersonDetection = deviceDto.HasPersonDetection,
                    IsIndoor = true,
                    IsOutdoor = true,
                    ModelNumber = deviceDto.ModelNumber!,
                    Name = deviceDto.Name!,
                    Photos = deviceDto.Photos!
                };
                AddCameraWithoutValidating(args, user);
            }
            else
            {
                var args = new CreateBasicDeviceArgs()
                {
                    Description = "-",
                    ModelNumber = deviceDto.ModelNumber!,
                    Name = deviceDto.Name!,
                    Photos = deviceDto.Photos!,
                    DeviceTypeName = deviceDto.DeviceType!
                };
                AddDeviceWithoutValidating(args, user);
            }
        }
    }
}
