namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PendingFundControllerTest
    {
        private ISimpleWidgetDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.For<ISimpleWidgetDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
        }

        [TestCase("CLMV")]
        public void GetData_InvalidBaseCurrency_ReturnZeroAndDoNotCallService(string baseCurrency)
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
                Assert.That(result, Is.Zero);
                _dataAccess.DidNotReceive().GetData(metaModel);
            }
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            const decimal Expected = 1570000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "PendingFund" };
            _dataAccess.GetData(metaModel).Returns(Expected);

            using (var controller = CreateController())
            {
                // Action
                controller.GetData(metaModel);

                // Assert
                Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
                _dataAccess.Received().GetData(metaModel);
            }
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const decimal Expected = 1570000M;
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };
            _dataAccess.GetData(metaModel).Returns(Expected);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(Expected));
            }
        }

        private PendingFundController CreateController()
        {
            var controller = new PendingFundController(_dataAccess);
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 }, { "currencyIDList", "1" } }
            };

            controller.ActionContext = mockActionContext;

            return controller;
        }
    }
}