namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Core.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public partial class MenuControllerTest : BaseControllerHelperTest
    {
        private IFunctionService _functionService;

        [SetUp]
        public void SetUp()
        {
            _functionService = Substitute.For<IFunctionService>();

            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            _functionService = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _functionService.Functions.Returns(new List<Function>());

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                StringAssert.Contains("Menu/Index.cshtml", result.ViewName);
            }
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.UserSession.FunctionIdList = new List<int> { 1, 2, 3 };
                _functionService.Functions.Returns(new List<Function>());
                AppSettings.FakeLicenseeList.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                // Action
                var result = controller.Index() as ViewResult;
                var viewModel = result.Model as MenuViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.IsAllowedCustonmerCommission, Is.EqualTo(DefaultLicenseeSession.IsAllowedCustomerComm));
                    Assert.That(viewModel.IsInternalLogin, Is.EqualTo(DefaultUserSession.IsInternal));
                    Assert.That(viewModel.IsAllowedManagementMember, Is.EqualTo(false));
                    Assert.That(viewModel.IsFakeLicensee, Is.EqualTo(true));
                    Assert.That(viewModel.UserFunctions, Is.Empty);
                });
            }
        }

        private MenuController CreateController()
        {
            return new MenuController(LangService, SessionService, AppSettings, ClientViewHelper, _functionService);
        }
    }
}