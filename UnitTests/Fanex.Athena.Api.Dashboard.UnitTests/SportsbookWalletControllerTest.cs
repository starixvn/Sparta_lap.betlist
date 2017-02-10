namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using Dashboard;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class SportsbookWalletControllerTest
    {
        private ISimpleWidgetService _sportsbookWalletService;
        private CustomerBalanceController _sportsbookWalletController;

        [OneTimeSetUp]
        public void Setup()
        {
            _sportsbookWalletService = Substitute.For<ISimpleWidgetService>();
            _sportsbookWalletController = new CustomerBalanceController(_sportsbookWalletService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sportsbookWalletService = null;
            _sportsbookWalletController = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel();
            _sportsbookWalletService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _sportsbookWalletController.ActionContext = mockActionContext;

            // Action
            var result = _sportsbookWalletController.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}