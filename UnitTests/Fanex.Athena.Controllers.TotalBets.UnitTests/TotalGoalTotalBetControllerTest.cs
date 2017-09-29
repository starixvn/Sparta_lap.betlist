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
    public class TotalGoalTotalBetControllerTest : BaseControllerHelperTest
    {
        private ITotalGoalDataAccess _totalGoalDataAccess;

        private TotalGoalTotalBetController _controller;

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

            _totalGoalDataAccess = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new TotalGoalTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _totalGoalDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void Index_WhenGet_ReturnMainView()
        {
            // Action
            var viewResult = _controller.Index() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/TotalGoal/Main.cshtml"));
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            // Arrange
            TotalGoal total;

            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as TotalGoalViewModel;

            // Assert
            _totalGoalDataAccess.ReceivedWithAnyArgs().GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out total);
            Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            // Arrange
            var metaModel = new TotalGoalMetaModel
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
            _totalGoalDataAccess = Substitute.For<ITotalGoalDataAccess>();

            TotalGoal total;
            _totalGoalDataAccess
                .GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new TotalGoal();
                    return new List<TotalGoal> { new TotalGoal() };
                });
        }
    }
}