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

        private HalfTimeFullTimeTotalBetController _controller;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _halfTimeFullTimeService = Substitute.For<IHalfTimeFullTimeService>();
            _controller = new HalfTimeFullTimeTotalBetController(
                LangService,
                SessionService,
                AppSettings,
                _halfTimeFullTimeService);
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
            _controller.Dispose();
            _controller = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            // Arrange

            // Action
            var viewResult = _controller.Index() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/HalfTimeFullTime/Main.cshtml"));
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            HalfTimeFullTime total;

            // Action
            var viewResult = _controller.Index() as ViewResult;
            var viewModel = viewResult.Model as HalfTimeFullTimeViewModel;

            // Assert
            _halfTimeFullTimeService.Received().GetHalfTimeFullTimeDataList(Arg.Any<HalfTimeFullTimeMetaModel>(), out total);
            Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
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
    }
}