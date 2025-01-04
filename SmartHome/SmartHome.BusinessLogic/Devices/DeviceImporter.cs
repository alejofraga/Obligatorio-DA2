using System.Reflection;
using Importer;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Devices;

public class DeviceImporter : IDeviceImporter
{
    private readonly string folderName = "Importers";

    public List<DeviceDto> Import(string importerName, Dictionary<string, string> parameters, User user)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
        const string searchPattern = "*.dll";
        var dllFiles = Directory.GetFiles(path, searchPattern);

        var assemblies = dllFiles.Select(Assembly.LoadFrom).ToArray();
        var importerType = assemblies
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => typeof(IImporter).IsAssignableFrom(t) && t.Name == importerName);
        if (importerType == null)
        {
            throw new InvalidOperationException("Importer not found");
        }

        var importer = (IImporter?)Activator.CreateInstance(importerType);
        if (importer == null)
        {
            throw new InvalidOperationException("caould not create instance of importer");
        }

        return importer.Import(parameters);
    }

    public Dictionary<string, string> GetImporterParams(string importerName)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
        const string searchPattern = "*.dll";
        var dllFiles = Directory.GetFiles(path, searchPattern);

        var assemblies = dllFiles.Select(Assembly.LoadFrom).ToArray();
        var importerType = assemblies
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => typeof(IImporter).IsAssignableFrom(t) && t.Name == importerName);
        if (importerType == null)
        {
            throw new InvalidOperationException("Importer not found");
        }

        var importer = (IImporter?)Activator.CreateInstance(importerType);
        if (importer == null)
        {
            throw new InvalidOperationException("caould not create instance of importer");
        }

        return importer.GetImporterParams();
    }

    public List<string> GetImporterNames()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
        const string searchPattern = "*.dll";
        var dllFiles = Directory.GetFiles(path, searchPattern);

        var assemblies = dllFiles.Select(Assembly.LoadFrom).ToArray();
        var importerTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IImporter).IsAssignableFrom(t))
            .ToList();

        if (!importerTypes.Any())
        {
            throw new InvalidOperationException("No importers found");
        }

        return importerTypes.Select(t => t.Name).ToList();
    }
}
