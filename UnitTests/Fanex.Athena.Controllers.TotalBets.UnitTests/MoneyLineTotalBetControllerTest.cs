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
    public class MoneyLineTotalBetControllerTest : BaseControllerTest
    {
        private IMoneyLineService _moneyLineService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _moneyLineService = Substitute.For<IMoneyLineService>();
            MoneyLine total;
            _moneyLineService.GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out total)
                .Returns(x =>
                {
                    x[1] = new MoneyLine();
                    return new List<MoneyLine> { new MoneyLine() };
                });
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _moneyLineService = null;
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/TotalBets/MoneyLine/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_CallAndGetDataFromService()
        {
            // Arrange
            using (var controller = CreateController())
            {
                MoneyLine total;

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as MoneyLineViewModel;

                // Assert
                _moneyLineService.ReceivedWithAnyArgs().GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out total);
                Assert.That(viewModel.MainData.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Index_InvalidCurrency_ViewNameIsAccessDenied()
        {
            // Arrange
            using (var controller = CreateController())
            {
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

        private MoneyLineTotalBetController CreateController()
        {
            return new MoneyLineTotalBetController(
                            LangService,
                            SessionService,
                            AppSettings,
                            _moneyLineService);
        }
    }
}