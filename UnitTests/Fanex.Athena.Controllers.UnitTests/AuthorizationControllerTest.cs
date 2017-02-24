namespace Fanex.Athena.Controllers.UnitTests
{
    using Base.UnitTests;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Services;

    [TestFixture]
    public partial class AuthorizationControllerTest : BaseControllerTest
    {
        private IMaintenanceService _maintenanceService;
        private IUserService _userService;
        private IFormsAuthenticationService _formAuthenticationService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _maintenanceService = Substitute.For<IMaintenanceService>();
            _userService = Substitute.For<IUserService>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _maintenanceService = null;
            _userService = null;
            _formAuthenticationService = null;
        }

        private AuthorizationController CreateController()
        {
            var controller = new AuthorizationController(
                AppSettings,
                LangService,
                SessionService,
                AppSettings,
                _maintenanceService,
                _userService,
                _formAuthenticationService);

            controller.ControllerContext = this.ControllerContext;

            return controller;
        }
    }
}