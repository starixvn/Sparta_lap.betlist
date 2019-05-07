namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
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

        private readonly BalanceUp _balance = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 8, 10),
            BalanceUpDay = new DateTime(2016, 8, 31)
        };

        private readonly MatchWinLossMetaModel _metaModel = new MatchWinLossMetaModel
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

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();

            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            DataAccess = Substitute.ForPartsOf<MatchWinLossDataAccess>(
                balanceUpDataAccess, 
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public async Task GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDataListSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("Date", _metaModel.Date);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", _balance.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
                ObjectDb.Received(1).SetParameterValue("showNG", false);
            });
        }

        [Test]
        public async Task SetToList_WhenCall_ReturnCorrectResult()
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
            var result = await DataAccess.GetDataList(_metaModel);

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
        public async Task SetToList_HasCustomerComm_ReturnCorrectData()
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
            var result = await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].CustomerComm, Is.EqualTo(expected.PlayerCommUSD));
        }

        [Test]
        public async Task SetToList_HasBetTypeColumn_ReturnCorrectData()
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
            var result = await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].BetType, Is.EqualTo(expected.bettype));
        }

        [Test]
        public async Task SetToList_HasProductNameColumn_ReturnCorrectData()
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
            var result = await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].ProductName, Is.EqualTo(expected.productname));
        }

        [Test]
        public async Task SetToList_HasLiveIndicatorColumn_ReturnCorrectData()
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
            var result = await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].IsLive, Is.EqualTo(expected.liveindicator));
        }

        [Test]
        public async Task SetToList_HasShowTimeColumn_ReturnCorrectData()
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
            var result = await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].ShowTime, Is.EqualTo(expected.showtime));
        }

        [Test]
        public async Task SetToList_HasSportTypeColumn_ReturnCorrectData()
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
            var result = await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].SportType, Is.EqualTo(expected.sporttype));
        }

        [Test]
        public async Task SetToList_HasKickOffTimeColumn_ReturnCorrectData()
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
            var result = await DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.That(result[0].KickOffTime, Is.EqualTo(expected.KickoffTime));
        }

        [Test]
        public async Task GetDetailDataSet_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailDataSetSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetDetailDataSet(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", _metaModel.Date);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", _balance.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("PageSize", _metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
                ObjectDb.Received(1).SetParameterValue("showNG", false);
            });
        }

        [Test]
        public async Task GetDetailDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailDataListSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetDetailDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("MatchId", _metaModel.MatchId);
                ObjectDb.Received(1).SetParameterValue("Date", _metaModel.Date);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", _balance.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("LeagueId", _metaModel.LeagueId);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
            });
        }

        [Test]
        public async Task GetOutrightParlayList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetOutrightParlayListSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetOutrightParlayList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("Date", _metaModel.Date);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", _balance.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("showsb", true);
                ObjectDb.Received(1).SetParameterValue("showba", false);
            });
        }
    }
}