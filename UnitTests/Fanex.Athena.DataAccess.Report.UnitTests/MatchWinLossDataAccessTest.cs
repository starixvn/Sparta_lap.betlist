namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class MatchWinLossDataAccessTest : BaseDataAccessHelperTest<MatchWinLossDataAccess>
    {
        private const string GetDataListSp = "Lrf_Rpt_MatchWinLoss";
        private const string GetDetailDataSetSp = "Lrf_Rpt_MatchWinLossDetail";
        private const string GetDetailDataListSp = "Lrf_Rpt_MatchWinLossByBettype";
        private const string GetOutrightParlayListSp = "Lrf_Rpt_MatchWinLossByOutrightParlay";

        private BalanceUp _balance;
        private MatchWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            ObjectDb = Substitute.For<IObjectDb>();
            DataAccess = Substitute.ForPartsOf<MatchWinLossDataAccess>(balanceUpDataAccess, Substitute.For<ICacheRefAdapter>());

            _metaModel = new MatchWinLossMetaModel
            {
                BaseCurrency = "USD",
                ProductCodeList = "SB",
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true },
                    new ProductOption { Code = "NG", Status = false }
                },
                AgentListXml = "<xml>",
                Date = new DateTime(2017, 1, 1),
                PageSize = 100
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
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("Date", DbType.DateTime, _metaModel.Date);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.Received(1).AddInParameter("showNG", DbType.Boolean, false);
            });
        }

        [Test]
        public void SetToList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var expected = new
            {
                leagueid = 1,
                matchid = 2,
                TurnoverUSD = 100,
                MemberWinLostUSD = 200
            };

            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].LeagueId, Is.EqualTo(expected.leagueid));
                Assert.That(result[0].MatchId, Is.EqualTo(expected.matchid));
                Assert.That(result[0].Turnover, Is.EqualTo(expected.TurnoverUSD));
                Assert.That(result[0].CustomerWL, Is.EqualTo(expected.MemberWinLostUSD));
            });
        }

        [Test]
        public void SetToList_HasCustomerComm_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("PlayerCommUSD");

            var expected = new
            {
                PlayerCommUSD = 200
            };
            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].CustomerComm, Is.EqualTo(expected.PlayerCommUSD));
        }

        [Test]
        public void SetToList_HasBetTypeColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("bettype");

            var expected = new
            {
                bettype = 2
            };
            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].BetType, Is.EqualTo(expected.bettype));
        }

        [Test]
        public void SetToList_HasProductNameColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("productname");

            var expected = new
            {
                productname = "Test"
            };
            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].ProductName, Is.EqualTo(expected.productname));
        }

        [Test]
        public void SetToList_HasLiveIndicatorColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("liveindicator");

            var expected = new
            {
                liveindicator = false
            };
            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].IsLive, Is.EqualTo(expected.liveindicator));
        }

        [Test]
        public void SetToList_HasShowTimeColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("showtime");

            var expected = new
            {
                showtime = "20170101"
            };
            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].ShowTime, Is.EqualTo(expected.showtime));
        }

        [Test]
        public void SetToList_HasSportTypeColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("sporttype");

            var expected = new
            {
                sporttype = 1
            };
            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].SportType, Is.EqualTo(expected.sporttype));
        }

        [Test]
        public void SetToList_HasKickOffTimeColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("KickoffTime");

            var expected = new
            {
                KickoffTime = new DateTime(2017, 1, 2)
            };
            MapReaderData(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].KickOffTime, Is.EqualTo(expected.KickoffTime));
        }

        [Test]
        public void GetDetailDataSet_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailDataSetSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetDetailDataSet(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, _metaModel.Date);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.Received(1).AddInParameter("showNG", DbType.Boolean, false);
            });
        }

        [Test]
        public void GetDetailDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailDataListSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetDetailDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("MatchId", DbType.Int32, _metaModel.MatchId);
                ObjectDb.Received(1).AddInParameter("Date", DbType.DateTime, _metaModel.Date);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("LeagueId", DbType.Int32, _metaModel.LeagueId);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
            });
        }

        [Test]
        public void GetOutrightParlayList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetOutrightParlayListSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetOutrightParlayList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("Date", DbType.DateTime, _metaModel.Date);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("showsb", DbType.Boolean, true);
                ObjectDb.Received(1).AddInParameter("showba", DbType.Boolean, false);
            });
        }
    }
}