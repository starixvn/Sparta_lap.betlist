namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Fanex.Athena.Controllers.TotalBets;
    using Fanex.Athena.Service.TotalBet.Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HandicapOverUnderTotalBetControllerTest : BaseControllerHelperTest
    {
        private IHandicapOverUnderService _handicapOverUnderService;

        private HandicapOverUnderTotalBetController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockDefaultData();

            SepUpForService();

            MockGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _handicapOverUnderService = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new HandicapOverUnderTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _handicapOverUnderService);
        }

        [TearDown]
        public void TearDown()
        {
            _controller = null;
        }

        [Test]
        public void Index_WhenGet_ReturnMainView()
        {
            // Action
            var viewResult = _controller.Index() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HandicapOverUnder/Main.cshtml"));
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            // Arrange
            HandicapOverUnder total;

            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as HandicapOverUnderViewModel;

            // Assert
            Assert.Multiple(() =>
            {
                _handicapOverUnderService.ReceivedWithAnyArgs().GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Index_WhenPostWithValidCurrency_ReturnMainView()
        {
            // Arrange
            var metaModel = new HandicapOverUnderMetaModel
            {
                CurrencyId = 0
            };

            // Action
            var viewResult = _controller.Index(metaModel) as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HandicapOverUnder/Main.cshtml"));
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            // Arrange
            var metaModel = new HandicapOverUnderMetaModel
            {
                CurrencyId = -999
            };

            // Action
            var viewResult = _controller.Index(metaModel) as RedirectResult;

            // Assert
            Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
        }

        [Test]
        public void Index_LicenseeIsNotOfferedNewForecast_ReturnNormalLicenseeView()
        {
            // Arrange
            MockGetValueAppSettings("LicenseeOfferNewForecast", "1001");

            var metaModel = new HandicapOverUnderMetaModel
            {
                CurrencyId = 0
            };

            // Action
            var viewResult = _controller.Index(metaModel) as ViewResult;

            // Assert
            Assert.That(viewResult.ViewBag.LicenseeView, Is.EqualTo("ContentData.cshtml"));
        }

        [Test]
        public void Index_LicenseeIsOfferedNewForecastSetting_ReturnBodogLicenseeView()
        {
            // Arrange
            MockGetValueAppSettings("LicenseeOfferNewForecast", SessionLicenseeId.ToString());

            var metaModel = new HandicapOverUnderMetaModel
            {
                CurrencyId = 0
            };

            // Action
            var viewResult = _controller.Index(metaModel) as ViewResult;

            // Assert
            Assert.That(viewResult.ViewBag.LicenseeView, Is.EqualTo("ContentDataBodog.cshtml"));
        }

        [Test]
        public void Index_LicenseeOfferedNewForecastIsNotSet_ReturnNormalLicenseeView()
        {
            // Arrange
            MockGetValueAppSettings("LicenseeOfferNewForecast", string.Empty);

            var metaModel = new HandicapOverUnderMetaModel
            {
                CurrencyId = 0
            };

            // Action
            var viewResult = _controller.Index(metaModel) as ViewResult;

            // Assert
            Assert.That(viewResult.ViewBag.LicenseeView, Is.EqualTo("ContentData.cshtml"));
        }

        private void SepUpForService()
        {
            _handicapOverUnderService = Substitute.For<IHandicapOverUnderService>();

            HandicapOverUnder total;
            _handicapOverUnderService
                .GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new HandicapOverUnder();
                    return new List<HandicapOverUnder> { new HandicapOverUnder() };
                });
        }
    }
}