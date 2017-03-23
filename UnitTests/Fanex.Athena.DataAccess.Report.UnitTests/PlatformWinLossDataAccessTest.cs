namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public class PlatformWinLossDataAccessTest
    {
        private PlatformWinLossDataAccess _dataAccess;
        private BalanceUp _balance;
        private PlatformWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };

            _dataAccess = Substitute.ForPartsOf<PlatformWinLossDataAccess>(_balance);

            _metaModel = new PlatformWinLossMetaModel
            {
                LicenseeId = 1,
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                Year = 2017,
                Month = 1,
                FromDay = 1,
                ToDay = 2
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
        public void GetSummaryData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinloss")).Returns(objectDb);
            PlatformWinLossSummary totalData;

            // Action
            _dataAccess.GetSummaryData(_metaModel, out totalData);

            // Assert
            objectDb.Received().AddInParameter("PlatformType", DbType.Byte, _metaModel.PlatformType);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("DrawFilter", DbType.Int16, _metaModel.DrawFilter);
            objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
        }

        [Test]
        public void GetSummaryData_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinloss")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            PlatformWinLossSummary totalData;
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "PlatformType", PlatformType.Asia },
                { "TurnoverUSD", 100 },
                { "productprefix", "SB" }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetSummaryData(_metaModel, out totalData);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[0].PlatformType, Is.EqualTo(PlatformType.Asia));
                Assert.That(result[0].Name, Is.EqualTo(ViewResources.Asia));
                Assert.That(result[0].Turnover, Is.EqualTo(100));

                Assert.That(result[1].Parent, Is.EqualTo(PlatformType.Asia.ToString()));
                Assert.That(result[1].Name, Is.EqualTo(ViewResources.ProductSB));
                Assert.That(result[1].Turnover, Is.EqualTo(100));

                Assert.That(totalData.TurnoverPercent, Is.EqualTo(100));
            });
        }

        [Test]
        public void GetMemberData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinlossDetail")).Returns(objectDb);
            PlatformWinLossMember totalData;

            // Action
            _dataAccess.GetMemberData(_metaModel, out totalData);

            // Assert
            objectDb.Received().AddInParameter("PlatformType", DbType.Byte, _metaModel.PlatformType);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("DrawFilter", DbType.Int16, _metaModel.DrawFilter);
            objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
        }

        [Test]
        public void GetMemberData_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinlossDetail")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            PlatformWinLossMember totalData;
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "TurnoverUSD", 100 },
                { "WinlostUSD", 200 }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetMemberData(_metaModel, out totalData);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].Turnover, Is.EqualTo(100));
                Assert.That(result[0].CustomerWinLoss, Is.EqualTo(200));

                Assert.That(totalData.Turnover, Is.EqualTo(100));
            });
        }

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}