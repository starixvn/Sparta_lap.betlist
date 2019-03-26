namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.TotalBet.Interfaces;
    using Fanex.Athena.Controllers.TotalBets;
    using Model.Core.ResultModel;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HandicapOverUnderTotalBetControllerTest : CultureBaseControllerHelperTest
    {
        private IHandicapOverUnderDataAccess _handicapOverUnderDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();

            SepUpForService();

            StubGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _handicapOverUnderDataAccess = null;
        }

        [Test]
        public void Index_WhenGet_ReturnMainView()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;

                // Assert
                StringAssert.Contains("TotalBets/HandicapOverUnder/Main.cshtml", viewResult.ViewName);
            }
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as HandicapOverUnderViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    _handicapOverUnderDataAccess.ReceivedWithAnyArgs().GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>());
                    Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
                });
            }
        }

        [Test]
        public void Index_WhenPostWithValidCurrency_ReturnMainView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new HandicapOverUnderMetaModel
                {
                    CurrencyId = 0
                };

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;

                // Assert
                StringAssert.Contains("TotalBets/HandicapOverUnder/Main.cshtml", viewResult.ViewName);
            }
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new HandicapOverUnderMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        [Test]
        public void Index_LicenseeIsNotOfferedNewForecast_ReturnNormalLicenseeView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetValueAppSettings("LicenseeOfferNewForecast", "1001");

                var metaModel = new HandicapOverUnderMetaModel
                {
                    CurrencyId = 0
                };

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.LicenseeView, Is.EqualTo("ContentData.cshtml"));
            }
        }

        [Test]
        public void Index_LicenseeIsOfferedNewForecastSetting_ReturnBodogLicenseeView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                var metaModel = new HandicapOverUnderMetaModel
                {
                    CurrencyId = 0
                };

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.LicenseeView, Is.EqualTo("ContentDataBodog.cshtml"));
            }
        }

        [Test]
        public void Index_LicenseeOfferedNewForecastIsNotSet_ReturnNormalLicenseeView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(new int[] { });

                var metaModel = new HandicapOverUnderMetaModel
                {
                    CurrencyId = 0
                };

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.LicenseeView, Is.EqualTo("ContentData.cshtml"));
            }
        }

        private void SepUpForService()
        {
            _handicapOverUnderDataAccess = Substitute.For<IHandicapOverUnderDataAccess>();

            _handicapOverUnderDataAccess
                .GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>())
                .Returns(new ReportResultModel<HandicapOverUnder>
                {
                    TotalData = new HandicapOverUnder(),
                    DataList = new List<HandicapOverUnder> { new HandicapOverUnder() }
                });
        }

        private HandicapOverUnderTotalBetController CreateController()
        {
            return new HandicapOverUnderTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _handicapOverUnderDataAccess);
        }
    }
}