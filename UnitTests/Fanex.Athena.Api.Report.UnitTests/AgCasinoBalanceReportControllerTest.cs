namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class AgCasinoBalanceReportControllerTest : BaseApiReportControllerHelperTest
    {
        private IAgCasinoBalanceService _service;
        private AgCasinoBalanceReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();

            _service = Substitute.For<IAgCasinoBalanceService>();

            _controller = Substitute.ForPartsOf<AgCasinoBalanceReportController>(_service, SessionService, ProductService);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _service = null;
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

            const int TotalRow = 0;
            var data = new List<AgCasinoBalance>();
            var total = new List<AgCasinoBalance>();
            var summaryTotal = new Dictionary<string, AgCasinoBalance>();
            var expectedResult = GetSerializationData(TotalRow, data, total, summaryTotal);

            // Action
            var result = ToJson(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(true)]
        [TestCase(false)]
        [TestCase(null)]
        public void Index_Level0_CallGetSummaryBalanceInService_ReturnCorrectData(bool? emptyDataList)
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

            IDictionary<string, AgCasinoBalance> summaryData;
            var expectedSummaryData = new Dictionary<string, AgCasinoBalance>();
            var dataListResult = !emptyDataList.HasValue 
                ? null 
                : emptyDataList.Value ? new SortableList<AgCasinoBalance>() : new SortableList<AgCasinoBalance> { new AgCasinoBalance() };
            _service.GetSummaryBalance(metaModel, out summaryData).Returns(x =>
            {
                x[1] = expectedSummaryData;
                return dataListResult;
            });

            var totalData = new List<AgCasinoBalance> { new AgCasinoBalance() };
            _service.TotalCalc(dataListResult).Returns(totalData);

            const int TotalRow = 0;
            var expectedResult = GetSerializationData(TotalRow, dataListResult, totalData, expectedSummaryData, emptyDataList ?? true);

            // Action
            var result = ToJson(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
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

            IDictionary<string, AgCasinoBalance> summaryData;
            var expectedSummaryData = new Dictionary<string, AgCasinoBalance>();
            var dataListResult = new SortableList<AgCasinoBalance> { new AgCasinoBalance() };
            _service.GetAgentBalanceDetail(metaModel, out summaryData).Returns(x =>
            {
                x[1] = expectedSummaryData;
                return dataListResult;
            });

            var totalData = new List<AgCasinoBalance> { new AgCasinoBalance() };
            _service.TotalCalc(dataListResult).Returns(totalData);

            const int TotalRow = 0;
            var expectedResult = GetSerializationData(TotalRow, dataListResult, totalData, expectedSummaryData);

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

            IDictionary<string, AgCasinoBalance> summaryData;
            var expectedSummaryData = new Dictionary<string, AgCasinoBalance>();
            var dataListResult = new SortableList<AgCasinoBalance> { new AgCasinoBalance() };
            var totalRow = 0;
            const int ExpectedTotalRow = 1;
            _service.GetCustomerBalanceDetail(metaModel, out summaryData, out totalRow).Returns(x =>
            {
                x[1] = expectedSummaryData;
                x[2] = ExpectedTotalRow;
                return dataListResult;
            });

            var totalData = new List<AgCasinoBalance> { new AgCasinoBalance() };
            _service.TotalCalc(dataListResult).Returns(totalData);

            var expectedResult = GetSerializationData(ExpectedTotalRow, dataListResult, totalData, expectedSummaryData);

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
            const int CurrencyId = -2, CustId = 1;
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = currencyId,
                IsExcludedTestData = true,
                DrillDownLevel = drillDownLevel,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
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

        private static string ToJson(object value)
        {
            return new JavaScriptSerializer().Serialize(value);
        }

        private static string GetSerializationData(
            int totalRow,
            IEnumerable<AgCasinoBalance> data,
            IEnumerable<AgCasinoBalance> total,
            Dictionary<string, AgCasinoBalance> summaryTotal,
            bool isEmptyData = false)
        {
            if (isEmptyData)
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
                    TotalRows = totalRow,
                    Data = data,
                    Total = total,
                    SummaryTotal = summaryTotal
                });
        }
    }
}