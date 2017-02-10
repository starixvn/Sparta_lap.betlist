namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class TurnoverByPlatformControllerTest
    {
        private IWidgetByPlatformService _turnoverByPlatformService;

        private TurnoverByPlatformController _turnoverByPlatformController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _turnoverByPlatformService = Substitute.For<IWidgetByPlatformService>();
            _turnoverByPlatformController = Substitute.ForPartsOf<TurnoverByPlatformController>(_turnoverByPlatformService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _turnoverByPlatformService = null;
            _turnoverByPlatformController = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                   new KeyValuePair<string, decimal>("Asia", 100M),
                   new KeyValuePair<string, decimal>("China", 300M),
                   new KeyValuePair<string, decimal>("WAP", 800M),
                   new KeyValuePair<string, decimal>("Smartsite", 700M),
                   new KeyValuePair<string, decimal>("Others", 1700M)
                }
            };
            var metaModel = new DashboardBaseModel();
            _turnoverByPlatformService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _turnoverByPlatformController.ActionContext = mockActionContext;

            // Action
            var result = _turnoverByPlatformController.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(5));
                Assert.That(result.Data[0].Key, Is.EqualTo(expected.Data[0].Key));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
                Assert.That(result.Data[2].Key, Is.EqualTo(expected.Data[2].Key));
                Assert.That(result.Data[2].Value, Is.EqualTo(expected.Data[2].Value));
                Assert.That(result.Data[4].Key, Is.EqualTo(expected.Data[4].Key));
                Assert.That(result.Data[4].Value, Is.EqualTo(expected.Data[4].Value));
            });
        }
    }
}