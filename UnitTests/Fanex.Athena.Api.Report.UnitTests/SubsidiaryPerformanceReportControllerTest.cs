namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        private ISubsidiaryPerformanceService _service;

        private SubsidiaryPerformanceReportController _controller;

        private JavaScriptSerializer _serializer;

        [OneTimeSetUp]
        public void SetUp()
        {
            var productService = Substitute.For<IProductService>();
            _sessionService = Substitute.For<ISessionService>();
            _service = Substitute.For<ISubsidiaryPerformanceService>();
            _controller = Substitute.ForPartsOf<SubsidiaryPerformanceReportController>(_sessionService, productService, _service);
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
        public void Index_WhenCall_ServiceIsCalledWithCorrectMetaModel()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></Root>";
            var metaModel = GetMetaModel();
            metaModel.AgentListXml = AgentListXml;

            _sessionService.GenerateAgentListXmlByAgentIdList(Arg.Any<IEnumerable<int>>()).Returns(AgentListXml);
            SubsidiaryPerformanceMetaModel passedMetaModel = null;
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            _service.GetData(Arg.Do<SubsidiaryPerformanceMetaModel>(x => passedMetaModel = x), out totalList)
                .Returns(x =>
                {
                    x[1] = new List<SubsidiaryPerformanceEntity>();
                    return null;
                });

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.That(metaModel, Is.EqualTo(passedMetaModel));
        }

        [Test]
        public void Index_SubsidiaryListHasValue_GenerateAgentListXmlByAgentIdListIsCalledWithCorrectParameter()
        {
            // Arrange
            var metaModel = GetMetaModel();
            IEnumerable<int> agentIdList = null;
            _sessionService.GenerateAgentListXmlByAgentIdList(Arg.Do<IEnumerable<int>>(x => agentIdList = x));
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            _service.GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalList)
                .Returns(x =>
                {
                    x[1] = new List<SubsidiaryPerformanceEntity>();
                    return null;
                });

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.That(agentIdList.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Index_TotalDataListIsNotEmpty_ReturnCorrectGrandTotalData()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></Root>";
            var metaModel = GetMetaModel();
            _sessionService.GenerateAgentListXmlByAgentIdList(Arg.Any<IEnumerable<int>>()).Returns(AgentListXml);
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
        public void Index_TotalDataListIsEmpty_GrandTotalIsNull()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></Root>";
            var metaModel = GetMetaModel();
            _sessionService.GenerateAgentListXmlByAgentIdList(Arg.Any<IEnumerable<int>>()).Returns(AgentListXml);
            IEnumerable<SubsidiaryPerformanceEntity> totalList;
            var expectedSummaryData = GetFakeSummaryData();
            IEnumerable<SubsidiaryPerformanceEntity> expectedTotalDataList = new List<SubsidiaryPerformanceEntity>();

            _service.GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalList).Returns(x =>
            {
                x[1] = new List<SubsidiaryPerformanceEntity>();
                return expectedSummaryData;
            });

            var expectedResult = GetSerializationData(expectedSummaryData, expectedTotalDataList);

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_FromDateIsGreaterThanToDate_DoNotCallService()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 1),
                ProductCodeList = "SB,BA",
                BaseCurrency = "EUR"
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.DidNotReceiveWithAnyArgs().GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalDataList);
        }

        [Test]
        public void Index_ProductCodeListIsEmpty_DoNotCallService()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "",
                BaseCurrency = "EUR"
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.DidNotReceiveWithAnyArgs().GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalDataList);
        }

        [Test]
        public void Index_BaseCurrencyIsEmpty_DoNotCallService()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                BaseCurrency = ""
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.DidNotReceiveWithAnyArgs().GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalDataList);
        }

        [Test]
        public void Index_MetaModelIsNotValid_ReturnEmptyList()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel();
            var expectedResult = _serializer.Serialize(new { Data = new List<SubsidiaryPerformanceDataModel>() });

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
                { SubsidiaryId = 1234, Subsidiary = "1234", CurrencyName = "EUR", BetCount = 150, Turnover = 300000m, CustomerWinLoss = 30000m, SubsidiaryWinLoss = -30000m},
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1235, Subsidiary = "1235", CurrencyName = "EUR", BetCount = 100, Turnover = 200000m, CustomerWinLoss = 20000m, SubsidiaryWinLoss = -20000m}
            };
        }

        private static SubsidiaryPerformanceEntity GetExpectedGrandTotal()
        {
            return new SubsidiaryPerformanceEntity
            {
                CurrencyName = "USD",
                BetCount = 250,
                Turnover = 500000m,
                CustomerWinLoss = 50000m,
                SubsidiaryWinLoss = -50000m
            };
        }

        private SubsidiaryPerformanceMetaModel GetMetaModel()
        {
            return new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "123,143",
                BaseCurrency = "USD"
            };
        }
    }
}