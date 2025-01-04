using SmartHome.BusinessLogic.Homes;

namespace SmartHome.BusinessLogic.Test.Homes;

[TestClass]
public class LampHardware_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateLampHardware()
    {
        var lampHardware = new LampHardware()
        {
            IsOn = true,
            HomeId = Guid.NewGuid(),
            DeviceModelNumber = "1"
        };

        var result = lampHardware.IsOn;
        Assert.IsTrue(result);
    }
}
