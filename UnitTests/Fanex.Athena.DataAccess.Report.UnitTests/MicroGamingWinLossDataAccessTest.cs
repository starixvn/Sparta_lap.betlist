namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MicroGamingWinLossDataAccessTest
    {
        private MicroGamingWinLossDataAccess _dataAccess;
        private BalanceUp _balance;
        private MicroGamingWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };

            _dataAccess = Substitute.ForPartsOf<MicroGamingWinLossDataAccess>(_balance);

            _metaModel = new MicroGamingWinLossMetaModel
            {
                LicenseeId = 1,
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                ReportBy = 2
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_MicroGamingWinloss")).Returns(objectDb);
            IDictionary<string, MicroGamingWinLoss> summaryTotal;

            // Action
            _dataAccess.GetSummaryWinloss(_metaModel, out summaryTotal);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.FromDate);
            objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.ToDate);
            objectDb.Received().AddInParameter("ReportType", DbType.Int32, _metaModel.ReportBy);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_MicroGamingWinloss")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            IDictionary<string, MicroGamingWinLoss> summaryTotal;
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "typeid", 1 },
                { "winlostdate", "20170101" },
                { "TurnoverUSD", 100 }
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
            var result = _dataAccess.GetSummaryWinloss(_metaModel, out summaryTotal);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].TypeID, Is.EqualTo(1));
                Assert.That(result[0].Date, Is.EqualTo(new DateTime(2017, 1, 1)));
                Assert.That(result[0].TurnOver, Is.EqualTo(100));

                Assert.That(summaryTotal["USD"].TurnOver, Is.EqualTo(100));
            });
        }
    }
}