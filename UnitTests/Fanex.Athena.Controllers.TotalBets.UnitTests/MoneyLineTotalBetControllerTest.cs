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
    public class MoneyLineTotalBetControllerTest : BaseControllerHelperTest
    {
        private IMoneyLineDataAccess _moneyLineDataAccess;

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

            _moneyLineDataAccess = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/MoneyLine/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            using (var controller = CreateController())
            {
                // Arrange
                MoneyLine total;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as MoneyLineViewModel;

                // Assert
                _moneyLineDataAccess.ReceivedWithAnyArgs().GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new MoneyLineMetaModel
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
            _moneyLineDataAccess = Substitute.For<IMoneyLineDataAccess>();

            MoneyLine total;
            _moneyLineDataAccess.GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new MoneyLine();
                    return new List<MoneyLine> { new MoneyLine() };
                });
        }

        private MoneyLineTotalBetController CreateController()
        {
            return new MoneyLineTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _moneyLineDataAccess);
        }
    }
}