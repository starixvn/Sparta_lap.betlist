namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Results;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using DataAccess.Report.Interfaces;
    using Excel.Report;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Common.Interfaces;

    public class WinLossByLeagueReportControllerTest : BaseApiControllerHelperTest
    {
        private IWinLossByLeagueDataAccess _dataAccess;
        private WinLossByLeagueReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            var productService = Substitute.For<IProductService>();
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            var appSettings = Substitute.For<IAppSettings>();
            _dataAccess = Substitute.For<IWinLossByLeagueDataAccess>();

            _controller = Substitute.ForPartsOf<WinLossByLeagueReportController>(
                SessionService, 
                ProductService, 
                LangService, 
                balanceUpDataAccess, 
                _dataAccess,
                appSettings);
            _controller.Request = new HttpRequestMessage();

            balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp());
            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _dataAccess = null;
            _controller = null;
        }

        [Test]
        public void Index_InvalidModel_ReturnBadRequestWithCorrectMessage()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel { CurrencyId = 1 };

            // Action
            var result = _controller.Index(metaModel) as BadRequestErrorMessageResult;

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.InvalidParams));
        }

        [Test]
        public void Index_Level0_GetCorrectDataFromService()
        {
            // Arrange
            var dataListResult = new List<WinLossByLeague>
            {
                new WinLossByLeague
                {
                    BetCount = 100
                },
                new WinLossByLeague
                {
                    BetCount = 200
                }
            };

            _dataAccess.GetSummary(Arg.Any<WinLossByLeagueMetaModel>()).Returns(dataListResult);

            // Action
            var result = _controller.Index(new WinLossByLeagueMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetSummary(Arg.Any<WinLossByLeagueMetaModel>());
                Assert.That(result.Data.Count, Is.EqualTo(2));
                Assert.That(result.Total.BetCount, Is.EqualTo(300));
            });
        }

        [Test]
        public void Index_Level1_GetCorrectDataFromService()
        {
            // Arrange
            const decimal ExpectedAmountDue = 999;
            const int ExpectedTotalRows = 1000;
            var dataListResult = new List<WinLossByLeague>
            {
                new WinLossByLeague()
            };
            var expectedResult = new ReportResultModel<WinLossByLeague>
            {
                DataList = dataListResult,
                TotalData = new WinLossByLeague
                {
                    AmountDue = ExpectedAmountDue
                },
                TotalRows = ExpectedTotalRows
            };

            _dataAccess.GetCustomers(Arg.Any<WinLossByLeagueMetaModel>())
                .ReturnsForAnyArgs(expectedResult);

            // Action
            var result = _controller.Index(new WinLossByLeagueMetaModel
            {
                Level = 1
            });

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.ReceivedWithAnyArgs().GetCustomers(Arg.Any<WinLossByLeagueMetaModel>());
                Assert.That(result.Total.AmountDue, Is.EqualTo(ExpectedAmountDue));
                Assert.That(result.TotalRows, Is.EqualTo(ExpectedTotalRows));
                Assert.That(result.Data.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Excel_InvalidModel_ReturnBadRequestWithCorrectMessage()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel { CurrencyId = 1 };

            // Action
            var result = _controller.Excel(metaModel);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void Excel_ValidModel_ExportWithCorrectData()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel
            {
                CurrencyId = -1,
                BaseCurrency = "ABC"
            };
            var excel = Substitute.ForPartsOf<WinlossByLeagueExcel>(
                string.Empty,
                new ReportResultModel<WinLossByLeague>
                {
                    DataList = new List<WinLossByLeague>(),
                    TotalData = new WinLossByLeague()
                },
                metaModel,
                true);
            excel.When(x => x.Export()).DoNotCallBase();

            ReportResultModel<WinLossByLeague> resultModel = null;
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Do<ReportResultModel<WinLossByLeague>>(x => resultModel = x),
                Arg.Any<WinLossByLeagueMetaModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
            _dataAccess.GetSummary(metaModel).Returns(new List<WinLossByLeague>
            {
                new WinLossByLeague()
            });

            // Action
            _controller.Excel(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                excel.Received().Export();
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>());
                Assert.That(resultModel.DataList.Count(), Is.EqualTo(1));
                Assert.That(metaModel.PageIndex, Is.EqualTo(1));
                Assert.That(metaModel.PageSize, Is.EqualTo(int.MaxValue - 1));
            });
        }
    }
}