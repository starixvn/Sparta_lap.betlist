namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using System.Web;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DTO;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class OneAuthenticationService_SetAuthorizeTest
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
            _licenseeService.GetLicenseeByName(Arg.Any<string>()).Returns(new Licensee { Id = 1, Name = "spondemo", IsAllowedCustomerComm = true });
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _userService.GetByUsername(Arg.Any<string>(), Arg.Any<int>()).Returns(new User { UserID = 1 });
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
        public void SignIn_SetAuthorize_IfNotAdminNotCallGetCurrencyFromDB()
        {
            // Arrange
            var tokenInfo = new OneAuthToken { IsAdminstrator = false };

            _oneAuthenticationService = new OneAuthenticationService(_sessionService, _licenseeService, _currencyService, _formAuthenticationService, _userService, null);

            // Action
            _oneAuthenticationService.SetAuthorize(tokenInfo);

            // Assert
            _currencyService.DidNotReceive().GetCurrenciesByUser(Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public void SignIn_SetAuthorize_ReturnErrorLicenseDoesNotOfferedCurrency()
        {
            // Arrange
            var tokenInfo = new OneAuthToken { UserSEQ = 1, UserID = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);

            _currencyService.GetCurrenciesByUser(Arg.Any<int>(), Arg.Any<int>()).Returns(new List<CurrencyUser>());

            _oneAuthenticationService = new OneAuthenticationService(_sessionService, _licenseeService, _currencyService, _formAuthenticationService, _userService, null);

            var expectedResult = new ResultInfo { Code = 600, Message = "Not found currency" };

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

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
            OneAuthToken tokenInfo;
            Licensee expectedLicInfo;
            CreateTokenInfoAndLicInfo(ExpectedLicId, out tokenInfo, out expectedLicInfo);
            var expectedCookie = $"{tokenInfo.VendorID}_{tokenInfo.SiteName}_{tokenInfo.UserID}";
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _currencyService.GetCurrenciesByUser(Arg.Any<int>(), Arg.Any<int>()).Returns(FakeCurrencyListMapByUser());

            _sessionService.When(x => x.CreateNewSession(Arg.Any<UserSession>(), Arg.Any<LicenseeSession>()))
                .Do(
                    x =>
                    {
                        sessionCounter++;
                        user = x.ArgAt<UserSession>(0);
                        lic = x.ArgAt<LicenseeSession>(1);
                    });

            _formAuthenticationService.When(x => x.SetAuthCookie(expectedCookie, false)).Do(x => cookieCounter++);

            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                _sessionService, _licenseeService, _currencyService, _formAuthenticationService, _userService, null);

            _oneAuthenticationService.GetStringFromFile(Arg.Any<string>()).Returns(GetJsonSystemFunctionList());

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Assert
            _sessionService.Received(1).CreateNewSession(Arg.Any<UserSession>(), Arg.Any<LicenseeSession>());
            _formAuthenticationService.Received(1).SetAuthCookie(expectedCookie, false);
            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(1, sessionCounter);
            Assert.AreEqual(1, cookieCounter);
            Assert.False(string.IsNullOrEmpty(result.ExtendedData));
            Assert.True(VeryfiLicSession(lic, expectedLicInfo));
        }

        private static bool VeryfiLicSession(LicenseeSession lic, Licensee expectedLicInfo)
        {
            return lic.LicenseeId == expectedLicInfo.Id && lic.LicenseeName == expectedLicInfo.Name
                 && lic.IsEnableBA == expectedLicInfo.IsEnableBA && lic.IsAllowedCustomerComm == expectedLicInfo.IsAllowedCustomerComm;
        }

        private static void CreateTokenInfoAndLicInfo(int expectedLicId, out OneAuthToken tokenInfo, out Licensee expectedLicInfo)
        {
            tokenInfo = new OneAuthToken
            {
                UserSEQ = 1,
                UserID = "pgmAlex",
                SiteName = "spondemo",
                VendorID = "locl",
                ActionLink = "test",
                Language = "cs",
                Currency = new OneAuthCurrency[]
                {
                    new OneAuthCurrency()
                },
                Permissions = new OneAuthPermission[]
                {
                    new OneAuthPermission()
                }
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

        private static IList<CurrencyUser> FakeCurrencyListMapByUser()
        {
            return new List<CurrencyUser>
            {
                new CurrencyUser
                {
                    CurrencyId = 1,
                    CurrencyName = "USD"
                }
            };
        }

        private static string GetJsonSystemFunctionList()
        {
            var items = new List<PortalSystemFunction>
                {
                    new PortalSystemFunction { FunctionKey = "LicenseeWinLoss" },
                    new PortalSystemFunction { FunctionKey = "BalanceReport" }
                };
            return JsonConvert.SerializeObject(items);
        }
    }
}