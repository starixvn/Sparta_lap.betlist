namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using Dashboard;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class CustomerBalanceControllerTest
    {
        private ISimpleWidgetService _customerBalanceService;
        private CustomerBalanceController _customerBalanceController;

        [OneTimeSetUp]
        public void Setup()
        {
            _customerBalanceService = Substitute.For<ISimpleWidgetService>();
            _customerBalanceController = new CustomerBalanceController(_customerBalanceService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _customerBalanceService = null;
            _customerBalanceController = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "CustomerBalance" };
            _customerBalanceService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _customerBalanceController.ActionContext = mockActionContext;

            // Action
            _customerBalanceController.GetData(metaModel);

            // Assert
            Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
            _customerBalanceService.Received().GetData(metaModel);
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel();
            _customerBalanceService.GetData(metaModel).Returns(expected);

            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 } }
            };
            _customerBalanceController.ActionContext = mockActionContext;

            // Action
            var result = _customerBalanceController.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}