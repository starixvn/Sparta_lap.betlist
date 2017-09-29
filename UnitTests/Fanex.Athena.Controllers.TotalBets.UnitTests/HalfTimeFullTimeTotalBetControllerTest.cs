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
    public class HalfTimeFullTimeTotalBetControllerTest : BaseControllerHelperTest
    {
        private IHalfTimeFullTimeDataAccess _halfTimeFullTimeDataAccess;

        private HalfTimeFullTimeTotalBetController _controller;

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

            _halfTimeFullTimeDataAccess = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new HalfTimeFullTimeTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _halfTimeFullTimeDataAccess);
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
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HalfTimeFullTime/Main.cshtml"));
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            // Arrange
            HalfTimeFullTime total = null;

            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as HalfTimeFullTimeViewModel;

            // Assert
            Assert.Multiple(() =>
            {
                _halfTimeFullTimeDataAccess.ReceivedWithAnyArgs().GetHalfTimeFullTimeDataList(Arg.Any<HalfTimeFullTimeMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            // Arrange
            var metaModel = new HalfTimeFullTimeMetaModel
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
            _halfTimeFullTimeDataAccess = Substitute.For<IHalfTimeFullTimeDataAccess>();

            HalfTimeFullTime total;
            _halfTimeFullTimeDataAccess
                .GetHalfTimeFullTimeDataList(Arg.Any<HalfTimeFullTimeMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new HalfTimeFullTime();
                    return new List<HalfTimeFullTime> { new HalfTimeFullTime() };
                });
        }
    }
}