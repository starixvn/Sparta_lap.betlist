namespace Fanex.Athena.Controllers.Customer.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using CustomerLog;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    public partial class CustomerLogControllerTest : BaseControllerHelperTest
    {
        private IProductService _productService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            SetUpDefaultFakeData();
            _productService = Substitute.For<IProductService>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();
            _productService = null;
        }

        [Test]
        public void Index_WhenCall_ReturnLogInLogView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ExpectedViewName = "Customer/CustomerLog/LogInLog/Main.cshtml";

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                StringAssert.Contains(ExpectedViewName, result.ViewName);
            }
        }

        private CustomerLogController CreateController()
        {
            return new CustomerLogController(LangService, SessionService, AppSettings, ClientViewHelper, _productService);
        }
    }
}