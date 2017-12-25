namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public class AgCasinoWinLossReportControllerTest : BaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as ThirdPartyWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/AgCasinoWinLoss/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                });
            }
        }

        private AgCasinoWinLossReportController CreateController()
        {
            return new AgCasinoWinLossReportController(
                LangService,
                SessionService,
                AppSettings,
                Substitute.For<IProductService>(),
                ClientViewHelper);
        }
    }
}