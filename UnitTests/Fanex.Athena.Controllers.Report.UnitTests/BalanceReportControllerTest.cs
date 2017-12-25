namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DTO.Common;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public class BalanceReportControllerTest : BaseControllerHelperTest
    {
        private IBalanceDataAccess _balanceDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _balanceDataAccess = Substitute.For<IBalanceDataAccess>();
            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as BalanceViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/Balance/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                });
            }
        }

        [Test]
        public void Index_WhenPostToExportExcel_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BalanceMetaModel
                {
                    IsExportExcel = true
                };
                _balanceDataAccess.GetReportData(metaModel).Returns(new Balance());
                var excel = Substitute.ForPartsOf<BalanceExcel>(
                    string.Empty,
                    Arg.Any<ShowingOption>(),
                    Arg.Any<ConvertingOption>(),
                    Arg.Any<Balance>(),
                    string.Empty);
                excel.When(x => x.Export()).DoNotCallBase();

                controller.CreateExcel(
                    string.Empty,
                    Arg.Any<ShowingOption>(),
                    Arg.Any<ConvertingOption>(),
                    Arg.Any<Balance>(),
                    string.Empty)
                    .ReturnsForAnyArgs(excel);

                // Action
                var result = controller.Index(metaModel) as ExcelResult;

                // Assert
                Assert.Multiple(() =>
                {
                    excel.Received(1).Export();
                    Assert.That(result, Is.Not.Null);
                });
            }
        }

        private BalanceReportController CreateController()
        {
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language)
                .Returns(new ProductAccess(new List<Product>()));
            return Substitute.ForPartsOf<BalanceReportController>(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _balanceDataAccess,
                ClientViewHelper
                );
        }
    }
}