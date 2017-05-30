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
    public class OddEvenDrawNoDrawTotalBetControllerTest : BaseControllerTest
    {
        private IOddEvenDrawNoDrawService _oddEvenDrawNoDrawService;
        
        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _oddEvenDrawNoDrawService = Substitute.For<IOddEvenDrawNoDrawService>();
            OddEvenDrawNoDraw total;
            _oddEvenDrawNoDrawService.GetOddEvenDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new OddEvenDrawNoDraw();
                    return new List<OddEvenDrawNoDraw> { new OddEvenDrawNoDraw() };
                });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _oddEvenDrawNoDrawService = null;
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/OddEvenDrawNoDraw/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                OddEvenDrawNoDraw total;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as OddEvenDrawNoDrawViewModel;

                // Assert
                _oddEvenDrawNoDrawService.ReceivedWithAnyArgs().GetOddEvenDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new OddEvenDrawNoDrawMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        private OddEvenDrawNoDrawTotalBetController CreateController()
        {
            return new OddEvenDrawNoDrawTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _oddEvenDrawNoDrawService);
        }
    }
}