namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class AgCasinoBalanceReportControllerTest : BaseApiReportControllerHelperTest
    {
        private IAgCasinoBalanceDataAccess _dataAccess;
        private AgCasinoBalanceReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();

            _dataAccess = Substitute.For<IAgCasinoBalanceDataAccess>();

            _controller = Substitute.ForPartsOf<AgCasinoBalanceReportController>(_dataAccess, SessionService, ProductService);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _dataAccess = null;
            _controller = null;
        }

        [Test]
        public void Index_WhenCallWithInvalidModel_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = 1,
                DrillDownCurrencyId = 1
            };

            var expectedResult = GetSerializationData();

            // Action
            var result = ToJson(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_NoDataReturned_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = -1
            };

            var resultModel = new SortableSummaryReportResultModel<AgCasinoBalance>
            {
                DataList = new SortableList<AgCasinoBalance>(),
                SummaryTotal = new Dictionary<string, AgCasinoBalance>()
            };
            _dataAccess.GetSummaryBalance(metaModel).Returns(resultModel);

            var expectedResult = GetSerializationData();

            // Action
            var result = ToJson(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_Level0_ReturnCorrectDataFromDataAccess()
        {
            // Arrange
            const int CurrencyId = -2;
            const bool IsExcludedTestData = true;
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = CurrencyId,
                DrillDownLevel = 0,
                DrillDownCurrencyId = 0,
                IsExcludedTestData = IsExcludedTestData
            };

            var today = DateTime.Today;
            var resultModel = new SortableSummaryReportResultModel<AgCasinoBalance>
            {
                DataList = new SortableList<AgCasinoBalance>
                {
                    new AgCasinoBalance { Date = null },
                    new AgCasinoBalance { Date = today },
                    new AgCasinoBalance { Date = today },
                    new AgCasinoBalance { Date = today.AddDays(1) },
                },
                SummaryTotal = new Dictionary<string, AgCasinoBalance>()
            };
            _dataAccess.GetSummaryBalance(metaModel).Returns(resultModel);

            var expectedDataList = new SortableList<AgCasinoBalance>
                {
                    new AgCasinoBalance { Date = null, RowSpan = 1, Index = 1 },
                    new AgCasinoBalance { Date = today, RowSpan = 2, Index = 2 },
                    new AgCasinoBalance { Date = today, RowSpan = -1, Index = 0 },
                    new AgCasinoBalance { Date = today.AddDays(1), RowSpan = 1, Index = 3 }
                };

            // Action
            var result = _controller.Index(metaModel).Data;

            // Assert
            Assert.That(ToJson(result), Is.EqualTo(ToJson(expectedDataList)));
        }

        [Test]
        public void Index_Level1_CallGetAgentBalanceDetailInService_ReturnCorrectData()
        {
            // Arrange
            const int CurrencyId = -2;
            const bool IsExcludedTestData = true;
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = CurrencyId,
                DrillDownLevel = 1,
                DrillDownCurrencyId = 1,
                IsExcludedTestData = IsExcludedTestData
            };

            var resultModel = new SortableSummaryReportResultModel<AgCasinoBalance>
            {
                TotalData = new List<AgCasinoBalance> { new AgCasinoBalance() },
                DataList = new SortableList<AgCasinoBalance> { new AgCasinoBalance() },
                SummaryTotal = new Dictionary<string, AgCasinoBalance>()
            };
            _dataAccess.GetAgentBalanceDetail(metaModel).Returns(resultModel);
            var expectedResult = GetSerializationData(resultModel);

            // Action
            var result = ToJson(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_Level2_CallGetCustomerBalanceDetailInService_ReturnCorrectData()
        {
            // Arrange
            const int CurrencyId = -2;
            const bool IsExcludedTestData = true;
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = CurrencyId,
                DrillDownLevel = 2,
                DrillDownCurrencyId = 1,
                IsExcludedTestData = IsExcludedTestData
            };

            var resultModel = new SortableSummaryReportResultModel<AgCasinoBalance>
            {
                TotalRows = 100,
                TotalData = new SortableList<AgCasinoBalance> { new AgCasinoBalance() },
                DataList = new SortableList<AgCasinoBalance> { new AgCasinoBalance() },
                SummaryTotal = new Dictionary<string, AgCasinoBalance>()
            };
            _dataAccess.GetCustomerBalanceDetail(metaModel).Returns(resultModel);

            var expectedResult = GetSerializationData(resultModel);

            // Action
            var result = ToJson(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 1)]
        public void Index_IsExportExcelTrue_CallExcelStreamWithCorrectData(int drillDownLevel, int currencyId)
        {
            // Arrange
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = currencyId,
                DrillDownLevel = drillDownLevel,
                DrillDownCurrencyId = -2,
                DrillDownCustomerId = 1,
                IsExportExcel = true,
                DrillDownFromDate = fromDate.AddDays(1),
                DrillDownToDate = toDate.AddDays(1),
                FromDate = fromDate,
                ToDate = toDate,
            };

            var excel = Substitute.ForPartsOf<AgCasinoBalanceExcel>(string.Empty, new ConvertingOption(), metaModel, new AgCasinoBalanceViewModel());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<AgCasinoBalanceMetaModel>(),
                Arg.Any<AgCasinoBalanceViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            var expectedFromDate = drillDownLevel > 0 ? metaModel.DrillDownFromDate : metaModel.FromDate;
            var expectedToDate = drillDownLevel > 0 ? metaModel.DrillDownToDate : metaModel.ToDate;
            var expectedCurrencyId = drillDownLevel > 0 && currencyId > 0 ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            MockDataAccess(_dataAccess, metaModel);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>());
                Assert.That(metaModel.FromDate, Is.EqualTo(expectedFromDate));
                Assert.That(metaModel.ToDate, Is.EqualTo(expectedToDate));
                Assert.That(metaModel.CurrencyId, Is.EqualTo(expectedCurrencyId));
            });
        }

        private static void MockDataAccess(IAgCasinoBalanceDataAccess dataAccess, AgCasinoBalanceMetaModel metaModel)
        {
            var resultModel = new SortableSummaryReportResultModel<AgCasinoBalance>
            {
                TotalData = new List<AgCasinoBalance> { new AgCasinoBalance() },
                DataList = new SortableList<AgCasinoBalance> { new AgCasinoBalance() },
                SummaryTotal = new Dictionary<string, AgCasinoBalance>()
            };
            dataAccess.GetSummaryBalance(metaModel).Returns(resultModel);
            dataAccess.GetAgentBalanceDetail(metaModel).Returns(resultModel);
            dataAccess.GetCustomerBalanceDetail(metaModel).Returns(resultModel);
        }

        private static string ToJson(object value)
        {
            return new JavaScriptSerializer().Serialize(value);
        }

        private static string GetSerializationData(SortableSummaryReportResultModel<AgCasinoBalance> data = null)
        {
            if (data == null)
            {
                return ToJson(
                new
                {
                    TotalRows = 0,
                    Data = new List<AgCasinoBalance>(),
                    Total = new List<AgCasinoBalance>(),
                    SummaryTotal = new Dictionary<string, AgCasinoBalance>()
                });
            }

            return ToJson(
                new
                {
                    TotalRows = data.TotalRows,
                    Data = data.DataList,
                    Total = data.TotalData,
                    SummaryTotal = data.SummaryTotal
                });
        }
    }
}