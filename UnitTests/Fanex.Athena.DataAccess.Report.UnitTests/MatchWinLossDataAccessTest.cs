namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MatchWinLossDataAccessTest
    {
        private MatchWinLossDataAccess _dataAccess;
        private BalanceUp _balance;
        private MatchWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };

            _dataAccess = Substitute.ForPartsOf<MatchWinLossDataAccess>(_balance);

            _metaModel = new MatchWinLossMetaModel
            {
                BaseCurrency = "USD",
                ProductOptions = GetProductOptions(),
                AgentListXml = "<xml>",
                Date = new DateTime(2017, 1, 1),
                PageSize = 100
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
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchWinLoss")).Returns(objectDb);

            // Action
            _dataAccess.GetDataList(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("Date", DbType.DateTime, _metaModel.Date);
            objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchWinLoss")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "leagueid", 1 },
                { "matchid", 2 },
                { "TurnoverUSD", 100 },
                { "MemberWinLostUSD", 200 }
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
            var result = _dataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].LeagueId, Is.EqualTo(1));
                Assert.That(result[0].MatchId, Is.EqualTo(2));
                Assert.That(result[0].Turnover, Is.EqualTo(100));
                Assert.That(result[0].CustomerWL, Is.EqualTo(200));
            });
        }

        [Test]
        public void GetDetailDataSet_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchWinLossDetail")).Returns(objectDb);

            // Action
            _dataAccess.GetDetailDataSet(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.Date);
            objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetDetailDataSet_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchWinLossDetail")).Returns(objectDb);

            // Action
            _dataAccess.GetDetailDataSet(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.Date);
            objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetDetailDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchWinLossByBettype")).Returns(objectDb);

            // Action
            _dataAccess.GetDetailDataList(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("MatchId", DbType.Int32, _metaModel.MatchId);
            objectDb.Received().AddInParameter("Date", DbType.DateTime, _metaModel.Date);
            objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("LeagueId", DbType.Int32, _metaModel.LeagueId);
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetOutrightParlayList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchWinLossByOutrightParlay")).Returns(objectDb);

            // Action
            _dataAccess.GetOutrightParlayList(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("Date", DbType.DateTime, _metaModel.Date);
            objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("showsb", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showba", DbType.Boolean, false);
        }

        private IList<ProductOption> GetProductOptions()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            };
        }
    }
}