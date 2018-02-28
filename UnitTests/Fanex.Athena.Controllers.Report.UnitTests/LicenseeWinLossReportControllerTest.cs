namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;

    [TestFixture]
    public class LicenseeWinLossReportControllerTest : BaseControllerHelperTest
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
                var viewModel = viewResult.Model as LicenseeWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    StringAssert.Contains("Report/LicenseeWinLoss/Main.cshtml", viewResult.ViewName);
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.Param.DrawFilter, Is.EqualTo(DrawFilterType.Normal));
                    Assert.That(viewModel.Param.ReportBy, Is.EqualTo(ReportByType.Daily));
                });
            }
        }

        private LicenseeWinLossReportController CreateController()
        {
            var reportSettingService = Substitute.For<IReportSettingService>();
            reportSettingService.GetSetting(
                SessionService.LicenseeSession.LicenseeName,
                PermissionKeys.ReportLicenseeWinLoss).Returns(new ReportSetting());
            var productService = Substitute.For<IProductService>();
            productService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.LicenseeWinLoss).Returns(new ProductAccess(new List<Product>()));
            return new LicenseeWinLossReportController(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                productService,
                reportSettingService);
        }
    }
}