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
        private AgCasinoBalanceMetaModel _metaModel = new AgCasinoBalanceMetaModel { IsExportExcel = false };
        private ISessionService _sessionService;
        private JavaScriptSerializer _serializer;

        [OneTimeSetUp]
        public void SetUp()
        {
            _service = Substitute.For<IAgCasinoBalanceService>();
            _sessionService = Substitute.For<ISessionService>();
            var productService = Substitute.For<IProductService>();
            _controller = Substitute.ForPartsOf<AgCasinoBalanceReportController>(_service, _sessionService, productService);
            _serializer = new JavaScriptSerializer();
            CreateSession();
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
            _metaModel.CurrencyId = 1;
            _metaModel.DrillDownCurrencyId = 1;

            const int TotalRow = 0;
            var data = new List<AgCasinoBalance>();
            var total = new List<AgCasinoBalance>();
            var summaryTotal = new Dictionary<string, AgCasinoBalance>();
            var expectedResult = GetSerializationData(TotalRow, data, total, summaryTotal);

            // Action
            var result = _serializer.Serialize(_controller.Index(_metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_Level0_CallGetSummaryBalanceInService_ReturnCorrectData()
        {
            // Arrange
            const int CurrencyId = -2;
            const bool IsExcludedTestData = true;

            _metaModel.CurrencyId = CurrencyId;
            _metaModel.DrillDownLevel = 0;
            _metaModel.DrillDownCurrencyId = 0;
            _metaModel.IsExcludedTestData = IsExcludedTestData;

            _sessionService.GenerateAgentListXml(CurrencyId, IsExcludedTestData).Returns("XmlData");

            IDictionary<string, AgCasinoBalance> summaryData;
            var expectedSummaryData = new Dictionary<string, AgCasinoBalance>();
            var dataListResult = new SortableList<AgCasinoBalance> { new AgCasinoBalance() };
            _service.GetSummaryBalance(_metaModel, out summaryData).Returns(x =>
            {
                x[1] = expectedSummaryData;
                return dataListResult;
            });

            var totalData = new List<AgCasinoBalance> { new AgCasinoBalance() };
            _service.TotalCalc(dataListResult).Returns(totalData);

            const int TotalRow = 0;
            var expectedResult = GetSerializationData(TotalRow, dataListResult, totalData, expectedSummaryData);

            // Action
            var result = _serializer.Serialize(_controller.Index(_metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_Level1_CallGetAgentBalanceDetailInService_ReturnCorrectData()
        {
            // Arrange
            const int CurrencyId = -2;
            const bool IsExcludedTestData = true;

            _metaModel.CurrencyId = CurrencyId;
            _metaModel.DrillDownLevel = 1;
            _metaModel.DrillDownCurrencyId = 1;
            _metaModel.IsExcludedTestData = IsExcludedTestData;

            _sessionService.GenerateAgentListXml(_metaModel.DrillDownCurrencyId, IsExcludedTestData).Returns("XmlData");

            IDictionary<string, AgCasinoBalance> summaryData;
            var expectedSummaryData = new Dictionary<string, AgCasinoBalance>();
            var dataListResult = new SortableList<AgCasinoBalance> { new AgCasinoBalance() };
            _service.GetAgentBalanceDetail(_metaModel, out summaryData).Returns(x =>
            {
                x[1] = expectedSummaryData;
                return dataListResult;
            });

            var totalData = new List<AgCasinoBalance> { new AgCasinoBalance() };
            _service.TotalCalc(dataListResult).Returns(totalData);

            const int TotalRow = 0;
            var expectedResult = GetSerializationData(TotalRow, dataListResult, totalData, expectedSummaryData);

            // Action
            var result = _serializer.Serialize(_controller.Index(_metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_Level2_CallGetCustomerBalanceDetailInService_ReturnCorrectData()
        {
            // Arrange
            const int CurrencyId = -2;
            const bool IsExcludedTestData = true;

            _metaModel.CurrencyId = CurrencyId;
            _metaModel.DrillDownLevel = 2;
            _metaModel.DrillDownCurrencyId = 1;
            _metaModel.IsExcludedTestData = IsExcludedTestData;

            _sessionService.GenerateAgentListXml(_metaModel.DrillDownCurrencyId, IsExcludedTestData).Returns("XmlData");

            IDictionary<string, AgCasinoBalance> summaryData;
            var expectedSummaryData = new Dictionary<string, AgCasinoBalance>();
            var dataListResult = new SortableList<AgCasinoBalance> { new AgCasinoBalance() };
            var totalRow = 0;
            const int ExpectedTotalRow = 1;
            _service.GetCustomerBalanceDetail(_metaModel, out summaryData, out totalRow).Returns(x =>
            {
                x[1] = expectedSummaryData;
                x[2] = ExpectedTotalRow;
                return dataListResult;
            });

            var totalData = new List<AgCasinoBalance> { new AgCasinoBalance() };
            _service.TotalCalc(dataListResult).Returns(totalData);

            var expectedResult = GetSerializationData(ExpectedTotalRow, dataListResult, totalData, expectedSummaryData);

            // Action
            var result = _serializer.Serialize(_controller.Index(_metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private void CreateSession()
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

            _sessionService.UserSession.Returns(userSession);
            _sessionService.LicenseeSession.Returns(licenseeSession);
        }

        private string GetSerializationData(
            int totalRow,
            IEnumerable<AgCasinoBalance> data,
            IEnumerable<AgCasinoBalance> total,
            Dictionary<string, AgCasinoBalance> summaryTotal)
        {
            return _serializer.Serialize(
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