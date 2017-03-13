namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
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

            var appSetting = Substitute.For<IAppSettings>();
            appSetting.HistoryDate.Returns(new DateTime(2016, 8, 31));
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

            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });

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
    }
}