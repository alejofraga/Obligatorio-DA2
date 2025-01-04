using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Test;
public class ImportController_Test
{
    [TestClass]
    public class ImportControllerTests
    {
        private Mock<IDeviceService> _deviceServiceMock = null!;
        private ImportController _controller = null!;
        private Mock<HttpContext> _httpContext = null!;

        [TestInitialize]
        public void Setup()
        {
            _deviceServiceMock = new Mock<IDeviceService>();
            _httpContext = new Mock<HttpContext>();
            _controller = new ImportController(_deviceServiceMock.Object);
        }

        [TestMethod]
        public void GetImporters_ReturnsOkResult_WithImporterNames()
        {
            var importerNames = new List<string> { "Importer1", "Importer2" };
            var importerObjects = importerNames.Select(name => new { Name = name }).ToList();
            _deviceServiceMock.Setup(service => service.GetImporterNames()).Returns(importerNames);
            var result = _controller.GetImporters() as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            var data = result.Value.GetType().GetProperty("Data").GetValue(result.Value) as IEnumerable<object>;
            Assert.IsNotNull(data);
            var dataList = data.ToList();
            Assert.AreEqual(importerObjects.Count, dataList.Count);
            for (var i = 0; i < importerObjects.Count; i++)
            {
                Assert.AreEqual(importerObjects[i].Name, dataList[i].GetType().GetProperty("Name").GetValue(dataList[i]));
            }
        }

        [TestMethod]
        public void GetImporterParams_ReturnsOkResult_WithImporterParams()
        {
            var importerParams = new Dictionary<string, string> { { "Param1", "Value1" } };
            _deviceServiceMock.Setup(service => service.GetImporterParams("Importer1")).Returns(importerParams);
            var result = _controller.GetImporterParams("Importer1") as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(importerParams, result.Value.GetType().GetProperty("Data").GetValue(result.Value));
        }

        [TestMethod]
        public void ImportDevices_ReturnsOkResult_WithSuccessMessage()
        {
            var request = new ImportRequest { Params = new Dictionary<string, string> { { "Param1", "Value1" } } };
            _deviceServiceMock.Setup(service => service.ImportDevices("Importer1", request.Params, It.IsAny<User>()));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext.Object
            };
            var loggedUser = new User { Name = "alejo", Lastname = "fraga", Email = "af282542@fi365.ort.edu.uy", Password = "pa$s32Word" };
            _httpContext
                .Setup(hc => hc.Items[Item.UserLogged])
                .Returns(loggedUser);
            var result = _controller.ImportDevices(request, "Importer1") as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Devices imported successfully!", result.Value.GetType().GetProperty("Message").GetValue(result.Value));
        }
    }
}
