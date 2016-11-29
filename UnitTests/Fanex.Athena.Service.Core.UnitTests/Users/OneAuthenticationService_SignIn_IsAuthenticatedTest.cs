namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DataDomain;
    using Model.Core.DTO;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Fanex.Athena.Model.Core.MetaModel.Account;

    [TestFixture]
    public class OneAuthenticationService_SignIn_IsAuthenticatedTest
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
            _userService.GetByUsername(Arg.Any<string>(), Arg.Any<int>()).Returns(new User { Id = 1 });

            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                _sessionService,
                _licenseeService,
                _currencyService,
                _formAuthenticationService,
                _userService);
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
        public void SignIn_NotHaveSession_CallSetAuthorize()
        {
            // Arrange
            var counter = 0;
            var licensee = new Licensee
            {
                Id = 1
            };
            var tokenInfo = new OneAuthenticationToken { UserID = "1" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).DoNotCallBase();

            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).Do(x => counter++);
            _licenseeService.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            _oneAuthenticationService.Received().SetAuthorize(licensee, Arg.Any<string>(), tokenInfo);
            Assert.AreEqual(1, counter);
        }

        [TestCase("zh")]
        [TestCase("cs")]
        [TestCase("ch")]
        [TestCase("vn")]
        [TestCase("ko")]
        [TestCase("jp")]
        public void SignIn_AlreadyHasSessionFixWithToken_UpdateLanguageAndAssignActionLinkToResult(string newLanguageCode)
        {
            // Arrange
            var licensee = new Licensee
            {
                Id = 1
            };
            var counter = 0;
            var langCounter = 0;

            var tokenInfo = FakeSessionFixToken(1);
            tokenInfo.Language = newLanguageCode;
            var fakeHttpContext = Substitute.For<HttpContextBase>();

            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(tokenInfo.UserName);
            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).Do(x => counter++);
            _sessionService.When(x => x.UpdateLanguage(Arg.Any<string>())).Do(x => langCounter++);
            _sessionService.UpdateLanguage(Arg.Is<string>(newLanguageCode)).Returns(true);
            _licenseeService.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            _oneAuthenticationService.DidNotReceive().SetAuthorize(Arg.Any<Licensee>(), Arg.Any<string>(), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(0, counter);
            _sessionService.Received(1).UpdateLanguage(Arg.Any<string>());
            Assert.AreEqual(1, langCounter);
            Assert.True(result.ExtendedData.Equals(tokenInfo.ActionLink));
        }

        [Test]
        public void SignIn_AlreadyHasSessionFixWithToken_AssignActionLinkToResult()
        {
            // Arrange
            var licensee = new Licensee
            {
                Id = 1
            };
            var counter = 0;
            var tokenInfo = FakeSessionFixToken(1);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(tokenInfo.UserName);
            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).Do(x => counter++);
            _licenseeService.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            _oneAuthenticationService.DidNotReceive().SetAuthorize(Arg.Is<Licensee>(licensee), Arg.Any<string>(), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(0, counter);
            Assert.True(result.ExtendedData.Equals(tokenInfo.ActionLink));
        }

        public void SignIn_LicenseeNameIsNotExisted_ReturnErrorCodeEqual200()
        {
            // Arrange
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "alex", VendorID = "spondemo" };

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            _licenseeService.GetLicenseeByName(tokenInfo.SiteName).Returns(x => null);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.That(result.Code.ToString(), Is.EqualTo(OneAuthorizationErrorCode.LicenseeIsNotExisted));
        }

        [TestCase(2, "alex", "spondemo", "1", 1, AppMode.Parasitic, true)]
        [TestCase(2, "alex3", "spondemo", "1", 1, AppMode.Parasitic, true)]
        [TestCase(2, "alex", "spondemo1", "1", 1, AppMode.Parasitic, true)]
        [TestCase(2, "alex", "spondemo", "3", 1, AppMode.Parasitic, true)]
        [TestCase(2, "alex", "spondemo", "1", 5, AppMode.Parasitic, true)]
        [TestCase(2, "alex", "spondemo", "1", 1, AppMode.Self, true)]
        [TestCase(2, "alex", "spondemo", "1", 1, AppMode.Parasitic, false)]
        public void SignIn_AlreadyHasSessionButNotFixWithToken_CallSetAuthorize(
            int userId,
            string userName,
            string vendorId,
            string identity,
            int licId,
            AppMode mode,
            bool isInternal)
        {
            // Arrange
            var licensee = new Licensee
            {
                Id = 1
            };

            var counter = 0;

            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "alex", VendorID = "spondemo" };
            var userSession = new UserSession
            {
                UserId = userId,
                UserName = userName,
                ApplicationMode = mode,
                VendorID = vendorId,
                IsInternal = isInternal,
                VendorIdentityID = identity
            };
            var licSession = new LicenseeSession
            {
                LicenseeId = licId
            };

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(tokenInfo.UserName);
            _sessionService.UserSession.Returns(x => userSession);
            _sessionService.LicenseeSession.Returns(x => licSession);
            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(licensee, Arg.Any<string>(), tokenInfo)).Do(x => counter++);
            _licenseeService.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            _oneAuthenticationService.Received().SetAuthorize(Arg.Any<Licensee>(), Arg.Any<string>(), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(1, counter);
        }

        private OneAuthenticationToken FakeSessionFixToken(int licenseeId)
        {
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "alex", VendorID = "spondemo", ActionLink = "LiceenseeWLReport" };
            var userSession = new UserSession
            {
                UserId = tokenInfo.UserSEQ,
                UserName = tokenInfo.UserName,
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID,
                IsInternal = true,
                VendorIdentityID = tokenInfo.UserID,
                Language = "en-US"
            };
            var licSession = new LicenseeSession
            {
                LicenseeId = licenseeId
            };

            _sessionService.UserSession.Returns(x => userSession);
            _sessionService.LicenseeSession.Returns(x => licSession);

            return tokenInfo;
        }

    }
}