namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using Session.Enum;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class BaseControllerTest : BaseControllerHelperTest
    {
        private FakeBaseController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new FakeBaseController(LangService, SessionService, AppSettings, ClientViewHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void SetConvetedUSDCurrencyId_IsBaseUsdLicensee_SetCurrencyIsUsd()
        {
            // Arrange
            var metaModel = new BaseMetaModel
            {
                LicenseeId = 1
            };

            StubGetValueAppSettings("BaseUsdLicensees", metaModel.LicenseeId.ToString());

            // Action
            _controller.SetConvetedUSDCurrencyId(metaModel);

            // Assert
            Assert.That(metaModel.CurrencyId, Is.EqualTo(BaseCurrencyType.Usd));
        }

        [Test]
        public void SetConvetedUSDCurrencyId_IsNotBaseUsdLicensee_SetCurrencyIsAll()
        {
            // Arrange
            var metaModel = new BaseMetaModel
            {
                LicenseeId = 1
            };

            StubGetValueAppSettings("BaseUsdLicensees", null);

            // Action
            _controller.SetConvetedUSDCurrencyId(metaModel);

            // Assert
            Assert.That(metaModel.CurrencyId, Is.EqualTo(BaseCurrencyType.All));
        }

        [Test]
        public void SetConvetedUSDCurrencyName_IsBaseUsdLicensee_SetBaseCurrencyIsUsd()
        {
            // Arrange
            var metaModel = new BaseMetaModel
            {
                LicenseeId = 1
            };

            StubGetValueAppSettings("BaseUsdLicensees", metaModel.LicenseeId.ToString());

            const string ExpectedBaseCurrency = "usd";

            // Action
            _controller.SetConvetedUSDCurrencyName(metaModel);

            // Assert
            Assert.That(metaModel.BaseCurrency, Is.EqualTo(ExpectedBaseCurrency));
        }

        [Test]
        public void SetConvetedUSDCurrencyName_IsNotBaseUsdLicensee_SetBaseCurrencyIsEur()
        {
            // Arrange
            var metaModel = new BaseMetaModel
            {
                LicenseeId = 1
            };

            StubGetValueAppSettings("BaseUsdLicensees", null);

            const string ExpectedBaseCurrency = "eur";

            // Action
            _controller.SetConvetedUSDCurrencyName(metaModel);

            // Assert
            Assert.That(metaModel.BaseCurrency, Is.EqualTo(ExpectedBaseCurrency));
        }

        [Test]
        public void Initialize_WhenCall_SetPageCulture()
        {
            // Arrange
            const string Language = "en";
            LangService.GetCurrentLanguage().Returns(Language);

            // Action
            _controller.Initialize(Url.RequestContext);

            // Assert
            LangService.Received().SetPageCulture(Language);
        }

        [Test]
        public void View_ClientViewTypeIsNone_ReturnViewNameBaseOnAppSetting()
        {
            // Arrange
            AppSettings.ClientViewType.Returns(ClientViewType.Default.ToString("d"));

            StubUserSessionData(new UserSession
            {
                ViewType = ClientViewType.None
            });

            const string ViewName = "TestView";

            // Action
            var actual = _controller.View(ViewName, string.Empty, null);

            // Assert
            Assert.That(actual.ViewName, Is.EqualTo(ViewName));
        }

        [Test]
        public void View_UserSessionIsNull_ReturnViewNameBaseOnAppSetting()
        {
            // Arrange
            AppSettings.ClientViewType.Returns(ClientViewType.Default.ToString("d"));

            StubUserSessionData(null);

            const string ViewName = "TestView";

            // Action
            var actual = _controller.View(ViewName, string.Empty, null);

            // Assert
            Assert.That(actual.ViewName, Is.EqualTo(ViewName));
        }

        [Test]
        public void View_ClientViewTypeIsNotNone_ReturnViewNameBaseOnUserSession()
        {
            // Arrange
            StubUserSessionData(new UserSession
            {
                ViewType = ClientViewType.Desktop
            });

            const string ViewName = "";

            // Action
            var actual = _controller.View(ViewName, string.Empty, null);

            // Assert
            Assert.That(actual.ViewName, Is.EqualTo(ViewName));
        }
    }
}