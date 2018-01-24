namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class LastBetsMonitoringBetListDataAccessTest : BaseDataAccessHelperTest<LastBetsMonitoringBetListDataAccess>
    {
        private const string UspKey = "Lrf_Monitor_BetlistSel_New";

        [Test]
        public void GetLastBetsMonitoringData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            var metaModel = GetLastBetsMonitoringMetaModel();

            // Action
            DataAccess.GetLastBetsMonitoringData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received().AddInParameter("BetType", DbType.String, metaModel.BetType);
                ObjectDb.Received().AddInParameter("Currency", DbType.String, metaModel.Currency);
                ObjectDb.Received().AddInParameter("SpecialLevel", DbType.String, metaModel.Level);
                ObjectDb.Received().AddInParameter("TransId", DbType.Int64, metaModel.TransId);
                ObjectDb.Received().AddInParameter("LimitRowCount", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received().AddInParameter("SportId", DbType.Int32, metaModel.SportId);
                ObjectDb.Received().AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
                ObjectDb.Received().AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                ObjectDb.Received().AddInParameter("MinStake", DbType.Decimal, metaModel.StakeFrom);
                ObjectDb.Received().AddInParameter("MaxStake", DbType.Decimal, metaModel.StakeTo);
                ObjectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveStatus);
                ObjectDb.Received().AddInParameter("BaseCurrency", DbType.String, metaModel.BasedCurrency);
                ObjectDb.Received().AddInParameter("BetFrom", DbType.Byte, metaModel.BetFrom);
            });
        }

        [Test]
        public void GetLastBetsMonitoringData_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                UspKey,
                () => DataAccess.GetLastBetsMonitoringData(new LastBetsMonitoringMetaModel()));
        }

        [Test]
        public void GetMatchList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);
            var expected = new BetListMatch { MatchId = 1, LeagueId = 2 };
            ObjectDb.Query<BetListMatch>().Returns(new List<BetListMatch> { expected });

            // Action
            var result = DataAccess.GetMatchList("SPNameTest")[0];

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetMatchList_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            const string MatchListSp = "SPNameTest";
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                MatchListSp,
                () => DataAccess.GetMatchList(MatchListSp));
        }

        private LastBetsMonitoringMetaModel GetLastBetsMonitoringMetaModel()
        {
            return new LastBetsMonitoringMetaModel
            {
                AgentListXml = "abc",
                BetType = "BetTypeTest",
                Currency = "CurrencyTest",
                Level = "LevelTest",
                TransId = 3,
                PageSize = 4,
                SportId = 5,
                LeagueId = 6,
                MatchId = 7,
                StakeFrom = 10,
                StakeTo = 20,
                LiveStatus = 8,
                BasedCurrency = "BasedCurrencyTest",
                BetFrom = 9
            };
        }
    }
}