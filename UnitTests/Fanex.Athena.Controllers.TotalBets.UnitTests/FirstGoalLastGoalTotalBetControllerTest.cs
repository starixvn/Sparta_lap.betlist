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
    public class FirstGoalLastGoalTotalBetControllerTest : BaseControllerTest
    {
        private IFirstGoalLastGoalService _firstGoalLastGoalService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _firstGoalLastGoalService = Substitute.For<IFirstGoalLastGoalService>();
            FirstGoalLastGoal total;
            _firstGoalLastGoalService.GetFirstGoalLastGoalDataList(Arg.Any<FirstGoalLastGoalMetaModel>(), out total)
                .Returns(new List<FirstGoalLastGoal> { new FirstGoalLastGoal() });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _firstGoalLastGoalService = null;
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/FirstGoalLastGoal/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                FirstGoalLastGoal total;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as FirstGoalLastGoalViewModel;

                // Assert
                _firstGoalLastGoalService.Received().GetFirstGoalLastGoalDataList(Arg.Any<FirstGoalLastGoalMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            using (var controller = CreateController())
            {
                var metaModel = new FirstGoalLastGoalMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        private FirstGoalLastGoalTotalBetController CreateController()
        {
            return new FirstGoalLastGoalTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _firstGoalLastGoalService);
        }
    }
}