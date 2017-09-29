namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.TotalBet.Interfaces;
    using Fanex.Athena.Controllers.TotalBets;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HomeDrawAwayNoBetTotalBetControllerTest : BaseControllerHelperTest
    {
        private IHomeDrawAwayNoBetDataAccess _homeDrawAwayNoBetDataAccess;

        private HomeDrawAwayNoBetTotalBetController _controller;

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

            _homeDrawAwayNoBetDataAccess = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new HomeDrawAwayNoBetTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _homeDrawAwayNoBetDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
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
            _homeDrawAwayNoBetDataAccess.ReceivedWithAnyArgs().GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>());
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
            _homeDrawAwayNoBetDataAccess = Substitute.For<IHomeDrawAwayNoBetDataAccess>();

            _homeDrawAwayNoBetDataAccess
                .GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>())
                .Returns(new List<HomeDrawAwayNoBet> { new HomeDrawAwayNoBet() });
        }
    }
}