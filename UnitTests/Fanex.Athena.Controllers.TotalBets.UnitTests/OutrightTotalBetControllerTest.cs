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
    public class OutrightTotalBetControllerTest : BaseControllerHelperTest
    {
        private IOutrightService _outrightService;

        private OutrightTotalBetController _controller;

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

            _outrightService = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new OutrightTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _outrightService);
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
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/Outright/Main.cshtml"));
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as OutrightViewModel;

            // Assert
            _outrightService.ReceivedWithAnyArgs().GetOutrightDataList(Arg.Any<OutrightMetaModel>());
            Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            // Arrange
            var metaModel = new OutrightMetaModel
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
            _outrightService = Substitute.For<IOutrightService>();

            _outrightService
                .GetOutrightDataList(Arg.Any<OutrightMetaModel>())
                .Returns(new List<Outright> { new Outright() });
        }
    }
}