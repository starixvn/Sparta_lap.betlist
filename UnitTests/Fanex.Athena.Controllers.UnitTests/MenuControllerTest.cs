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

        private MenuController _controller;

        [SetUp]
        public void SetUp()
        {
            _functionService = Substitute.For<IFunctionService>();

            SetUpDefaultFakeData();

            _controller = new MenuController(LangService, SessionService, AppSettings, _functionService);
        }

        [TearDown]
        public void TearDown()
        {
            _functionService = null;

            _controller.Dispose();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            // Arrange
            _functionService.Functions.Returns(new List<Function>());

            // Action
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/Menu/Index.cshtml"));
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewModel()
        {
            // Arrange
            SessionService.UserSession.FunctionIdList = new List<int> { 1, 2, 3 };
            _functionService.Functions.Returns(new List<Function>());
            AppSettings.FakeLicenseeList.Returns(new int[] { DefaultLicenseeSession.LicenseeId });

            // Action
            var result = _controller.Index() as ViewResult;
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
}