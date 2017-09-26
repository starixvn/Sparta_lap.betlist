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
    public class HomeDrawAwayNoBetTotalBetControllerTest : BaseControllerHelperTest
    {
        private IHomeDrawAwayNoBetService _homeDrawAwayNoBetService;

        private HomeDrawAwayNoBetTotalBetController _controller;

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

            _homeDrawAwayNoBetService = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new HomeDrawAwayNoBetTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _homeDrawAwayNoBetService);
        }

        [TearDown]
        public void TearDown()
        {
            _controller = null;
        }

        [Test]
        public void Index_WhenGet_ReturnMainView()
        {
            // Arrange

            // Action
            var viewResult = _controller.Index() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HomeDrawAwayNoBet/Main.cshtml"));
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as HomeDrawAwayNoBetViewModel;

            // Assert
            _homeDrawAwayNoBetService.ReceivedWithAnyArgs().GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>());
            Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            // Arrange
            var metaModel = new HomeDrawAwayNoBetMetaModel
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
            _homeDrawAwayNoBetService = Substitute.For<IHomeDrawAwayNoBetService>();

            _homeDrawAwayNoBetService
                .GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>())
                .Returns(new List<HomeDrawAwayNoBet> { new HomeDrawAwayNoBet() });
        }
    }
}