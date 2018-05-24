namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Session.Enum;
    using Session.Model;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class OneAuthenticationServiceTest_SignIn
    {
        private OneAuthenticationService _oneAuthenticationService;
        private ISessionService _sessionService;
        private ILicenseeDataAccess _licenseeDataAccess;
        private IFormsAuthenticationService _formAuthenticationService;
        private IOneBookSecurityWrapper _oneBookSecurityWrapper;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _oneBookSecurityWrapper = Substitute.For<IOneBookSecurityWrapper>();

            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                _sessionService,
                _licenseeDataAccess,
                _formAuthenticationService,
                _oneBookSecurityWrapper);
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
        public void SignIn_GetLicenseeByNameReturnNull_ReturnCorrectErrorResultInfo()
        {
            // Arrange
            var tokenInfo = FakeHelper.Create<OneAuthToken>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(x => { return null; });

            var expectedResult = new ResultInfo
            {
                Code = OneAuthorizationErrorCode.LicenseeIsNotExisted,
                Message = ViewResources.LicenseeIsNotExisted
            };

            // Action
            var result = _oneAuthenticationService.SignIn(Arg.Any<HttpContextBase>(), tokenInfo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void SignIn_CurrencyListIsNull_ReturnCorrectErrorResultInfo()
        {
            // Arrange
            var tokenInfo = new OneAuthToken
            {
                SiteName = FakeHelper.Create<string>(),
                Currency = null
            };

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            var expectedResult = new ResultInfo
            {
                Code = OneAuthorizationErrorCode.UserNotHaveCurrency,
                Message = ViewResources.UserNotHaveCurrency
            };

            // Action
            var result = _oneAuthenticationService.SignIn(Arg.Any<HttpContextBase>(), tokenInfo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void SignIn_CurrencyListIsEmpty_ReturnCorrectErrorResultInfo()
        {
            // Arrange
            var tokenInfo = new OneAuthToken
            {
                SiteName = FakeHelper.Create<string>(),
                Currency = new OneAuthCurrency[0]
            };

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            var expectedResult = new ResultInfo
            {
                Code = OneAuthorizationErrorCode.UserNotHaveCurrency,
                Message = ViewResources.UserNotHaveCurrency
            };

            // Action
            var result = _oneAuthenticationService.SignIn(Arg.Any<HttpContextBase>(), tokenInfo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void SignIn_NotHaveUserSession_CreateNewSession()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false);

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
                _sessionService.Received().AssignSessions(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    Arg.Is<LicenseeSession>(x => AssertNewLicenseeSession(x, licensee)));
            });
        }

        [Test]
        public void SignIn_IsInValidIdentity_CreateNewSession()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false);

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            _sessionService.UserSession.Returns(new UserSession());
            _sessionService.LicenseeSession.Returns(new LicenseeSession());

            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.Returns(x => { return null; });

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
                _sessionService.Received().AssignSessions(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    Arg.Is<LicenseeSession>(x => AssertNewLicenseeSession(x, licensee)));
            });
        }

        [Test]
        public void SignIn_IsInValidSessionUser_CreateNewSession()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false);

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            var inValidUser = new UserSession
            {
                UserId = 0,
                VendorIdentityID = null,
                IsInternal = false,
                VendorID = null,
                ApplicationMode = AppMode.Self
            };
            _sessionService.UserSession.Returns(inValidUser);

            var validLicensee = new LicenseeSession
            {
                LicenseeId = Math.Abs(FakeHelper.Create<int>()) + 1
            };
            _sessionService.LicenseeSession.Returns(validLicensee);

            var fakeHttpContext = CreateValidFakeContext(tokenInfo.UserID);

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
                _sessionService.Received().AssignSessions(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    Arg.Is<LicenseeSession>(x => AssertNewLicenseeSession(x, licensee)));
            });
        }

        [Test]
        public void SignIn_IsInValidLicenseeUser_CreateNewSession()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false);

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            var validUser = new UserSession
            {
                UserId = Math.Abs(FakeHelper.Create<int>()) + 1,
                VendorIdentityID = FakeHelper.Create<string>(),
                IsInternal = true,
                VendorID = FakeHelper.Create<string>(),
                ApplicationMode = AppMode.Parasitic
            };
            _sessionService.UserSession.Returns(validUser);

            var inValidLicensee = new LicenseeSession
            {
                LicenseeId = 0
            };
            _sessionService.LicenseeSession.Returns(inValidLicensee);

            var fakeHttpContext = CreateValidFakeContext(tokenInfo.UserID);

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
                _sessionService.Received().AssignSessions(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    Arg.Is<LicenseeSession>(x => AssertNewLicenseeSession(x, licensee)));
            });
        }

        [Test]
        public void SignIn_SessionIsAnotherUser_CreateNewSession()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false);

            var licensee = new Licensee { Id = Math.Abs(FakeHelper.Create<int>()) + 1 };
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            var validAnotherUser = new UserSession
            {
                UserId = Math.Abs(FakeHelper.Create<int>()) + 1,
                IsInternal = true,
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID + 1,
                UserName = tokenInfo.UserID + 1,
                VendorIdentityID = tokenInfo.UserID + 1
            };
            _sessionService.UserSession.Returns(validAnotherUser);

            var validAnotherLicensee = new LicenseeSession
            {
                LicenseeId = licensee.Id + 1
            };
            _sessionService.LicenseeSession.Returns(validAnotherLicensee);

            var fakeHttpContext = CreateValidFakeContext(tokenInfo.UserID);

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
                _sessionService.Received().AssignSessions(
                    Arg.Is<UserSession>(x => AssertNewUserSession(x, tokenInfo)),
                    Arg.Is<LicenseeSession>(x => AssertNewLicenseeSession(x, licensee)));
            });
        }

        [Test]
        public void SignIn_IsAuthenticatedAndIsSameSession_UpdateCurrentSession()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false, "en");

            var licensee = new Licensee { Id = Math.Abs(FakeHelper.Create<int>()) + 1 };
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            var validUser = new UserSession
            {
                UserId = Math.Abs(FakeHelper.Create<int>()) + 1,
                IsInternal = true,
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID,
                UserName = tokenInfo.UserID,
                VendorIdentityID = tokenInfo.UserID,
                Language = "en-US"
            };
            _sessionService.UserSession.Returns(validUser);

            var validLicensee = new LicenseeSession
            {
                LicenseeId = licensee.Id
            };
            _sessionService.LicenseeSession.Returns(validLicensee);

            var fakeHttpContext = CreateValidFakeContext(tokenInfo.UserID);

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    validUser,
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
                Assert.That(string.Join(",", validUser.PortalFunctionList), Is.EqualTo(string.Join(",", tokenInfo.Permissions.Select(y => y.FunctionKey))));
            });
        }

        [TestCase("en", "en-US")]
        [TestCase("ch", "zh-TW")]
        [TestCase("cs", "zh-CN")]
        [TestCase("zhcn", "zh-Hans")]
        [TestCase("jp", "ja-JP")]
        [TestCase("th", "th-TH")]
        [TestCase("ko", "ko-KR")]
        [TestCase("vn", "vi-VN")]
        [TestCase("invalid", "en-Us")]
        public void SignIn_IsAuthenticatedAndIsSameSession_LanguageIsChanged_UpdateLanguage(string tokenLanguage, string expectedLanguageChanged)
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false, tokenLanguage);

            var licensee = new Licensee { Id = Math.Abs(FakeHelper.Create<int>()) + 1 };
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            var validUser = new UserSession
            {
                UserId = Math.Abs(FakeHelper.Create<int>()) + 1,
                IsInternal = true,
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID,
                UserName = tokenInfo.UserID,
                VendorIdentityID = tokenInfo.UserID,
                Language = string.Empty
            };
            _sessionService.UserSession.Returns(validUser);

            var validLicensee = new LicenseeSession
            {
                LicenseeId = licensee.Id
            };
            _sessionService.LicenseeSession.Returns(validLicensee);

            var fakeHttpContext = CreateValidFakeContext(tokenInfo.UserID);

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            _sessionService.Received().UpdateLanguage(expectedLanguageChanged);
        }

        [Test]
        public void SignIn_GetPermissionList_IsNotAdminstrator_GetFromToken_ReturnCorrectData()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(false);

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(
                        x => string.Join(",", x.PortalFunctionList) == string.Join(",", tokenInfo.Permissions.Select(y => y.FunctionKey))),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
            });
        }

        [Test]
        public void SignIn_GetPermissionList_IsAdminstrator_GetFromFile_NoData_ReturnEmptyData()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(true);

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);

            _oneAuthenticationService.GetStringFromFile(Arg.Any<string>()).Returns(string.Empty);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(
                        x => !x.PortalFunctionList.Any()),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
            });
        }

        [Test]
        public void SignIn_GetPermissionList_IsAdminstrator_GetFromFile_HasData_ReturnEmptyData()
        {
            // Arrange
            var tokenInfo = CreateValidFakeToken(true);

            var licensee = FakeHelper.Create<Licensee>();
            _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName).Returns(licensee);

            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);

            var expected = new[]
            {
                new { FunctionId = FakeHelper.Create<int>(), FunctionKey = FakeHelper.Create<string>() },
                new { FunctionId = FakeHelper.Create<int>(), FunctionKey = FakeHelper.Create<string>() }
            };
            var jsonString = JsonConvert.SerializeObject(expected);
            _oneAuthenticationService.GetStringFromFile(Arg.Any<string>()).Returns(jsonString);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            // Action
            _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Result
            Assert.Multiple(() =>
            {
                _sessionService.Received().AssignCurrenciesToUserSession(
                    Arg.Is<UserSession>(
                        x => string.Join(",", x.PortalFunctionList) == string.Join(",", expected.Select(y => y.FunctionKey))),
                    licensee.Id,
                    Arg.Any<IEnumerable<CurrencyUser>>());
            });
        }

        private HttpContextBase CreateValidFakeContext(string identityName)
        {
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(identityName);

            return fakeHttpContext;
        }

        private OneAuthToken CreateValidFakeToken(bool isAdminstrator = false, string language = "")
        {
            return new OneAuthToken
            {
                SiteName = FakeHelper.Create<string>(),
                Currency = FakeHelper.CreateMany<OneAuthCurrency>(2).ToArray(),
                Permissions = FakeHelper.CreateMany<OneAuthPermission>(2).ToArray(),
                IsAdminstrator = isAdminstrator,
                UserID = FakeHelper.Create<string>(),
                VendorID = FakeHelper.Create<string>(),
                Language = language
            };
        }

        private bool AssertNewUserSession(UserSession newUser, OneAuthToken token)
        => newUser.UserId == token.UserSEQ
                && newUser.OnUserId == token.UserSEQ
                && newUser.UserName == token.UserID
                && newUser.IsInternal
                && !newUser.FunctionIdList.Any()
                && newUser.DefaultReportUrl == "BalanceReport"
                && newUser.ApplicationMode == AppMode.Parasitic
                && newUser.VendorID == token.VendorID
                && newUser.VendorIdentityID == token.UserID
                && newUser.ViewType == ClientViewType.Desktop;

        private bool AssertNewLicenseeSession(LicenseeSession newLicensee, Licensee licenseeInfo)
        => newLicensee.LicenseeId == licenseeInfo.Id
                && newLicensee.IsAllowedCustomerComm == licenseeInfo.IsAllowedCustomerComm
                && newLicensee.IsEnableBA == licenseeInfo.IsEnableBA
                && newLicensee.LicenseeName == licenseeInfo.Name;
    }
}