namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeWinLossByPlatformControllerTest
    {
        private ILicenseeDataByPlatformDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ILicenseeDataByPlatformDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
        }

        [TestCase("CLMV")]
        public void GetWinLoss_InvalidBaseCurrency_ReturnNullData(string baseCurrency)
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
                });
            }
        }

        [Test]
        public void GetWinLoss_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                    new KeyValuePair<string, decimal>("Asia", 100M),
                    new KeyValuePair<string, decimal>("China", 300M),
                    new KeyValuePair<string, decimal>("WAP", 800M),
                    new KeyValuePair<string, decimal>("Mobile", 700M),
                    new KeyValuePair<string, decimal>("Others", 1700M)
                }
            };
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };
            _dataAccess.GetWinLoss(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.Data.Count, Is.EqualTo(5));
                    Assert.That(result.Data, Is.EqualTo(expected.Data));
                });
            }
        }

        private LicenseeWinLossByPlatformController CreateController()
        {
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 }, { "currencyIDList", "1" } }
            };

            return new LicenseeWinLossByPlatformController(_dataAccess)
            {
                ActionContext = mockActionContext
            };
        }
    }
}