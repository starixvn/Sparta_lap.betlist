namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Linq;
    using System.Web;
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class OneAuthenticationServiceTest_SetAuthorize
    {
        private OneAuthenticationService _oneAuthenticationService;
        private ISessionService _sessionService;
        private ILicenseeDataAccess _licenseeDataAccess;
        private IFormsAuthenticationService _formAuthenticationService;
        private IOneBookSecurityWrapper _oneBookSecurityWrapper;
        private IOneAuthDataAccess _oneAuthDataAccess;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _oneBookSecurityWrapper = Substitute.For<IOneBookSecurityWrapper>();
            _oneAuthDataAccess = Substitute.For<IOneAuthDataAccess>();

            _oneAuthDataAccess.GetUserInfo(Arg.Any<int>(), Arg.Any<string>()).Returns(new OneAuthUserInfo());

            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                _sessionService,
                _licenseeDataAccess,
                _formAuthenticationService,
                _oneBookSecurityWrapper,
                Substitute.For<ICoreLogger>(),
                _oneAuthDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _oneAuthenticationService = null;
            _sessionService = null;
            _licenseeDataAccess = null;
            _formAuthenticationService = null;
        }

        [Test]
        public void SignIn_SetAuthorize_UpdateCookieAndReturnCorrectResult()
        {
            // Arrange
            var tokenInfo = new OneAuthToken
            {
                SiteName = FakeHelper.Create<string>(),
                Currency = FakeHelper.CreateMany<OneAuthCurrency>(2).ToArray(),
                Permissions = FakeHelper.CreateMany<OneAuthPermission>(2).ToArray(),
                IsAdminstrator = false,
                UserID = FakeHelper.Create<string>(),
                VendorID = FakeHelper.Create<string>(),
                ActionLink = FakeHelper.Create<string>()
            };

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            var expectedAuthName = $"{tokenInfo.VendorID}_{tokenInfo.SiteName}_{tokenInfo.UserID}";

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _formAuthenticationService.Received().SetAuthCookie(expectedAuthName, false);
                Assert.That(result.ExtendedData, Is.EqualTo(tokenInfo.ActionLink));
            });
        }
    }
}