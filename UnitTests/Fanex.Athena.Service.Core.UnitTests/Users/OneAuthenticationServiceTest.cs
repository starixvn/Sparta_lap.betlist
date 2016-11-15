namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Caching;
    using Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class OneAuthenticationServiceTest
    {
        private OneAuthenticationService _oneAuthenticationService;
        private ICache _cacheService;
        private TextReader fakeTextReader;
        private int _cacheCounter = 0;
        private ISessionService _sessionService;
        private ILicenseeService _licenseeService;
        private IFormsAuthenticationService _formAuthenticationService;

        [SetUp]
        public void SetUp()
        {
            fakeTextReader = new FakeReader();
            _cacheService = Substitute.For<ICache>();
            _cacheService.Get<IList<LicenseeSiteName>>(Arg.Is<string>("OneAuthenticationService_LicenseeSiteNameCacheKey")).Returns(x => null);
            _cacheService.When(x => x.Set<IList<LicenseeSiteName>>(Arg.Any<string>(), Arg.Any<IList<LicenseeSiteName>>(), Arg.Any<DateTime>()))
                .DoNotCallBase();

            _cacheService.When(x => x.Set<IList<LicenseeSiteName>>(Arg.Any<string>(), Arg.Any<IList<LicenseeSiteName>>(), Arg.Any<DateTime>()))
                .Do(x => _cacheCounter++);

            _sessionService = Substitute.For<ISessionService>();
            _licenseeService = Substitute.For<LicenseeService>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _oneAuthenticationService = Substitute.For<OneAuthenticationService>(_sessionService, _licenseeService, _formAuthenticationService);

            _oneAuthenticationService.When(x => x.GetMemCacheService()).DoNotCallBase();
            _oneAuthenticationService.GetMemCacheService().Returns(_cacheService);
            _oneAuthenticationService.When(x => x.GetStreamReader(null)).DoNotCallBase();
            _oneAuthenticationService.GetStreamReader(null).Returns(fakeTextReader);
        }

        [TearDown]
        public void TearDown()
        {
            _cacheService = null;
            fakeTextReader = null;
            _oneAuthenticationService = null;
            _cacheCounter = 0;
            _sessionService = null;
            _licenseeService = null;
            _formAuthenticationService = null;
        }

        #region SingleLicenseeSiteName

        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("12 bet", 0)]
        public void SingleLicenseeSiteName_InvalidSiteName_ReturnDefatulValue(string siteName, int expectedLicenseeId)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LicenseeID, expectedLicenseeId);
        }

        [TestCase("hello", 0)]
        public void SingleLicenseeSiteName_NotExistedSiteName_ReturnDefatulValue(string siteName, int expectedLicenseeId)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LicenseeID, expectedLicenseeId);
        }

        [TestCase(1, "12bet")]
        public void SingleLicenseeSiteName_ExitedSiteName_ReturnValidLicenseeSiteName(int licenseeId, string siteName)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.SiteName, siteName);
            Assert.AreEqual(result.LicenseeID, licenseeId);
        }

        [TestCase(2, "12bet44")]
        [TestCase(1, "12bet")]
        public void SingleLicenseeSiteName_AnySiteName_SetDataToCache(int licenseeId, string siteName)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.AreEqual(1, _cacheCounter);
        }

        [TestCase(3, "12bet44")]
        public void SingleLicenseeSiteName_AnySiteName_GetDataFromCache(int licenseeId, string siteName)
        {
            // Arrange
            var data = new List<LicenseeSiteName>
                {
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "12bet44" },
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "spondemo222" }
                };
            _cacheService.Get<IList<LicenseeSiteName>>(Arg.Is<string>("OneAuthenticationService_LicenseeSiteNameCacheKey")).Returns(data);

            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            _cacheService.DidNotReceive().Set(Arg.Any<string>(), Arg.Any<IList<LicenseeSiteName>>(), Arg.Any<DateTime>());
        }

        #endregion SingleLicenseeSiteName

        #region SignIn

        [Test]
        public void SignIn_NotHaveSession_CallSetAuthorize()
        {
            // Arrange
            var counter = 0;
            var licenseeId = 1;
            var tokenInfo = new OneAuthenticationToken { UserID = "1" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).DoNotCallBase();

            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).Do(x => counter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, licenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.Received().SetAuthorize(licenseeId, tokenInfo);
            Assert.AreEqual(1, counter);
        }

        [TestCase("zh")]
        [TestCase("cs")]
        [TestCase("ch")]
        [TestCase("vn")]
        [TestCase("ko")]
        [TestCase("jp")]
        public void SignIn_AlreadyHasSessionFixWithToken_UpdateLanguage(string newLanguageCode)
        {
            // Arrange
            int licenseeId = 1, counter = 0, langCounter = 0;

            var tokenInfo = FakeSessionFixToken(1);
            tokenInfo.Language = newLanguageCode;
            var fakeHttpContext = Substitute.For<HttpContextBase>();

            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(tokenInfo.UserName);
            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).Do(x => counter++);
            _sessionService.When(x => x.UpdateLanguage(Arg.Any<string>())).Do(x => langCounter++);
            _sessionService.UpdateLanguage(Arg.Is<string>(newLanguageCode)).Returns(true);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, licenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.DidNotReceive().SetAuthorize(Arg.Is<int>(licenseeId), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(0, counter);
            _sessionService.Received(1).UpdateLanguage(Arg.Any<string>());
            Assert.AreEqual(1, langCounter);
        }

        [Test]
        public void SignIn_AlreadyHasSessionFixWithToken_DoNoThing()
        {
            // Arrange
            int licenseeId = 1, counter = 0;
            var tokenInfo = FakeSessionFixToken(1);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(tokenInfo.UserName);
            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).Do(x => counter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, licenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.DidNotReceive().SetAuthorize(Arg.Is<int>(licenseeId), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(0, counter);
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
            int licenseeId = 1, counter = 0;
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "alex", VendorID = "spondemo" };
            var userSession = new UserSession
            {
                UserId = userId,
                UserName = userName,
                ApplicationMode = AppMode.Parasitic,
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
            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(1, tokenInfo)).Do(x => counter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, licenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.Received().SetAuthorize(Arg.Is<int>(licenseeId), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(1, counter);
        }

        private OneAuthenticationToken FakeSessionFixToken(int licenseeId)
        {
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "alex", VendorID = "spondemo" };
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

        #endregion SignIn

        private class FakeReader : TextReader
        {
            public override string ReadToEnd()
            {
                var items = new List<LicenseeSiteName>
                {
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "12bet" },
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "spondemo" }
                };
                return JsonConvert.SerializeObject(items);
            }
        }
    }
}