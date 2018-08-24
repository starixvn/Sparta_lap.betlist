namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using Core.Common;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class LeagueDataAccessTest : BaseDataAccessHelperTest<LeagueDataAccess>
    {
        [Test]
        public void GetLeaguesBySport_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_SelLeagueBySport")).Returns(ObjectDb);
            var metaModel = new LeagueMetaModel
            {
                AgentListXml = "hello",
                SportId = 4,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 11)
            };

            // Action
            DataAccess.GetLeaguesBySport(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("sportId", DbType.Int32, metaModel.SportId);
                ObjectDb.Received(1).AddInParameter("Fromdate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("Todate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            });
        }

        [Test]
        public void GetRunningLeagues_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const string AgentList = "<sdfsdfml>";
            const string BetTypes = "1,2,3,4,5";
            DataAccess.GetObjectDb(Arg.Is("Lrf_LeagueSelRunning")).Returns(ObjectDb);

            // Action
            DataAccess.GetRunningLeagues(AgentList, BetTypes);

            // Assert
            ObjectDb.Received().AddInParameter("AgentList", DbType.String, AgentList);
            ObjectDb.Received().AddInParameter("betTypes", DbType.String, BetTypes);
        }
    }
}