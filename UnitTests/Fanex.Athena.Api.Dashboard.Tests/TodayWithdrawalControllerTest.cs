namespace Fanex.Athena.Api.Dashboard.Tests
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
        private TodayWithdrawalController _todayWithdrawalController;

        [OneTimeSetUp]
        public void Setup()
        {
            _todayWithdrawalService = Substitute.For<ISimpleWidgetService>();
            _todayWithdrawalController = new TodayWithdrawalController(_todayWithdrawalService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithdrawalService = null;
            _todayWithdrawalController = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var metaModel = new DashboardBaseModel();
            _todayWithdrawalService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _todayWithdrawalController.ActionContext = mockActionContext;

            // Action
            var result = _todayWithdrawalController.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}