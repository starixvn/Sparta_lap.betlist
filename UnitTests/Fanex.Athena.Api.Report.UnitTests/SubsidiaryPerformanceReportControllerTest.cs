namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [TestFixture]
    public class SubsidiaryPerformanceReportControllerTest
    {
        private ISessionService _sessionService;

        private IProductService _productService;

        private ISubsidiaryPerformanceService _service;

        private SubsidiaryPerformanceReportController _controller;

        private JavaScriptSerializer _serializer;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<ISubsidiaryPerformanceService>();
            _controller = Substitute.ForPartsOf<SubsidiaryPerformanceReportController>(_sessionService, _productService, _service);
            _serializer = new JavaScriptSerializer();
            CreateSession();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = null;
            _controller = null;
            _sessionService = null;
        }

        [Test]
        public void Index_WhenCall_CallServiceWithCorrectMetaModel()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></Root>";
            _sessionService.GenerateAgentListXml().Returns(AgentListXml);
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                LicenseeId = 1,
                ProductCodeList = "1",
                SubsidiaryList = "1,2,3",
                AgentListXml = AgentListXml
            };
            SubsidiaryPerformanceMetaModel passedMetaModel = null;
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            _service.GetData(Arg.Do<SubsidiaryPerformanceMetaModel>(x => passedMetaModel = x), out totalList);

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.ReceivedWithAnyArgs().GetData(passedMetaModel, out totalList);
            Assert.That(metaModel, Is.EqualTo(passedMetaModel));
        }

        [Test]
        public void Index_TotalDataListIsNotNull_ReturnCorrectData()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></Root>";
            _sessionService.GenerateAgentListXml().Returns(AgentListXml);
            var metaModel = new SubsidiaryPerformanceMetaModel { BaseCurrency = "USD" };
            IEnumerable<SubsidiaryPerformanceEntity> totalList;
            var expectedSummaryData = GetFakeSummaryData();
            var expectedGrandTotal = GetExpectedGrandTotal();
            IEnumerable<SubsidiaryPerformanceEntity> expectedTotalDataList = GetFakeTotalDataList();

            _service.GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalList).Returns(x =>
            {
                x[1] = expectedTotalDataList;
                return expectedSummaryData;
            });

            var expectedResult = GetSerializationData(expectedSummaryData, expectedTotalDataList, expectedGrandTotal);

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_TotalDataListNotNull_TotalAndGrandTotalIsNull()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></Root>";
            _sessionService.GenerateAgentListXml().Returns(AgentListXml);
            var metaModel = new SubsidiaryPerformanceMetaModel();
            IEnumerable<SubsidiaryPerformanceEntity> totalList;
            var expectedSummaryData = GetFakeSummaryData();
            IEnumerable<SubsidiaryPerformanceEntity> expectedTotalDataList = null;

            _service.GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalList).Returns(x =>
            {
                x[1] = null;
                return expectedSummaryData;
            });

            var expectedResult = GetSerializationData(expectedSummaryData, expectedTotalDataList);

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private string GetSerializationData(
            IEnumerable<SubsidiaryPerformanceDataModel> summaryData,
            IEnumerable<SubsidiaryPerformanceEntity> totalList,
            SubsidiaryPerformanceEntity grandTotal = null)
        {
            return _serializer.Serialize(
                new
                {
                    Data = summaryData,
                    Total = totalList,
                    GrandTotal = grandTotal
                });
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

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetFakeSummaryData()
        {
            var detailData = new List<SubsidiaryPerformanceDetail>
            {
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m}
            };

            return new List<SubsidiaryPerformanceDataModel>
            {
                new SubsidiaryPerformanceDataModel { Subsidiary = "New York", Currency = "USD", Details = detailData},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "THB", Details = detailData},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "THB", Details = detailData},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "THB", Details = detailData}
            };
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetFakeTotalDataList()
        {
            return new List<SubsidiaryPerformanceEntity>
            {
                new SubsidiaryPerformanceEntity
                { Subsidiary = "New York", Currency = "USD", Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { Subsidiary = "India", Currency = "USD", Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m}
            };
        }

        private static SubsidiaryPerformanceEntity GetExpectedGrandTotal()
        {
            return new SubsidiaryPerformanceEntity
            {
                Currency = "USD",
                BetCount = 100,
                Turnover = 200000m,
                CustomerWinLoss = 20000m,
                SubsidiaryWinLoss = -20000m
            };
        }
    }
}