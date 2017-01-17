namespace Fanex.Athena.Controllers.UnitTests
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
    public class HandicapOverUnderTotalBetControllerTest : BaseControllerTest
    {
        private IHandicapOverUnderService _handicapOverUnderService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _handicapOverUnderService = Substitute.For<IHandicapOverUnderService>();
            HandicapOverUnder total;
            _handicapOverUnderService.GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new HandicapOverUnder();
                    return new List<HandicapOverUnder> { new HandicapOverUnder() };
                });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _handicapOverUnderService = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HandicapOverUnder/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                HandicapOverUnder total = null;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as HandicapOverUnderViewModel;

                // Assert
                _handicapOverUnderService.ReceivedWithAnyArgs().GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ReturnCorrectViewName()
        {
            // Arrange
            using (var controller = CreateController())
            {
                var metaModel = new HandicapOverUnderMetaModel
                {
                    CurrencyId = 0
                };

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HandicapOverUnder/Main.cshtml"));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            using (var controller = CreateController())
            {
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
        public void Index_LicenseeIdNOTInLicenseeOfferNewForecastSetting_ReturNormalLicenseeView()
        {
            // Arrange
            using (var controller = CreateController())
            {
                AppSettings.GetValue("LicenseeOfferNewForecast").Returns("1001");
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
        public void Index_LicenseeIdInLicenseeOfferNewForecastSetting_ReturnBodogLicenseeView()
        {
            // Arrange
            using (var controller = CreateController())
            {
                AppSettings.GetValue("LicenseeOfferNewForecast").Returns("100");
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

        private HandicapOverUnderTotalBetController CreateController()
        {
            return new HandicapOverUnderTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _handicapOverUnderService);
        }
    }
}