namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.SessionState;
    using Core.Common.Interfaces;
    using Core.Users;
    using Core.Users.Interfaces;
    using Model.Core.DTO;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SessionServiceTest
    {
        private const int DefaultUserId = 1;

        private IFunctionService _functionService;
        private ILicenseeService _licenseeService;
        private ICurrencyService _currencyService;
        private ISessionService _sessionService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _functionService = Substitute.For<IFunctionService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            var licenseeList = new List<Licensee>
            {
                new Licensee
                {
                    Id = 1,
                    Name = "12bet"
                }
            };
            _licenseeService.GetByUser(DefaultUserId).Returns<IList<Licensee>>(i => licenseeList);
            _currencyService = Substitute.For<ICurrencyService>();
            _sessionService = new SessionService(_functionService, _licenseeService, _currencyService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _functionService = null;
            _licenseeService = null;
            _currencyService = null;
            _sessionService = null;
        }

        [Test]
        public void CreateNewSession_WhenCall_UserSessionIsCreatedProperly()
        {
            // Action
            CreateDefaultNewSession();

            // Assert
            Assert.That(_sessionService.UserSession.UserId, Is.EqualTo(DefaultUserId));
        }

        [Test]
        public void CreateNewSession_WhenCall_LicenseeSessionIsCreatedProperly()
        {
            // Arrange
            const int LicenseeId = 1;
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" }
            };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);

            // Action
            CreateDefaultNewSession(LicenseeId);

            // Assert
            Assert.That(_sessionService.LicenseeSession.LicenseeId, Is.EqualTo(LicenseeId));
            Assert.That(_sessionService.LicenseeSession.LicenseeName, Is.EqualTo("spondemo"));
        }

        [Test]
        public void CreateNewSession_CurrentSessionOfLicenseeIsNull_LicenseeSessionIsNull()
        {
            // Arrange
            const int LicenseeId = 1;
            const string LicenseeSessionKey = "LicenseeSessionKey";

            // Action
            CreateDefaultNewSession(LicenseeId);
            HttpContext.Current.Session[LicenseeSessionKey] = null;

            // Assert
            Assert.That(_sessionService.LicenseeSession, Is.Null);
        }

        [Test]
        public void CreateNewSession_LicenseeIdIsZero_LicenseeSessionIsNull()
        {
            // Action
            CreateDefaultNewSession(0);

            // Assert
            Assert.That(_sessionService.LicenseeSession, Is.Null);
        }

        [Test]
        public void UpdateLiceseeSession_WhenCall_LicenseeSessionIsCreatedProperly()
        {
            // Arrange
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" },
                new Licensee { Id = 2, Name = "12bet" },
            };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);
            const int OldLicenseeId = 1;
            const int NewLicenseeId = 2;
            CreateDefaultNewSession(OldLicenseeId);

            // Action
            _sessionService.UpdateLiceseeSession(NewLicenseeId);

            // Assert
            Assert.That(_sessionService.LicenseeSession.LicenseeId, Is.EqualTo(NewLicenseeId));
            Assert.That(_sessionService.LicenseeSession.LicenseeName, Is.EqualTo("12bet"));
        }

        [Test]
        public void UpdateLiceseeSession_LicenseeIdIsZero_LicenseeSessionIsNotCreated()
        {
            // Arrange
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" },
                new Licensee { Id = 2, Name = "12bet" },
            };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);
            const int OldLicenseeId = 1;
            const int NewLicenseeId = 0;
            CreateDefaultNewSession(OldLicenseeId);

            // Action
            _sessionService.UpdateLiceseeSession(NewLicenseeId);

            // Assert
            Assert.That(_sessionService.LicenseeSession.LicenseeId, Is.EqualTo(OldLicenseeId));
            Assert.That(_sessionService.LicenseeSession.LicenseeName, Is.EqualTo("spondemo"));
        }

        [Test]
        public void UpdateLiceseeSession_FunctionListContainReportBalance_ReturnCorrectReportUrl()
        {
            // Arrange
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" },
                new Licensee { Id = 2, Name = "12bet" },
            };
            var functions = new List<Function> { new Function { FunctionId = 42 } };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);
            const int OldLicenseeId = 1;
            const int NewLicenseeId = 2;
            _functionService.GetUserFunctionList(NewLicenseeId, DefaultUserId).Returns(functions);
            CreateDefaultNewSession(OldLicenseeId);

            // Action
            _sessionService.UpdateLiceseeSession(NewLicenseeId);

            // Assert
            Assert.That(_sessionService.UserSession.DefaultReportUrl, Is.EqualTo("BalanceReport"));
        }

        [Test]
        public void UpdateLiceseeSession_FunctionListIsNotContainReportBalance_ReturnCorrectReportUrl()
        {
            // Arrange
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" },
                new Licensee { Id = 2, Name = "12bet" },
            };
            var functions = new List<Function> { new Function { FunctionId = 1, ParentId = 1, IsActive = true, Url = "TestUrl" } };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);
            const int OldLicenseeId = 1;
            const int NewLicenseeId = 2;
            _functionService.GetUserFunctionList(NewLicenseeId, DefaultUserId).Returns(functions);
            CreateDefaultNewSession(OldLicenseeId);

            // Action
            _sessionService.UpdateLiceseeSession(NewLicenseeId);

            // Assert
            Assert.That(_sessionService.UserSession.DefaultReportUrl, Is.EqualTo("TestUrl"));
        }

        [Test]
        public void UpdateLiceseeSession_FunctionListIsNotContainReportBalanceButIsActiveIsFalse_ReturnCorrectReportUrl()
        {
            // Arrange
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" },
                new Licensee { Id = 2, Name = "12bet" },
            };
            var functions = new List<Function> { new Function { FunctionId = 1, ParentId = 1, IsActive = false, Url = "TestUrl" } };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);
            const int OldLicenseeId = 1;
            const int NewLicenseeId = 2;
            _functionService.GetUserFunctionList(NewLicenseeId, DefaultUserId).Returns(functions);
            CreateDefaultNewSession(OldLicenseeId);

            // Action
            _sessionService.UpdateLiceseeSession(NewLicenseeId);

            // Assert
            Assert.That(_sessionService.UserSession.DefaultReportUrl, Is.EqualTo("BalanceReport"));
        }

        [Test]
        public void UpdateLiceseeSession_FunctionListIsNotContainReportBalanceButParentIdIsZero_ReturnCorrectReportUrl()
        {
            // Arrange
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" },
                new Licensee { Id = 2, Name = "12bet" },
            };
            var functions = new List<Function> { new Function { FunctionId = 1, ParentId = 0, IsActive = true, Url = "TestUrl" } };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);
            const int OldLicenseeId = 1;
            const int NewLicenseeId = 2;
            _functionService.GetUserFunctionList(NewLicenseeId, DefaultUserId).Returns(functions);
            CreateDefaultNewSession(OldLicenseeId);

            // Action
            _sessionService.UpdateLiceseeSession(NewLicenseeId);

            // Assert
            Assert.That(_sessionService.UserSession.DefaultReportUrl, Is.EqualTo("BalanceReport"));
        }

        [Test]
        public void UpdateLanguage_UserSessionAndLanguageCodeNotNull_UserSessionLanguageIsCorrect()
        {
            // Arrange
            const string LanguageCode = "vi-VN";
            CreateDefaultNewSession();

            // Action
            _sessionService.UpdateLanguage(LanguageCode);

            // Assert
            Assert.That(_sessionService.UserSession.Language, Is.EqualTo(LanguageCode));
        }

        [Test]
        public void UpdateLanguage_UserSessionAndLanguageCodeNotNull_ReturnTrue()
        {
            // Arrange
            const string LanguageCode = "vi-VN";
            CreateDefaultNewSession();

            // Action
            var result = _sessionService.UpdateLanguage(LanguageCode);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void UpdateLanguage_LanguageCodeIsEmpty_ReturnFalse()
        {
            // Arrange
            CreateDefaultNewSession();

            // Action
            var result = _sessionService.UpdateLanguage(string.Empty);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAllowedEditCommission_ApplicationModeIsParasiticAndPortalFunctionListContainCorrectKey_ReturnTrue()
        {
            // Arrange
            var functionList = new List<PortalSystemFunction> { new PortalSystemFunction { FunctionId = 1, FunctionKey = "CommissionGroupList55" } };
            var userSession = new UserSession { PortalFunctionList = functionList, ApplicationMode = Model.Core.DataDomain.AppMode.Parasitic };

            // Action
            var result = _sessionService.IsAllowedEditCommission(userSession);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAllowedEditCommission_ApplicationModeIsParasiticAndPortalFunctionListNotContainCorrectKey_ReturnFalse()
        {
            // Arrange
            var functionList = new List<PortalSystemFunction> { new PortalSystemFunction { FunctionId = 1, FunctionKey = "CommissionGroupList56" } };
            var userSession = new UserSession { PortalFunctionList = functionList, ApplicationMode = Model.Core.DataDomain.AppMode.Parasitic };

            // Action
            var result = _sessionService.IsAllowedEditCommission(userSession);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAllowedEditCommission_ApplicationModeIsNotParasiticAndFunctionIdListContainCorrectKey_ReturnTrue()
        {
            // Arrange
            var functionIdList = new List<int> { 55 };
            var userSession = new UserSession { FunctionIdList = functionIdList, ApplicationMode = Model.Core.DataDomain.AppMode.Self };

            // Action
            var result = _sessionService.IsAllowedEditCommission(userSession);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAllowedEditCommission_ApplicationModeIsNotParasiticAndFunctionIdListNotContainCorrectKey_ReturnFalse()
        {
            // Arrange
            var functionIdList = new List<int> { 56 };
            var userSession = new UserSession { FunctionIdList = functionIdList, ApplicationMode = Model.Core.DataDomain.AppMode.Self };

            // Action
            var result = _sessionService.IsAllowedEditCommission(userSession);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAllowedSpecialBA_ApplicationModeIsParasiticAndPortalFunctionListContainCorrectKey_ReturnTrue()
        {
            // Arrange
            var functionList = new List<PortalSystemFunction> { new PortalSystemFunction { FunctionId = 1, FunctionKey = "SpecialBA54" } };
            var userSession = new UserSession { PortalFunctionList = functionList, ApplicationMode = Model.Core.DataDomain.AppMode.Parasitic };

            // Action
            var result = _sessionService.IsAllowedSpecialBA(userSession);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAllowedSpecialBA_ApplicationModeIsParasiticAndPortalFunctionListNotContainCorrectKey_ReturnFalse()
        {
            // Arrange
            var functionList = new List<PortalSystemFunction> { new PortalSystemFunction { FunctionId = 1, FunctionKey = "SpecialBA55" } };
            var userSession = new UserSession { PortalFunctionList = functionList, ApplicationMode = Model.Core.DataDomain.AppMode.Parasitic };

            // Action
            var result = _sessionService.IsAllowedSpecialBA(userSession);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAllowedSpecialBA_ApplicationModeIsNotParasiticAndFunctionIdListContainCorrectKey_ReturnTrue()
        {
            // Arrange
            var functionIdList = new List<int> { 54 };
            var userSession = new UserSession { FunctionIdList = functionIdList, ApplicationMode = Model.Core.DataDomain.AppMode.Self };

            // Action
            var result = _sessionService.IsAllowedSpecialBA(userSession);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAllowedSpecialBA_ApplicationModeIsNotParasiticAndFunctionIdListNotContainCorrectKey_ReturnFalse()
        {
            // Arrange
            var functionIdList = new List<int> { 55 };
            var userSession = new UserSession { FunctionIdList = functionIdList, ApplicationMode = Model.Core.DataDomain.AppMode.Self };

            // Action
            var result = _sessionService.IsAllowedSpecialBA(userSession);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GenerateAgentListXml_CurrencyIdIsNumberAndPositive_ReturnCorrectXmlString()
        {
            // Arrange
            const int UserId = 1, LicenseeId = 1;
            var currencyUserList = new List<CurrencyUser>
            {
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 1,
                    CurrencyName = "USD",
                    AgentId = 2,
                    AgentIdList = new int[] { 1, 2 }
                },
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 20,
                    CurrencyName = "UUS",
                    AgentId = 3,
                    AgentIdList = new int[] { 3, 4, 5 }
                }
            };
            _currencyService.GetCurrenciesByUser(LicenseeId, UserId).Returns(currencyUserList);
            CreateDefaultNewSession();
            var expectedResult = "<ROOT><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></ROOT>";

            // Action
            var result = _sessionService.GenerateAgentListXml(0, true);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateAgentListXml_CurrencyIdIsNumberAndNegative_ReturnCorrectXmlString()
        {
            // Arrange
            const int UserId = 1, LicenseeId = 1;
            var currencyUserList = new List<CurrencyUser>
            {
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 1,
                    CurrencyName = "USD",
                    AgentId = 2,
                    AgentIdList = new int[] { 1, 2 }
                }
            };
            _currencyService.GetCurrenciesByUser(LicenseeId, UserId).Returns(currencyUserList);
            CreateDefaultNewSession();
            var expectedResult = "<ROOT><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></ROOT>";

            // Action
            var result = _sessionService.GenerateAgentListXml(-2, true);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateAgentListXml_CurrencyIdIsNumberAndExcludeTestDataParamIsFalse_ReturnCorrectXmlString()
        {
            // Arrange
            const int UserId = 1, LicenseeId = 1;
            var currencyUserList = new List<CurrencyUser>
            {
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 1,
                    CurrencyName = "USD",
                    AgentId = 2,
                    AgentIdList = new int[] { 1, 2 }
                },
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 2,
                    CurrencyName = "EUR",
                    AgentId = 3,
                    AgentIdList = new int[] { 3 }
                }
            };
            _currencyService.GetCurrenciesByUser(LicenseeId, UserId).Returns(currencyUserList);
            CreateDefaultNewSession();
            var expectedResult = "<ROOT><Agent AgentId=\"3\" Currency=\"2\"/></ROOT>";

            // Action
            var result = _sessionService.GenerateAgentListXml(2, false);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateAgentListXml_CurrencyIdIsListAndContainUUSCurrency_ReturnCorrectXmlString()
        {
            // Arrange
            const int UserId = 1, LicenseeId = 1;
            var currencyUserList = new List<CurrencyUser>
            {
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 1,
                    CurrencyName = "USD",
                    AgentId = 2,
                    AgentIdList = new int[] { 1, 2 }
                },
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 2,
                    CurrencyName = "EUR",
                    AgentId = 3,
                    AgentIdList = new int[] { 3 }
                }
            };
            _currencyService.GetCurrenciesByUser(LicenseeId, UserId).Returns(currencyUserList);
            CreateDefaultNewSession();
            var expectedResult = "<ROOT><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/><Agent AgentId=\"3\" Currency=\"2\"/></ROOT>";

            // Action
            var result = _sessionService.GenerateAgentListXml(new int[] { 0, 1, 20 }, false);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateAgentListXml_CallWithKeenOceanLicensee_ReturnXmlRootString()
        {
            // Arrange
            const int UserId = 1, LicenseeId = 96;
            var currencyUserList = new List<CurrencyUser>
            {
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 1,
                    CurrencyName = "USD",
                    AgentId = 2,
                    AgentIdList = new int[] { 1, 2 }
                },
                new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = 66,
                    CurrencyName = "EUR",
                    AgentId = 3,
                    AgentIdList = new int[] { 3 }
                }
            };
            _currencyService.GetCurrenciesByUser(LicenseeId, UserId).Returns(currencyUserList);
            CreateDefaultNewSession(96);
            var expectedResult = "<ROOT><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/><Agent AgentId=\"3\" Currency=\"66\"/></ROOT>";

            // Action
            var result = _sessionService.GenerateAgentListXml(new int[] { 0, 1 }, false);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("ASDF")]
        [TestCase("12234")]
        public void CaptchaCode_Set_ValueIsSetCorrectly(string value)
        {
            // Arrange
            const string CaptchaCodeKey = "CaptchaCodeKey";
            CreateDefaultNewSession();
            _sessionService.CaptchaCode = value;
            var expectedResult = value;

            // Action
            var result = HttpContext.Current.Session[CaptchaCodeKey] as string;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("ASDF")]
        [TestCase("12234")]
        public void CaptchaCode_Get_ValueIsGotCorrectly(string value)
        {
            // Arrange
            const string CaptchaCodeKey = "CaptchaCodeKey";
            CreateDefaultNewSession();
            HttpContext.Current.Session[CaptchaCodeKey] = value;
            var expectedResult = value;

            // Action
            var result = _sessionService.CaptchaCode;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static void MockHttpContext()
        {
            var httpContext = new HttpContext(
                            new HttpRequest(string.Empty, "http://tempuri.org", string.Empty),
                            new HttpResponse(new StringWriter()));

            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
            HttpContext.Current = httpContext;
        }

        private void CreateDefaultNewSession(int licenseeId = 1)
        {
            MockHttpContext();
            const int UserId = 1;
            const int OnUserId = 2;
            const string UserName = "user1";
            const bool IsInternal = false;

            _sessionService.CreateNewSession(UserId, OnUserId, UserName, IsInternal, licenseeId);
        }
    }
}