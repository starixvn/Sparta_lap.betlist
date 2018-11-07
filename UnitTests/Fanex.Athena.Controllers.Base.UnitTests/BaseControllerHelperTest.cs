namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Extension.Utility;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public abstract class BaseControllerHelperTest
    {
        protected static readonly UserSession DefaultUserSession = new UserSession
        {
            CurrencyList = new Dictionary<int, string> { { 1, "UUS" } },
            UserId = 1,
            Language = "lan",
            UserName = "alex.vn",
            OnUserId = 999,
            FunctionIdList = new int[0]
        };

        protected static readonly LicenseeSession DefaultLicenseeSession = new LicenseeSession
        {
            LicenseeId = 100,
            LicenseeName = "Spondemo"
        };

        protected UrlHelper Url { get; set; }

        protected ControllerContext ControllerContext { get; set; }

        protected ILanguageService LangService { get; set; }

        protected ISessionService SessionService { get; set; }

        protected IAppSettings AppSettings { get; set; }

        protected IClientViewHelper ClientViewHelper { get; set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            LangService = Substitute.For<ILanguageService>();
            SessionService = Substitute.For<ISessionService>();
            AppSettings = Substitute.For<IAppSettings>();
            ClientViewHelper = Substitute.For<IClientViewHelper>();
            ClientViewHelper.GetViewPath(Arg.Any<string>()).Returns(x => (string)x[0]);
            SetUpForContexts();
        }

        [OneTimeTearDown]
        public virtual void FixtureTearDown()
        {
            Url = null;
            ControllerContext = null;
            LangService = null;
            SessionService = null;
            AppSettings = null;
            ClientViewHelper = null;
        }

        protected void SetUpDefaultFakeData()
        {
            StubUserSessionData(DefaultUserSession);

            StubLicenseeSessionData(DefaultLicenseeSession);

            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);
        }

        protected void StubUserSessionData(UserSession fakeUserSession)
        {
            SessionService.UserSession.Returns(fakeUserSession);
        }

        protected void StubLicenseeSessionData(LicenseeSession fakeLicenseeSession)
        {
            SessionService.LicenseeSession.Returns(fakeLicenseeSession);
        }

        protected void StubGetValueAppSettings(string valueName, string fakeValue)
        {
            AppSettings.GetValue(valueName).Returns(fakeValue);
        }

        private void SetUpForContexts()
        {
            ControllerContext = MvcHelper.CreateControllerContext();
            Url = MvcHelper.CreateUrlHelper(ControllerContext.HttpContext);
        }
    }
}