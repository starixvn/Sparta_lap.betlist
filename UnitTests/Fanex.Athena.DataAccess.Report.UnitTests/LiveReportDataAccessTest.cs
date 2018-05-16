namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Constant.DataDomain;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Interfaces;
    using Session.Model;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class LiveReportDataAccessTest : BaseDataAccessHelperTest<LiveReportDataAccess>
    {
        private const string GetLiveReportSummarySp = "Lrf_Rpt_LiveReportSummary";

        private const string GetDetailBySportSp = "Lrf_Rpt_LiveReportBySport";

        private const string GetDetailByCurrencyAndBettypeSp = "Lrf_Rpt_LiveReportSummaryDetail";

        private const string GetLiveReportWinlossByMatch = "Lrf_Rpt_LiveReportWinlossByMatch";

        private LiveReportMetaModel _metaModel;

        private BalanceUp _balance;

        private ICacheRefAdapter _cacheRefAdapter;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 2, 2)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            ObjectDb = Substitute.For<IObjectDb>();

            var sessionStorage = Substitute.For<ISessionStorage>();
            sessionStorage.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>()
            });
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();

            DataAccess = Substitute.ForPartsOf<LiveReportDataAccess>(
                Substitute.For<IAppSettings>(),
                sessionStorage,
                balanceUpDataAccess,
                _cacheRefAdapter);

            _metaModel = new LiveReportMetaModel
            {
                BaseCurrency = "UUS",
                LicenseeId = 2,
                AgentListXml = "3",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                LiveIndicator = 1
            };
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _balance = null;
            _metaModel = null;

            base.FixtureTearDown();
        }

        [Test]
        public void GetLiveReportSummary_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLiveReportSummarySp)).Returns(ObjectDb);
            _metaModel.ProductCodeList = "SB,NG";

            // Action
            DataAccess.GetLiveReportSummary(_metaModel);

            // Assert
            AssertCommonAddInParameters();
        }

        [Test]
        public void GetLiveReportSummary_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLiveReportSummarySp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "ProductName", "Sportsbook" }, { "ProductCode", "SB" }, { "TurnoverUUS", 100 }, { "WinlostUUS", 200 },
                { "PlayerCommUUS", 300 }, { "CommUUS", 400 }, { "LicenseeWLUUS", 500 }, { "AmtDueto3rdUUS", 600 }, { "Betcount", 700 },
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetLiveReportSummary(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].ProductName, Is.EqualTo("Sportsbook"));
                Assert.That(result[0].ProductCode, Is.EqualTo("SB"));
                Assert.That(result[0].TurnOver, Is.EqualTo(100));
                Assert.That(result[0].CustomerWinLoss, Is.EqualTo(200));
                Assert.That(result[0].CustomerComm, Is.EqualTo(300));
                Assert.That(result[0].LicenseeComm, Is.EqualTo(400));
                Assert.That(result[0].LicenseeWinLoss, Is.EqualTo(500));
                Assert.That(result[0].AmountDue, Is.EqualTo(600));
                Assert.That(result[0].BetCount, Is.EqualTo(700));
            });
        }

        [Test]
        public void GetLiveReportSummary_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetLiveReportSummarySp,
                () => DataAccess.GetLiveReportSummary(_metaModel));
        }

        [Test]
        public void GetDetailBySport_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailBySportSp)).Returns(ObjectDb);
            _metaModel.ProductCodeList = "SB,NG";

            // Action
            DataAccess.GetDetailBySport(_metaModel);

            // Assert
            AssertCommonAddInParameters();
        }

        [Test]
        public void GetDetailBySport_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailBySportSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "SportType", "1" }, { "ProductCode", "SB" }, { "TurnoverUUS", 100 }, { "WinlostUUS", 200 }
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetDetailBySport(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].ProductCode, Is.EqualTo("1"));
                Assert.That(result[0].TurnOver, Is.EqualTo(100));
                Assert.That(result[0].CustomerWinLoss, Is.EqualTo(200));
            });
        }

        [TestCase("101", BetType.MixParlay)]
        [TestCase("102", BetType.CombinationMixParlay)]
        [TestCase("103", BetType.Outright)]
        public void GetProductNameBySport_WhenCall_CallCorrectCacheRefMethod(string sportType, BetType bettype)
        {
            // Action
            DataAccess.GetProductNameBySport(sportType);

            // Assert
            _cacheRefAdapter.Received(1).GetBettype((int)bettype);
        }

        [Test]
        public void GetProductNameBySport_VirtualSports2MixParlay_CallCorrectCacheRefMethod()
        {
            // Arrange
            const string Bettype = "104";

            // Action
            DataAccess.GetProductNameBySport(Bettype);

            // Assert
            _cacheRefAdapter.Received(1).GetBettypeWithProductName((int)BetType.VirtualSports2MixParlay);
        }

        [Test]
        public void GetDetailBySport_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetDetailBySportSp,
                () => DataAccess.GetDetailBySport(_metaModel));
        }

        [Test]
        public void GetDetailByCurrencyAndBettype_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailByCurrencyAndBettypeSp)).Returns(ObjectDb);
            _metaModel.ProductCodeList = "SB,NG";

            // Action
            DataAccess.GetDetailByCurrencyAndBettype(_metaModel, new List<Product>());

            // Assert
            AssertCommonAddInParameters();
        }

        [Test]
        public void GetDetailByCurrencyAndBettype_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { ProductCode = "SB", ProductName = "SuperBoy" },
            };

            DataAccess.GetObjectDb(Arg.Is(GetDetailByCurrencyAndBettypeSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Currency", "1" }, { "TurnoverUUS", 100 },
                { "WinlostUUS", 200 }, { "bettype", "1" }, { "ProductCode", "SB" }
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetDetailByCurrencyAndBettype(_metaModel, productList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1.Count, Is.EqualTo(1));
                Assert.That(result.Item1[0].TurnOver, Is.EqualTo(100));
                Assert.That(result.Item1[0].CustomerWinLoss, Is.EqualTo(200));

                Assert.That(result.Item2.Count, Is.EqualTo(1));
                Assert.That(result.Item2[0].LiveReports.Count, Is.EqualTo(1));
                Assert.That(result.Item2[0].ProductName, Is.EqualTo("SuperBoy"));
            });
        }

        [TestCase("2803")]
        [TestCase("2804")]
        [TestCase("2805")]
        [TestCase("2806")]
        public void GetDetailByCurrencyAndBettype_BettypeOfVirtualBasketball_ReturnCorrectBetTypeName(string betType)
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { ProductCode = "VS2", ProductName = "VS2" },
            };

            _cacheRefAdapter.GetBettype(Arg.Any<int>(), false).Returns(betType);
            _cacheRefAdapter.GetSportName(Arg.Any<int>()).Returns("Virtual Basketball");

            DataAccess.GetObjectDb(Arg.Is(GetDetailByCurrencyAndBettypeSp)).Returns(ObjectDb);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Currency", "20" }, { "TurnoverUUS", 200 },
                { "WinlostUUS", 400 }, { "bettype", betType }, { "ProductCode", "VS2" }
            };

            MapReaderData(readerDataMapping);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            // Action
            var result = DataAccess.GetDetailByCurrencyAndBettype(_metaModel, productList);

            // Assert
            Assert.That(result.Item2[0].LiveReports[0].ProductName, Is.EqualTo($"{betType} (Virtual Basketball)"));
        }

        [Test]
        public void GetDetailByCurrencyAndBettype_NoData_ReturnEmptyResult()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { ProductCode = "SB", ProductName = "SuperBoy" }
            };

            DataAccess.GetObjectDb(Arg.Is(GetDetailByCurrencyAndBettypeSp)).Returns(ObjectDb);

            Reader.Read().Returns(false);
            Reader.NextResult().Returns(true);

            // Action
            var result = DataAccess.GetDetailByCurrencyAndBettype(_metaModel, productList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.Empty);
                Assert.That(result.Item2, Is.Empty);
            });
        }

        [Test]
        public void GetDetailByCurrencyAndBettype_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetDetailByCurrencyAndBettypeSp,
                () => DataAccess.GetDetailByCurrencyAndBettype(_metaModel, null));
        }

        [Test]
        public void GetLiveReportWinlossByMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLiveReportWinlossByMatch)).Returns(ObjectDb);
            var metaModel = new LiveReportMatchListMetalModel
            {
                ProductCodeList = "NG",
                AgentListXml = "3",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                LiveIndicator = 1,
                SportId = 3
            };

            // Action
            DataAccess.GetLiveReportWinlossByMatch(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);
                ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("sportid", DbType.Int32, metaModel.SportId);
                ObjectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void GetLiveReportWinlossByMatch_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLiveReportWinlossByMatch)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new
            {
                matchid = 2,
                homeid = 3,
                awayid = 4
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetLiveReportWinlossByMatch(new LiveReportMatchListMetalModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].MatchId, Is.EqualTo(2));
                Assert.That(result[0].HomeId, Is.EqualTo(3));
                Assert.That(result[0].AwayId, Is.EqualTo(4));
            });
        }

        [Test]
        public void GetLiveReportWinlossByMatch_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetLiveReportWinlossByMatch,
                () => DataAccess.GetLiveReportWinlossByMatch(new LiveReportMatchListMetalModel()));
        }

        private void AssertCommonAddInParameters()
        {
            ObjectDb.Received(1).AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            ObjectDb.Received(1).AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate.Date);
            ObjectDb.Received(1).AddInParameter("todate", DbType.DateTime, _metaModel.ToDate.Date);
            ObjectDb.Received(1).AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator);
            ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            ObjectDb.Received(1).AddInParameter("ProductCodes", DbType.String, _metaModel.ProductCodeList);
        }
    }
}