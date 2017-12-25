namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.TotalBet.Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using TotalBets;

    [TestFixture]
    public class FirstGoalLastGoalTotalBetControllerTest : BaseControllerHelperTest
    {
        private IFirstGoalLastGoalDataAccess _firstGoalLastGoalDataAccess;

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

            _firstGoalLastGoalDataAccess = null;
        }

        [Test]
        public void Index_WhenGet_ReturnMainView()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/FirstGoalLastGoal/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            using (var controller = CreateController())
            {
                // Arrange
                FirstGoalLastGoal total;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as FirstGoalLastGoalViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    _firstGoalLastGoalDataAccess.Received().GetFirstGoalLastGoalDataList(Arg.Any<FirstGoalLastGoalMetaModel>(), out total);
                    Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
                });
            }
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
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

        private void SepUpForService()
        {
            _firstGoalLastGoalDataAccess = Substitute.For<IFirstGoalLastGoalDataAccess>();

            FirstGoalLastGoal total;
            _firstGoalLastGoalDataAccess
                .GetFirstGoalLastGoalDataList(Arg.Any<FirstGoalLastGoalMetaModel>(), out total)
                .Returns(new List<FirstGoalLastGoal> { new FirstGoalLastGoal() });
        }

        private FirstGoalLastGoalTotalBetController CreateController()
        {
            return new FirstGoalLastGoalTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _firstGoalLastGoalDataAccess,
                            ClientViewHelper);
        }
    }
}