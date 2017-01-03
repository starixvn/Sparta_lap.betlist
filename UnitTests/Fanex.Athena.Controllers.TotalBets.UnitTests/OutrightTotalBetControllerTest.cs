namespace Fanex.Athena.Controllers.UnitTests
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
    public class OutrightTotalBetControllerTest : BaseControllerTest
    {
        private IOutrightService _outrightService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _outrightService = Substitute.For<IOutrightService>();
            _outrightService.GetOutrightDataList(Arg.Any<OutrightMetaModel>())
                .Returns(new List<Outright> { new Outright() });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _outrightService = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/Outright/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as OutrightViewModel;

                // Assert
                _outrightService.ReceivedWithAnyArgs().GetOutrightDataList(Arg.Any<OutrightMetaModel>());
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            using (var controller = CreateController())
            {
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
                            _outrightService);
        }
    }
}