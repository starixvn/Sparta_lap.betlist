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
    public class TotalGoalTotalBetControllerTest : BaseControllerHelperTest
    {
        private ITotalGoalService _totalGoalService;

        private TotalGoalTotalBetController _controller;

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

            _totalGoalService = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new TotalGoalTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _totalGoalService);
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
            _totalGoalService.ReceivedWithAnyArgs().GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out total);
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
            _totalGoalService = Substitute.For<ITotalGoalService>();

            TotalGoal total;
            _totalGoalService
                .GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new TotalGoal();
                    return new List<TotalGoal> { new TotalGoal() };
                });
        }
    }
}