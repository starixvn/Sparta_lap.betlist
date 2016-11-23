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

    [TestFixture]
    public class OneAuthenticationService_SignInTest
    {
        private OneAuthenticationService _oneAuthenticationService;
        private TextReader _fakeTextReader;
        private ISessionService _sessionService;
        private IFunctionService _functionService;
        private ILicenseeService _licenseeService;
        private ICurrencyService _currencyService;
        private IFormsAuthenticationService _formAuthenticationService;

        [SetUp]
        public void SetUp()
        {
            _fakeTextReader = new FakeFunctionReader();
            _sessionService = Substitute.For<ISessionService>();
            _functionService = Substitute.For<IFunctionService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _currencyService = Substitute.For<ICurrencyService>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                _sessionService,
                _functionService,
                _licenseeService,
                _currencyService,
                _formAuthenticationService);
            _oneAuthenticationService.GetStringFromFile(Arg.Any<string>()).Returns(_fakeTextReader.ReadToEnd());
        }

        [TearDown]
        public void TearDown()
        {
            _fakeTextReader = null;
            _oneAuthenticationService = null;
            _sessionService = null;
            _licenseeService = null;
            _currencyService = null;
            _formAuthenticationService = null;
        }

        [Test]
        public void SignIn_SetAuthorize_ReturnErrorLicenseDoesNotOfferedCurrency()
        {
            // Arrange
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _licenseeService.GetLicenseeById(Arg.Any<int>()).Returns(x => new Licensee { Id = 1 });
            _currencyService.GetCurrencies(Arg.Any<int>()).Returns(new List<Currency>());
            _oneAuthenticationService = new OneAuthenticationService(_sessionService, _functionService, _licenseeService, _currencyService, _formAuthenticationService);

            var expectedResult = new ResultInfo { Code = 600, Message = "Not found currency" };

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, Arg.Any<int>(), tokenInfo);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedResult.Code, result.Code);
        }

        [Test]
        public void SignIn_SetAuthorize_ReturnErrorLicenseDoesNotHaveAgent()
        {
            // Arrange
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _licenseeService.GetLicenseeById(Arg.Any<int>()).Returns(x => new Licensee { Id = 1 });
            _currencyService.GetCurrencyMapByUser(Arg.Any<int>(), Arg.Any<int>()).Returns(new Dictionary<string, int> { { "UUS", 0 } });
            _oneAuthenticationService = new OneAuthenticationService(_sessionService, _functionService, _licenseeService, _currencyService, _formAuthenticationService);

            var expectedResult = new ResultInfo { Code = 200, Message = "Not found currency" };

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, Arg.Any<int>(), tokenInfo);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedResult.Code, result.Code);
        }

        [Test]
        public void SignIn_SetAuthorize_ReturnErrorIsLicenseeNotFound()
        {
            // Arrange
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _licenseeService.GetLicenseeById(Arg.Any<int>()).Returns(x => null);
            var expectedResult = new ResultInfo { Code = 200, Message = "Licensee is Not Exit" };
            _oneAuthenticationService = new OneAuthenticationService(_sessionService, _functionService, _licenseeService, _currencyService, _formAuthenticationService);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, Arg.Any<int>(), tokenInfo);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedResult.Code, result.Code);
        }

        [Test]
        public void SignIn_SetAuthorize_ReturnRightResultInfo()
        {
            // Arrange
            var sessionCounter = 0;
            var cookieCounter = 0;
            const int ExpectedLicId = 1;
            UserSession user = null;
            LicenseeSession lic = null;
            OneAuthenticationToken tokenInfo;
            Licensee expectedLicInfo;
            CreateTokenInfoAndLicInfo(ExpectedLicId, out tokenInfo, out expectedLicInfo);
            var expectedCookie = $"{tokenInfo.UserName}_{tokenInfo.VendorID}_{tokenInfo.SiteName}";
            var expectedCurrency = new Dictionary<string, int> { { "USD", 1 } };
            var expectedCurrencyList = expectedCurrency.Select(x => new Currency
            {
                CurrencyId = x.Value,
                Name = x.Key
            }).ToList();
            var expectedAgents = new List<LicenseeCustomer> { new LicenseeCustomer() };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _functionService.GetUserFunctionList(Arg.Any<int>(), Arg.Any<int>()).Returns(new List<Function> { new Function() });
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _licenseeService.GetLicenseeById(Arg.Any<int>()).Returns(x => expectedLicInfo);
            _currencyService.GetCurrencyMapByUser(Arg.Any<int>(), Arg.Any<int>()).Returns(expectedCurrency);
            _licenseeService.GetCustByLicensee(Arg.Any<int>()).Returns(expectedAgents);
            _sessionService.When(x => x.CreateNewSession(Arg.Any<UserSession>(), Arg.Any<LicenseeSession>()))
                .Do(
                    x =>
                    {
                        sessionCounter++;
                        user = x.ArgAt<UserSession>(0);
                        lic = x.ArgAt<LicenseeSession>(1);
                    });

            _formAuthenticationService.When(x => x.SetAuthCookie(expectedCookie, false)).Do(x => cookieCounter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, ExpectedLicId, tokenInfo);

            // Assert
            _sessionService.Received(1).CreateNewSession(Arg.Any<UserSession>(), Arg.Any<LicenseeSession>());
            _formAuthenticationService.Received(1).SetAuthCookie(expectedCookie, false);
            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(1, sessionCounter);
            Assert.AreEqual(1, cookieCounter);
            Assert.False(string.IsNullOrEmpty(result.ExtendedData));
            Assert.True(VerifyUserSession(user, expectedCurrencyList, tokenInfo));
            Assert.True(VeryfiLicSession(lic, expectedAgents, expectedLicInfo, tokenInfo));
        }

        [Test]
        public void SignIn_NotHaveSession_CallSetAuthorize()
        {
            // Arrange
            var counter = 0;
            const int LicenseeId = 1;
            var tokenInfo = new OneAuthenticationToken { UserID = "1" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).DoNotCallBase();

            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).Do(x => counter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, LicenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.Received().SetAuthorize(LicenseeId, Arg.Any<string>(), tokenInfo);
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
            const int LicenseeId = 1;
            var counter = 0;
            var langCounter = 0;

            var tokenInfo = FakeSessionFixToken(1);
            tokenInfo.Language = newLanguageCode;
            var fakeHttpContext = Substitute.For<HttpContextBase>();

            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(tokenInfo.UserName);
            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).Do(x => counter++);
            _sessionService.When(x => x.UpdateLanguage(Arg.Any<string>())).Do(x => langCounter++);
            _sessionService.UpdateLanguage(Arg.Is<string>(newLanguageCode)).Returns(true);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, LicenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.DidNotReceive().SetAuthorize(Arg.Is<int>(LicenseeId), Arg.Any<string>(), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(0, counter);
            _sessionService.Received(1).UpdateLanguage(Arg.Any<string>());
            Assert.AreEqual(1, langCounter);
            Assert.True(result.ExtendedData.Equals(tokenInfo.ActionLink));
        }

        [Test]
        public void SignIn_AlreadyHasSessionFixWithToken_AssignActionLinkToResult()
        {
            // Arrange
            const int LicenseeId = 1;
            var counter = 0;
            var tokenInfo = FakeSessionFixToken(1);

            var fakeHttpContext = Substitute.For<HttpContextBase>();

            fakeHttpContext.User.Identity.IsAuthenticated.Returns(true);
            fakeHttpContext.User.Identity.Name.Returns(tokenInfo.UserName);
            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).Do(x => counter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, LicenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.DidNotReceive().SetAuthorize(Arg.Is<int>(LicenseeId), Arg.Any<string>(), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(0, counter);
            Assert.True(result.ExtendedData.Equals(tokenInfo.ActionLink));
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
            const int LicenseeId = 1;

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
            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(1, Arg.Any<string>(), tokenInfo)).Do(x => counter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, LicenseeId, tokenInfo);

            // Result
            _oneAuthenticationService.Received().SetAuthorize(Arg.Is<int>(LicenseeId), Arg.Any<string>(), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(1, counter);
        }

        private static bool VeryfiLicSession(LicenseeSession lic, List<LicenseeCustomer> expectedAgents, Licensee expectedLicInfo, OneAuthenticationToken tokenInfo)
        {
            var licType = expectedAgents.Where(s => !string.IsNullOrEmpty(s.Description))
                                        .Select(s => s.Description)
                                        .Distinct()
                                        .ToList();
            var curName = expectedAgents.Select(s => s.CurrencyName).Distinct().ToList();
            return lic.LicenseeId == expectedLicInfo.Id && lic.CurrencyName.Count == curName.Count
                 && lic.LicenseeType.Count == licType.Count && lic.LicenseeName == expectedLicInfo.Name
                 && lic.IsEnableBA == expectedLicInfo.IsEnableBA && lic.IsAllowedCustomerComm == expectedLicInfo.IsAllowedCustomerComm;
        }

        private static void CreateTokenInfoAndLicInfo(int expectedLicId, out OneAuthenticationToken tokenInfo, out Licensee expectedLicInfo)
        {
            tokenInfo = new OneAuthenticationToken
            {
                UserID = "1",
                UserSEQ = 1,
                UserName = "pgmAlex",
                SiteName = "spondemo",
                VendorID = "locl",
                ActionLink = "test",
                Language = "cs"
            };
            expectedLicInfo = new Licensee
            {
                Id = expectedLicId,
                Name = tokenInfo.SiteName,
                Description = "USD",
                IsActive = true,
                IsAllowedCustomerComm = true,
                IsEnableBA = false,
                ResellerId = 1,
                ResellerName = "test"
            };
        }

        private static bool VerifyUserSession(UserSession userSession, IList<Currency> currency, OneAuthenticationToken tokenInfo)
        {
            var lang = Util.MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                       .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
            return userSession != null && userSession.UserId == tokenInfo.UserSEQ &&
                userSession.VendorID == tokenInfo.VendorID && userSession.VendorIdentityID == tokenInfo.UserID
                && userSession.IsInternal && userSession.ApplicationMode == AppMode.Parasitic
                && userSession.PortalFunctionList.Count > 0 && userSession.CurrencyList.Count == currency.Count
                && userSession.Language == lang && userSession.FunctionIdList.Count == 0;
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

        private class FakeFunctionReader : TextReader
        {
            public override string ReadToEnd()
            {
                var items = new List<PortalSystemFunction>
                {
                    new PortalSystemFunction { FunctionId = 1, FunctionKey = "LicenseeWinLoss" },
                    new PortalSystemFunction { FunctionId = 1, FunctionKey = "BalanceReport" }
                };
                return JsonConvert.SerializeObject(items);
            }
        }
    }
}