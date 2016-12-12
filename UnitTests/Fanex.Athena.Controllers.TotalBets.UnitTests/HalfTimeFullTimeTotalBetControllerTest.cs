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
    public class HalfTimeFullTimeTotalBetControllerTest : BaseControllerTest
    {
        private IHalfTimeFullTimeService _halfTimeFullTimeService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _halfTimeFullTimeService = Substitute.For<IHalfTimeFullTimeService>();
            HalfTimeFullTime total;
            _halfTimeFullTimeService.GetHalfTimeFullTimeDataList(Arg.Any<HalfTimeFullTimeMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new HalfTimeFullTime();
                    return new List<HalfTimeFullTime> { new HalfTimeFullTime() };
                });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _halfTimeFullTimeService = null;
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HalfTimeFullTime/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                HalfTimeFullTime total = null;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as HalfTimeFullTimeViewModel;

                // Assert
                _halfTimeFullTimeService.ReceivedWithAnyArgs().GetHalfTimeFullTimeDataList(Arg.Any<HalfTimeFullTimeMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            using (var controller = CreateController())
            {
                var metaModel = new HalfTimeFullTimeMetaModel
                {
                    CurrencyId = -999
                };

                // Action
                var viewResult = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(viewResult.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        private HalfTimeFullTimeTotalBetController CreateController()
        {
            return new HalfTimeFullTimeTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _halfTimeFullTimeService);
        }
    }
}