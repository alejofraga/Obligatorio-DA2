using SmartHome.BusinessLogic.Homes;

namespace SmartHome.BusinessLogic.Test.Homes;

[TestClass]
public class SensorHardware_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateSensorHardware()
    {
        var sensorHardware = new SensorHardware()
        {
            IsOpen = true,
            HomeId = Guid.NewGuid(),
            DeviceModelNumber = "1"
        };

        var result = sensorHardware.IsOpen;
        Assert.IsTrue(result);
    }
}
