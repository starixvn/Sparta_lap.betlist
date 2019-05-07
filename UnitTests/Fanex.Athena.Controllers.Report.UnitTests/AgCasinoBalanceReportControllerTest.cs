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
    public class AgCasinoBalanceReportControllerTest : CultureBaseControllerHelperTest
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
                var viewModel = viewResult.Model as AgCasinoBalanceViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    StringAssert.Contains("Report/AgCasinoBalance/Main.cshtml", viewResult.ViewName);
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                });
            }
        }

        private AgCasinoBalanceReportController CreateController()
        {
            return new AgCasinoBalanceReportController(
                LangService,
                SessionService,
                AppSettings,
                Substitute.For<IProductService>());
        }
    }
}