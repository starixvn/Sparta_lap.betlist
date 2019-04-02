namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.TotalBet.Interfaces;
    using Fanex.Athena.Controllers.TotalBets;
    using Model.Core.ResultModel;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TotalGoalTotalBetControllerTest : CultureBaseControllerHelperTest
    {
        private ITotalGoalDataAccess _totalGoalDataAccess;

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

        [Test]
        public async Task Index_WhenGet_ReturnMainView()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = await controller.Index() as ViewResult;

                // Assert
                StringAssert.Contains("TotalBets/TotalGoal/Main.cshtml", viewResult.ViewName);
            }
        }

        [Test]
        public async Task Index_WhenGet_GetDataFromService()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = await controller.Index() as ViewResult;
                var viewModel = viewResult.Model as TotalGoalViewModel;

                // Assert
                await _totalGoalDataAccess.ReceivedWithAnyArgs().GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>());
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new TotalGoalMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = await controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        private TotalGoalTotalBetController CreateController()
        {
            return new TotalGoalTotalBetController(LangService, SessionService, AppSettings, _totalGoalDataAccess);
        }

        private void SepUpForService()
        {
            _totalGoalDataAccess = Substitute.For<ITotalGoalDataAccess>();

            _totalGoalDataAccess
                .GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>())
                .Returns(new ReportResultModel<TotalGoal>
                {
                    TotalData = new TotalGoal(),
                    DataList = new List<TotalGoal> { new TotalGoal() }
                });
        }
    }
}