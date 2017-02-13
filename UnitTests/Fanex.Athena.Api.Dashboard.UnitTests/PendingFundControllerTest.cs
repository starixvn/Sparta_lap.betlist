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
        private PendingFundController _pendingFundController;

        [OneTimeSetUp]
        public void Setup()
        {
            _pendingFundService = Substitute.For<ISimpleWidgetService>();
            _pendingFundController = new PendingFundController(_pendingFundService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _pendingFundService = null;
            _pendingFundController = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "PendingFund" };
            _pendingFundService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _pendingFundController.ActionContext = mockActionContext;

            // Action
            _pendingFundController.GetData(metaModel);

            // Assert
            Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
            _pendingFundService.Received().GetData(metaModel);
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel();
            _pendingFundService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _pendingFundController.ActionContext = mockActionContext;

            // Action
            var result = _pendingFundController.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}