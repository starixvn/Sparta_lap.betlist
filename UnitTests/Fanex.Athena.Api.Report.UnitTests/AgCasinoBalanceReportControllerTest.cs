namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Extension.Utility;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class AgCasinoBalanceReportControllerTest
    {
        private IAgCasinoBalanceService _service;
        private AgCasinoBalanceReportController _controller;
        private ISessionService _sessionService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _service = Substitute.For<IAgCasinoBalanceService>();
            _sessionService = Substitute.For<ISessionService>();
            var productService = Substitute.For<IProductService>();

            _controller = Substitute.ForPartsOf<AgCasinoBalanceReportController>(_service, _sessionService, productService);
            MockSessionData(_sessionService);
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