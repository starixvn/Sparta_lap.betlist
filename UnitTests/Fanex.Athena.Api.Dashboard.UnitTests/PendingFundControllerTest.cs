namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class PendingFundControllerTest
    {
        private ISimpleWidgetService _pendingFundService;

        [OneTimeSetUp]
        public void Setup()
        {
            _pendingFundService = Substitute.For<ISimpleWidgetService>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _pendingFundService = null;
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("CLMV")]
        public void GetData_InvalidBaseCurrency_ReturnZero(string baseCurrency)
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
                _pendingFundService.DidNotReceive().GetData(metaModel);
            }
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            const decimal Expected = 1570000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "PendingFund" };
            _pendingFundService.GetData(metaModel).Returns(Expected);

            using (var controller = CreateController())
            {
                // Action
                controller.GetData(metaModel);

                // Assert
                Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
                _pendingFundService.Received().GetData(metaModel);
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
            _pendingFundService.GetData(metaModel).Returns(Expected);

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
            var controller = new PendingFundController(_pendingFundService);
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