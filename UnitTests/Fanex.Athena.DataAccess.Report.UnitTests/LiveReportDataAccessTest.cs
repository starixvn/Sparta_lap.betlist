namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NSubstitute.Core;
    using NUnit.Framework;

    [TestFixture]
    public class LiveReportDataAccessTest
    {
        private LiveReportDataAccess _dataAccess;
        private LiveReportMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.ForPartsOf<LiveReportDataAccess>();

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
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
        }

        [Test]
        public void GetLiveReportSummary_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportSummary")).Returns(objectDb);
            var moveBetsDate = new DateTime(2017, 2, 2);

            // Action
            _dataAccess.GetLiveReportSummary(_metaModel, moveBetsDate);

            // Assert
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate.Date);
            objectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate.Date);
            objectDb.Received().AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
        }

        [Test]
        public void GetLiveReportSummary_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportSummary")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "ProductName", "Sportsbook" }, { "ProductCode", "SB" }, { "TurnoverUUS", 100 }, { "WinlostUUS", 200 },
                { "PlayerCommUUS", 300 }, { "CommUUS", 400 }, { "LicenseeWLUUS", 500 }, { "AmtDueto3rdUUS", 600 }, { "Betcount", 700 },
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetLiveReportSummary(_metaModel, Arg.Any<DateTime>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].ReportType, Is.EqualTo(1));
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
        public void GetDetailBySport_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportBySport")).Returns(objectDb);
            var moveBetsDate = new DateTime(2017, 2, 2);

            // Action
            _dataAccess.GetDetailBySport(_metaModel, moveBetsDate, "SB");

            // Assert
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate.Date);
            objectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate.Date);
            objectDb.Received().AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
            objectDb.Received().AddInParameter("productcode", DbType.String, "SB");
        }

        [Test]
        public void GetDetailBySport_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportBySport")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "SportType", "1" }, { "ProductCode", "SB" }, { "TurnoverUUS", 100 }, { "WinlostUUS", 200 }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetDetailBySport(_metaModel, Arg.Any<DateTime>(), string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].ProductCode, Is.EqualTo("1"));
                Assert.That(result[0].TurnOver, Is.EqualTo(100));
                Assert.That(result[0].CustomerWinLoss, Is.EqualTo(200));
            });
        }

        [Test]
        public void GetDetailByCurrencyAndBettype_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportSummaryDetail")).Returns(objectDb);
            var moveBetsDate = new DateTime(2017, 2, 2);
            var productList = new List<Product>
            {
                new Product { ProductCode = "SB" }
            };

            // Action
            _dataAccess.GetDetailByCurrencyAndBettype(_metaModel, moveBetsDate, "SB", productList);

            // Assert
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate.Date);
            objectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate.Date);
            objectDb.Received().AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
            objectDb.Received().AddInParameter("productcode", DbType.String, "SB");
        }

        [Test]
        public void GetDetailByCurrencyAndBettype_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { ProductCode = "SB", ProductName = "SuperBoy" }
            };
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportSummaryDetail")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);

            var readerDataMappingForCurrency = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Currency", "1" }, { "TurnoverUUS", 100 }, { "WinlostUUS", 200 }
            };

            var readerDataMappingForBettype = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "bettype", "1" }, { "TurnoverUUS", 100 }, { "WinlostUUS", 200 }
            };

            MapReaderData(reader, readerDataMappingForCurrency, readerDataMappingForBettype);

            // Action
            var result = _dataAccess.GetDetailByCurrencyAndBettype(_metaModel, Arg.Any<DateTime>(), string.Empty, productList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1.Count, Is.EqualTo(1));
                Assert.That(result.Item1[0].ReportType, Is.EqualTo(0));
                Assert.That(result.Item1[0].TurnOver, Is.EqualTo(100));
                Assert.That(result.Item1[0].CustomerWinLoss, Is.EqualTo(200));

                Assert.That(result.Item2.Count, Is.EqualTo(1));
                Assert.That(result.Item2[0].LiveReports.Count, Is.EqualTo(1));
                Assert.That(result.Item2[0].ProductName, Is.EqualTo("SuperBoy"));
            });
        }

        [Test]
        public void GetLiveReportWinlossByMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportWinlossByMatch")).Returns(objectDb);
            var moveBetsDate = new DateTime(2017, 2, 2);
            var metaModel = new LiveReportMatchListMetalModel
            {
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "NG", Status = true }
                },
                AgentListXml = "3",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                LiveIndicator = 1,
                SportId = 3
            };

            // Action
            _dataAccess.GetLiveReportWinlossByMatch(metaModel, moveBetsDate);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            objectDb.Received().AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            objectDb.Received().AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            objectDb.Received().AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
            objectDb.Received().AddInParameter("sportid", DbType.Int32, metaModel.SportId);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
        }

        [Test]
        public void GetLiveReportWinlossByMatch_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveReportWinlossByMatch")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "matchid", 2 }, { "homeid", 3 }, { "awayid", 4 }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetLiveReportWinlossByMatch(new LiveReportMatchListMetalModel(), Arg.Any<DateTime>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].MatchId, Is.EqualTo(2));
                Assert.That(result[0].HomeId, Is.EqualTo(3));
                Assert.That(result[0].AwayId, Is.EqualTo(4));
            });
        }

        private void MapReaderData(
            IDataReader reader,
            IDictionary<string, object> readerDataMapping1,
            IDictionary<string, object> readerDataMapping2 = null)
        {
            var firstFunc = new Func<CallInfo, object>(x =>
            {
                var paramname = x.ArgAt<string>(0);
                if (readerDataMapping1.ContainsKey(paramname))
                {
                    return readerDataMapping1[paramname];
                }

                return string.Empty;
            });

            if (readerDataMapping2 == null)
            {
                reader[Arg.Any<string>()].Returns(firstFunc);
            }
            else
            {
                reader[Arg.Any<string>()].Returns(
                    firstFunc,
                    x =>
                    {
                        var paramname = x.ArgAt<string>(0);
                        if (readerDataMapping2.ContainsKey(paramname))
                        {
                            return readerDataMapping2[paramname];
                        }

                        return string.Empty;
                    });
            }
        }
    }
}