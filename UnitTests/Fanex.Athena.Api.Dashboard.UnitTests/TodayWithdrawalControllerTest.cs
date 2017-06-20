namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class TodayWithdrawalControllerTest
    {
        private ISimpleWidgetService _todayWithdrawalService;

        [OneTimeSetUp]
        public void Setup()
        {
            _todayWithdrawalService = Substitute.For<ISimpleWidgetService>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithdrawalService = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var expected = 500000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "PendingFund" };
            _todayWithdrawalService.GetData(metaModel).Returns(expected);
            using (var controller = CreateController())
            {
                // Action
                controller.GetData(metaModel);

                // Assert
                Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
                _todayWithdrawalService.Received().GetData(metaModel);
            }
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };
            _todayWithdrawalService.GetData(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        private TodayWithdrawalController CreateController()
        {
            var controller = new TodayWithdrawalController(_todayWithdrawalService);
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