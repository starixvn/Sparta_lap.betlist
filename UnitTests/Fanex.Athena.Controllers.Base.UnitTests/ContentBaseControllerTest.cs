namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using Model.Core.DataDomain;
    using Model.Core.MetaModel;
    using NUnit.Framework;

    [TestFixture]
    public class ContentBaseControllerTest : CultureBaseControllerHelperTest
    {
        private FakeContentBaseController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new FakeContentBaseController(LangService, SessionService, AppSettings)
            {
                ControllerContext = ControllerContext
            };
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
    }
}