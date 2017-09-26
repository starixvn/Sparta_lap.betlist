namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Fanex.Athena.Service.TotalBet.Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using TotalBets;

    [TestFixture]
    public class OddEvenDrawNoDrawTotalBetControllerTest : BaseControllerHelperTest
    {
        private IOddEvenDrawNoDrawService _oddEvenDrawNoDrawService;

        private OddEvenDrawNoDrawTotalBetController _controller;

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

            _oddEvenDrawNoDrawService = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new OddEvenDrawNoDrawTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _oddEvenDrawNoDrawService);
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
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/OddEvenDrawNoDraw/Main.cshtml"));
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            // Arrange
            OddEvenDrawNoDraw total;

            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as OddEvenDrawNoDrawViewModel;

            // Assert
            _oddEvenDrawNoDrawService.ReceivedWithAnyArgs().GetOddEvenDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out total);
            Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            // Arrange
            var metaModel = new OddEvenDrawNoDrawMetaModel
            {
                CurrencyId = -999
            };

            // Action
            var viewResult = _controller.Index(metaModel) as RedirectResult;

            // Assert
            Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
        }

        private void SepUpForService()
        {
            _oddEvenDrawNoDrawService = Substitute.For<IOddEvenDrawNoDrawService>();

            OddEvenDrawNoDraw total;
            _oddEvenDrawNoDrawService
                .GetOddEvenDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new OddEvenDrawNoDraw();
                    return new List<OddEvenDrawNoDraw> { new OddEvenDrawNoDraw() };
                });
        }
    }
}