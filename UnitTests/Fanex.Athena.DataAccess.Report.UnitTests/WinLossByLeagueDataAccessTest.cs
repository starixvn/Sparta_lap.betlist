namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class WinLossByLeagueDataAccessTest
    {
        private WinLossByLeagueDataAccess _dataAccess;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.ForPartsOf<WinLossByLeagueDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
        }

        [Test]
        public void GetWinlossByLeagues_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                AgentListXml = "xml",
                Odds = 1,
                League = 2,
                Match = 3,
                ShowWin = true,
                ShowLoss = true,
                ShowDraw = true,
                BalanceUpDay = new DateTime(2018, 1, 1),
                MoveBetsDay = new DateTime(2018, 2, 2),
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true }
                }
            };
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlostByLeague")).Returns(objectDb);

            // Action
            _dataAccess.GetSummary(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                objectDb.Received().AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                objectDb.Received().AddInParameter("odds", DbType.Decimal, metaModel.Odds);
                objectDb.Received().AddInParameter("leagueid", DbType.Int32, metaModel.League);
                objectDb.Received().AddInParameter("matchid", DbType.Int32, metaModel.Match);
                objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
                objectDb.Received().AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
                objectDb.Received().AddInParameter("showwin", DbType.Boolean, metaModel.ShowWin);
                objectDb.Received().AddInParameter("showloss", DbType.Boolean, metaModel.ShowLoss);
                objectDb.Received().AddInParameter("showdraw", DbType.Boolean, metaModel.ShowDraw);
                objectDb.Received().AddInParameter("showsb", DbType.Boolean, true);
                objectDb.Received().AddInParameter("showba", DbType.Boolean, false);
                objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            });
        }

        [Test]
        public void GetWinlossByLeagues_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlostByLeague")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "turnoverUSD", 999 }
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetSummary(new WinLossByLeagueMetaModel
            {
                BaseCurrency = "USD"
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(result.ElementAt(0).TurnOver, Is.EqualTo(999));
            });
        }

        [Test]
        public void GetWinlossByLeagueCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                AgentListXml = "xml",
                Odds = 1,
                League = 2,
                Match = 3,
                ShowWin = true,
                ShowLoss = true,
                ShowDraw = true,
                BalanceUpDay = new DateTime(2018, 1, 1),
                MoveBetsDay = new DateTime(2018, 2, 2),
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true }
                }
            };
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlossByLeagueDetail")).Returns(objectDb);
            objectDb.GetParameterValue("totalrows").Returns(1000);

            // Action
            var result = _dataAccess.GetCustomers(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                objectDb.Received().AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                objectDb.Received().AddInParameter("odds", DbType.Decimal, metaModel.Odds);
                objectDb.Received().AddInParameter("leagueid", DbType.Int32, metaModel.League);
                objectDb.Received().AddInParameter("matchid", DbType.Int32, metaModel.Match);
                objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
                objectDb.Received().AddInParameter("showwin", DbType.Boolean, metaModel.ShowWin);
                objectDb.Received().AddInParameter("showloss", DbType.Boolean, metaModel.ShowLoss);
                objectDb.Received().AddInParameter("showdraw", DbType.Boolean, metaModel.ShowDraw);
                objectDb.Received().AddInParameter("showsb", DbType.Boolean, true);
                objectDb.Received().AddInParameter("showba", DbType.Boolean, false);
                objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

                Assert.That(result.TotalRows, Is.EqualTo(1000));
            });
        }

        [Test]
        public void GetWinlossByLeagueCustomers_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlossByLeagueDetail")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "turnoverUSD", 999 }
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetCustomers(
                new WinLossByLeagueMetaModel
                {
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).TurnOver, Is.EqualTo(999));
                Assert.That(result.TotalData.TurnOver, Is.EqualTo(999));
            });
        }
    }
}