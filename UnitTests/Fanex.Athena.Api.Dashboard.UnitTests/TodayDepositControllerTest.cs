namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TodayDepositControllerTest
    {
        private ISimpleWidgetDataAccess _todayDepositService;

        [OneTimeSetUp]
        public void Setup()
        {
            _todayDepositService = Substitute.For<ISimpleWidgetDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayDepositService = null;
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
                _todayDepositService.DidNotReceive().GetData(metaModel);
            }
        }

        [Test]
        public void Index_WhenCall_PassCorrectParam()
        {
            // Arrange
            var expected = 2500000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "TodayDeposit" };
            _todayDepositService.GetData(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                controller.GetData(metaModel);

                // Assert
                Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
                _todayDepositService.Received().GetData(metaModel);
            }
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 2500000M;
            _todayDepositService.GetData(Arg.Any<DashboardBaseModel>()).Returns(expected);
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        private TodayDepositController CreateController()
        {
            var controller = new TodayDepositController(_todayDepositService);
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