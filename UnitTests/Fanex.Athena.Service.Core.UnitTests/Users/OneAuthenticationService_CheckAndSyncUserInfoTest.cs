namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Web;
    using Core.Users.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Core.Users.Interfaces;
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
        private ILicenseeDataAccess _licenseeDataAccess;
        private ICurrencyDataAccess _currencyDataAccess;
        private IFormsAuthenticationService _formAuthenticationService;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _currencyDataAccess = Substitute.For<ICurrencyDataAccess>();
            _userService = Substitute.For<IUserService>();
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _licenseeDataAccess.GetLicenseeByName(Arg.Any<string>()).Returns(new Licensee { Id = 1, Name = "test", IsAllowedCustomerComm = true });
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
        }

        [TearDown]
        public void TearDown()
        {
            _oneAuthenticationService = null;
            _sessionService = null;
            _licenseeDataAccess = null;
            _currencyDataAccess = null;
            _formAuthenticationService = null;
        }

        [Test]
        public void SignIn_NotFoundUser_ReturnErrorNotFoundUser()
        {
            // Arrange
            var currencies = new OneAuthCurrency[] { new OneAuthCurrency() };
            var permissions = new OneAuthPermission[] { new OneAuthPermission() };
            var tokenInfo = new OneAuthToken
            {
                UserSEQ = 1,
                UserID = "pgmAlex",
                SiteName = "spondemo",
                Currency = currencies,
                Permissions = permissions,
                IsAdminstrator = true
            };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _userService.GetByUsername(tokenInfo.UserID, true, Arg.Any<int>()).Returns(x => null);
            _oneAuthenticationService = new OneAuthenticationService(
                _sessionService,
                _licenseeDataAccess,
                _currencyDataAccess,
                _formAuthenticationService,
                _userService,
                null);

            var expectedResult = new ResultInfo { Code = 800, Message = "Not found user" };

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedResult.Code, result.Code);
        }

        [Test]
        public void SignIn_ExistUser_CallSetAuthorize()
        {
            // Arrange
            var counter = 0;
            var currencies = new OneAuthCurrency[] { new OneAuthCurrency() };
            var permissions = new OneAuthPermission[] { new OneAuthPermission() };
            var tokenInfo = new OneAuthToken
            {
                UserID = "pgmAlex",
                SiteName = "spondemo",
                Permissions = permissions,
                Currency = currencies
            };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _userService.GetByUsername(tokenInfo.UserID, true, Arg.Any<int>()).Returns(x => new User());

            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                _sessionService, _licenseeDataAccess, _currencyDataAccess, _formAuthenticationService, _userService, null);

            _oneAuthenticationService.When(x => x.SetAuthorize(tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(tokenInfo)).Do(x => counter++);

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Assert
            _oneAuthenticationService.Received().SetAuthorize(Arg.Is<OneAuthToken>(tokenInfo));
            Assert.AreEqual(1, counter);
        }
    }
}