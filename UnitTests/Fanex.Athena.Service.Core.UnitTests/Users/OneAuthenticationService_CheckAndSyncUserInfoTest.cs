namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Web;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class OneAuthenticationService_CheckAndSyncUserInfoTest
    {
        private OneAuthenticationService _oneAuthenticationService;
        private ISessionService _sessionService;
        private ILicenseeService _licenseeService;
        private ICurrencyService _currencyService;
        private IFormsAuthenticationService _formAuthenticationService;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _currencyService = Substitute.For<ICurrencyService>();
            _userService = Substitute.For<IUserService>();
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _licenseeService.GetLicenseeByName(Arg.Any<string>()).Returns(new Licensee { Id = 1, Name = "test", IsAllowedCustomerComm = true });
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
        }

        [TearDown]
        public void TearDown()
        {
            _oneAuthenticationService = null;
            _sessionService = null;
            _licenseeService = null;
            _currencyService = null;
            _formAuthenticationService = null;
        }

        [Test]
        public void SignIn_NotFoundUser_ReturnErrorNotFoundUser()
        {
            // Arrange
            var tokenInfo = new OneAuthToken { UserSEQ = 1, UserID = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _userService.GetByUsername(tokenInfo.UserID, Arg.Any<int>()).Returns(x => null);
            _oneAuthenticationService = new OneAuthenticationService(_sessionService, _licenseeService, _currencyService, _formAuthenticationService, _userService, null);

            var expectedResult = new ResultInfo { Code = 800, Message = "Not found user" };

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedResult.Code, result.Code);
        }

        [Test]
        public void SignIn_ExistUser_UpdateUserSEOAndCallSetAuthorize()
        {
            // Arrange
            var counter = 0;
            var exptectedUserId = 10;
            var tokenInfo = new OneAuthToken { UserSEQ = 1, UserID = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _userService.GetByUsername(tokenInfo.UserID, Arg.Any<int>()).Returns(x => new User { UserID = exptectedUserId });

            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                _sessionService, _licenseeService, _currencyService, _formAuthenticationService, _userService, null);

            _oneAuthenticationService.When(x => x.SetAuthorize(tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(tokenInfo)).Do(x => counter++);

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Assert
            _oneAuthenticationService.Received().SetAuthorize(Arg.Is<OneAuthToken>(tokenInfo));
            Assert.AreEqual(exptectedUserId, tokenInfo.UserSEQ);
            Assert.AreEqual(1, counter);
        }
    }
}