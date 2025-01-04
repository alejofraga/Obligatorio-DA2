namespace Importer;
public interface IImporter
{
    List<DeviceDto> Import(Dictionary<string, string> parameters);
    Dictionary<string, string> GetImporterParams();
}
