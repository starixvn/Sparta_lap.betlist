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
    public class OutrightTotalBetControllerTest : BaseControllerHelperTest
    {
        private IOutrightDataAccess _outrightDataAccess;

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

            _outrightDataAccess = null;
        }

        [Test]
        public void Index_WhenGet_ReturnMainView()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;

                // Assert
                StringAssert.Contains("TotalBets/Outright/Main.cshtml", viewResult.ViewName);
            }
        }

        [Test]
        public void Index_WhenGet_GetDataFromService()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as OutrightViewModel;

                // Assert
                _outrightDataAccess.ReceivedWithAnyArgs().GetOutrightDataList(Arg.Any<OutrightMetaModel>());
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_WhenPostWithInvalidCurrency_AccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new OutrightMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        private OutrightTotalBetController CreateController()
        {
            return new OutrightTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            ClientViewHelper,
                            _outrightDataAccess);
        }

        private void SepUpForService()
        {
            _outrightDataAccess = Substitute.For<IOutrightDataAccess>();

            _outrightDataAccess
                .GetOutrightDataList(Arg.Any<OutrightMetaModel>())
                .Returns(new List<Outright> { new Outright() });
        }
    }
}