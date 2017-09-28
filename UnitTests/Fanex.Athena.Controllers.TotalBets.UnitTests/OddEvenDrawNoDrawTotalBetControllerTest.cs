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
    public class OddEvenDrawNoDrawTotalBetControllerTest : BaseControllerHelperTest
    {
        private IOddEvenDrawNoDrawDataAccess _oddEvenDrawNoDrawDataAccess;

        private OddEvenDrawNoDrawTotalBetController _controller;

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

            _oddEvenDrawNoDrawDataAccess = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new OddEvenDrawNoDrawTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _oddEvenDrawNoDrawDataAccess);
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
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/OddEvenDrawNoDraw/Main.cshtml"));
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            // Arrange
            OddEvenDrawNoDraw total;

            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as OddEvenDrawNoDrawViewModel;

            // Assert
            _oddEvenDrawNoDrawDataAccess.ReceivedWithAnyArgs().GetOddEvenDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out total);
            Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            // Arrange
            var metaModel = new OddEvenDrawNoDrawMetaModel
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
            _oddEvenDrawNoDrawDataAccess = Substitute.For<IOddEvenDrawNoDrawDataAccess>();

            OddEvenDrawNoDraw total;
            _oddEvenDrawNoDrawDataAccess
                .GetOddEvenDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new OddEvenDrawNoDraw();
                    return new List<OddEvenDrawNoDraw> { new OddEvenDrawNoDraw() };
                });
        }
    }
}