namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Web.ResultExtension;

    [TestFixture]
    public partial class MatchWinLossReportControllerTest : CultureBaseControllerHelperTest
    {
        [Test]
        public async Task Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = await controller.Index() as ViewResult;
                var viewModel = viewResult.Model as MatchWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/MatchWinLoss/Main.cshtml"));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                    Assert.That(viewModel.Param.BalanceUpDay, Is.EqualTo(BalanceUpDate.ToShortDateString()));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                });
            }
        }

        [TestCase("", false, 6)]
        [TestCase("BA", false, 8)]
        [TestCase("BA", true, 10)]
        public async Task IndexPost_NotExportExcel_ReturnCorrectViewResult(
            string productCodeList,
            bool isAllowedCustomerComm,
            int expectedColumnCount)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Arrange
                SessionService.LicenseeSession.IsAllowedCustomerComm = isAllowedCustomerComm;
                var metaModel = new MatchWinLossMetaModel
                {
                    CurrencyId = CurrencyId,
                    IsExportExcel = false
                };

                var expectedData = new List<MatchWinLoss>
                {
                    new MatchWinLoss()
                };
                _matchWinLossService.GetDataList(metaModel).Returns(expectedData);

                // Action
                var viewResult = await controller.Index(metaModel) as ViewResult;
                var viewModel = viewResult.Model as MatchWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/MatchWinLoss/Main.cshtml"));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                    Assert.That(viewModel.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(viewModel.MainResult, Is.EqualTo(expectedData));
                    Assert.That(viewModel.ColumnCount, Is.EqualTo(expectedColumnCount));
                });
            }
        }

        [Test]
        public async Task IndexPost_ExportExcelWithInvalidCurrency_ReturnEmptyFile()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new MatchWinLossMetaModel
                {
                    CurrencyId = -999,
                    IsExportExcel = true
                };

                // Action
                var result = await controller.Index(metaModel) as FileContentResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ContentType, Is.EqualTo("application/vnd.ms-excel"));
                    Assert.That(result.FileDownloadName, Is.EqualTo("Empty.xls"));
                });
            }
        }

        [Test]
        public async Task IndexPost_ExportExcel_ReturnExcelResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                MockCreatingExcel(controller);
                var metaModel = new MatchWinLossMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "All",
                    CurrencyId = 0,
                    IsExportExcel = true
                };
                var expectedData = new DataSet();
                expectedData.Tables.Add(new DataTable("MainData"));
                expectedData.Tables.Add(new DataTable("TotalData"));

                _matchWinLossService.GetDetailDataSet(metaModel).Returns(expectedData);

                // Action
                var result = await controller.Index(metaModel);

                // Assert
                Assert.That(result, Is.InstanceOf<ExcelResult>());
            }
        }
    }
}