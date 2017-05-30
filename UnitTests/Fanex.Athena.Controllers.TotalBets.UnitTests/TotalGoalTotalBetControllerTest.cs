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
    public class TotalGoalTotalBetControllerTest : BaseControllerTest
    {
        private ITotalGoalService _totalGoalService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _totalGoalService = Substitute.For<ITotalGoalService>();
            TotalGoal total;
            _totalGoalService.GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new TotalGoal();
                    return new List<TotalGoal> { new TotalGoal() };
                });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _totalGoalService = null;
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/TotalGoal/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                TotalGoal total;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as TotalGoalViewModel;

                // Assert
                _totalGoalService.ReceivedWithAnyArgs().GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            using (var controller = CreateController())
            {
                var metaModel = new TotalGoalMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        private TotalGoalTotalBetController CreateController()
        {
            return new TotalGoalTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _totalGoalService);
        }
    }
}