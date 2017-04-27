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
    public class HomeDrawAwayNoBetTotalBetControllerTest : BaseControllerTest
    {
        private IHomeDrawAwayNoBetService _homeDrawAwayNoBetService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _homeDrawAwayNoBetService = Substitute.For<IHomeDrawAwayNoBetService>();
            _homeDrawAwayNoBetService.GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>())
                .Returns(new List<HomeDrawAwayNoBet> { new HomeDrawAwayNoBet() });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _homeDrawAwayNoBetService = null;
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HomeDrawAwayNoBet/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as HomeDrawAwayNoBetViewModel;

                // Assert
                _homeDrawAwayNoBetService.ReceivedWithAnyArgs().GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>());
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            using (var controller = CreateController())
            {
                var metaModel = new HomeDrawAwayNoBetMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = controller.Index(metaModel) as RedirectResult;

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
                            _homeDrawAwayNoBetService);
        }
    }
}