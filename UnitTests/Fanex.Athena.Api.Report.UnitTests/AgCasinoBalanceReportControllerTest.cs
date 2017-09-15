namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class AgCasinoBalanceReportControllerTest
    {
        private IAgCasinoBalanceService _service;
        private AgCasinoBalanceReportController _controller;

        [OneTimeSetUp]
        public void SetUp()
        {
            _service = Substitute.For<IAgCasinoBalanceService>();
            var sessionService = Substitute.For<ISessionService>();
            var productService = Substitute.For<IProductService>();

            _controller = Substitute.ForPartsOf<AgCasinoBalanceReportController>(_service, sessionService, productService);
            MockSessionData(sessionService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
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

        [Test]
        public void Index_Level0_CallGetSummaryBalanceInService_ReturnCorrectData()
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
            var dataListResult = new SortableList<AgCasinoBalance> { new AgCasinoBalance() };
            _service.GetSummaryBalance(metaModel, out summaryData).Returns(x =>
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

        [Test]
        public void Index_IsExportExcelTrue_CallExcelStream()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            DateTime fromDate = new DateTime(2017, 4, 1), toDate = new DateTime(2017, 4, 18);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = -1,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
                IsExportExcel = true,
                DrillDownFromDate = fromDate,
                DrillDownToDate = toDate,
                FromDate = fromDate
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

            // Action
            _controller.Index(metaModel);

            // Assert
            _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>());
        }

        private static string ToJson(object value)
        {
            return new JavaScriptSerializer().Serialize(value);
        }

        private static void MockSessionData(ISessionService sessionService)
        {
            var userSession = new UserSession
            {
                CurrencyList = new Dictionary<int, string>
            {
                { -1, "EUR" }, { -2, "USD" }
            }
            };

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = 1
            };

            sessionService.UserSession.Returns(userSession);
            sessionService.LicenseeSession.Returns(licenseeSession);
        }

        private static string GetSerializationData(
            int totalRow,
            IEnumerable<AgCasinoBalance> data,
            IEnumerable<AgCasinoBalance> total,
            Dictionary<string, AgCasinoBalance> summaryTotal)
        {
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