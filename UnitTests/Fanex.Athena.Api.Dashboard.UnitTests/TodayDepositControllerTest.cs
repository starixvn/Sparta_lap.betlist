namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class TodayDepositControllerTest
    {
        private ISimpleWidgetService _todayDepositService;
        private TodayDepositController _todayDepositController;

        [OneTimeSetUp]
        public void Setup()
        {
            _todayDepositService = Substitute.For<ISimpleWidgetService>();
            _todayDepositController = new TodayDepositController(_todayDepositService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayDepositService = null;
            _todayDepositController = null;
        }

        [Test]
        public void Index_WhenCall_PassCorrectParam()
        {
            // Arrange
            var expected = 2500000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "TodayDeposit" };
            _todayDepositService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _todayDepositController.ActionContext = mockActionContext;

            // Action
            _todayDepositController.GetData(metaModel);

            // Assert
            Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
            _todayDepositService.Received().GetData(metaModel);
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 2500000M;
            _todayDepositService.GetData(Arg.Any<DashboardBaseModel>()).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _todayDepositController.ActionContext = mockActionContext;

            // Action
            var result = _todayDepositController.GetData(new DashboardBaseModel());

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}