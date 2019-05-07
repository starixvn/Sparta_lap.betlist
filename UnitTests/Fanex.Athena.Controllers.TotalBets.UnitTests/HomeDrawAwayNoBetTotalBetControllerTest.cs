namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
    public class HomeDrawAwayNoBetTotalBetControllerTest : CultureBaseControllerHelperTest
    {
        private IHomeDrawAwayNoBetDataAccess _homeDrawAwayNoBetDataAccess;

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

        [Test]
        public async Task Index_WhenGet_ReturnMainView()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = await controller.Index() as ViewResult;

                // Assert
                StringAssert.Contains("TotalBets/HomeDrawAwayNoBet/Main.cshtml", viewResult.ViewName);
            }
        }

        [Test]
        public async Task Index_WhenGet_GetDataFromService()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = await controller.Index() as ViewResult;
                var viewModel = viewResult.Model as HomeDrawAwayNoBetViewModel;

                // Assert
                await _homeDrawAwayNoBetDataAccess.ReceivedWithAnyArgs(1).GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>());
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new HomeDrawAwayNoBetMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = await controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        private HomeDrawAwayNoBetTotalBetController CreateController()
        {
            return new HomeDrawAwayNoBetTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _homeDrawAwayNoBetDataAccess);
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