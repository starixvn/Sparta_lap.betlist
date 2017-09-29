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

        private MoneyLineTotalBetController _controller;

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

        [SetUp]
        public void SetUp()
        {
            _controller = new MoneyLineTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _moneyLineDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            // Action
            var viewResult = _controller.Index() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/MoneyLine/Main.cshtml"));
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            MoneyLine total;

            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as MoneyLineViewModel;

            // Assert
            _moneyLineDataAccess.ReceivedWithAnyArgs().GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out total);
            Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            var metaModel = new MoneyLineMetaModel
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
            _moneyLineDataAccess = Substitute.For<IMoneyLineDataAccess>();

            MoneyLine total;
            _moneyLineDataAccess.GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new MoneyLine();
                    return new List<MoneyLine> { new MoneyLine() };
                });
        }
    }
}