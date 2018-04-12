namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class LastBetsMonitoringBetListDataAccessTest : BaseDataAccessHelperTest<LastBetsMonitoringBetListDataAccess>
    {
        private const string UspKey = "Lrf_Monitor_BetlistSel_New";

        [Test]
        public void GetLastBetsMonitoringData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

            // Action
            DataAccess.GetLastBetsMonitoringData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("BetType", DbType.String, metaModel.BetType);
                ObjectDb.Received(1).AddInParameter("Currency", DbType.String, metaModel.Currency);
                ObjectDb.Received(1).AddInParameter("SpecialLevel", DbType.String, metaModel.Level);
                ObjectDb.Received(1).AddInParameter("TransId", DbType.Int64, metaModel.TransId);
                ObjectDb.Received(1).AddInParameter("LimitRowCount", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("SportId", DbType.Int32, metaModel.SportId);
                ObjectDb.Received(1).AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
                ObjectDb.Received(1).AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                ObjectDb.Received(1).AddInParameter("MinStake", DbType.Decimal, metaModel.StakeFrom);
                ObjectDb.Received(1).AddInParameter("MaxStake", DbType.Decimal, metaModel.StakeTo);
                ObjectDb.Received(1).AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveStatus);
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, metaModel.BasedCurrency);
                ObjectDb.Received(1).AddInParameter("BetFrom", DbType.Byte, metaModel.BetFrom);
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
    }
}