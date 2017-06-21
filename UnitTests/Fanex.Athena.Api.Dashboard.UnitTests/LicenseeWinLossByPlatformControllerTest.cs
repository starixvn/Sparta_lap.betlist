namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using Dashboard;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class LicenseeWinLossByPlatformControllerTest
    {
        private IWidgetByPlatformService _licenseeWinLossByPlatformService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _licenseeWinLossByPlatformService = Substitute.For<IWidgetByPlatformService>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _licenseeWinLossByPlatformService = null;
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("CLMV")]
        public void GetData_InvalidBaseCurrency_ReturnEmptyResult(string baseCurrency)
        {
            // Arrange
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = baseCurrency
            };

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.Data, Is.Null);
                    Assert.That(result.Total, Is.Zero);
                    _licenseeWinLossByPlatformService.DidNotReceive().GetData(metaModel);
                });
            }
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel();
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "PendingFund" };
            _licenseeWinLossByPlatformService.GetData(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                controller.GetData(metaModel);

                // Assert
                Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
                _licenseeWinLossByPlatformService.Received().GetData(metaModel);
            }
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
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };
            _licenseeWinLossByPlatformService.GetData(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

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

        private LicenseeWinLossByPlatformController CreateController()
        {
            var controller = new LicenseeWinLossByPlatformController(_licenseeWinLossByPlatformService);
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };

            controller.ActionContext = mockActionContext;

            return controller;
        }
    }
}