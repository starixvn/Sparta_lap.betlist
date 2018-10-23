namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Common;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Interfaces;
    using TestHelpers;
    using VendorAdapter.Interfaces;

    public class LeagueDataAccessTest : BaseDataAccessHelperTest<LeagueDataAccess>
    {
        private ICacheRefAdapter _cacheRef;

        protected override void SetUpDataAccess()
        {
            _cacheRef = Substitute.For<ICacheRefAdapter>();
            DataAccess = Substitute.ForPartsOf<LeagueDataAccess>(
                AppSetting,
                Substitute.For<ISessionStorage>(),
                _cacheRef);
        }

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
        public void GetLeaguesBySport_WhenCall_ReturnData()
        {
            // Arrange
            const int LeagueId = 1234;
            const string LeagueName = "League 1234 xxx";
            DataAccess.GetObjectDb(Arg.Is("Lrf_SelLeagueBySport")).Returns(ObjectDb);
            Reader.Read().Returns(true, false);
            MapReaderData(new
            {
                leagueid = LeagueId
            });
            _cacheRef.GetLeagueName(LeagueId).Returns(LeagueName);

            // Action
            var result = DataAccess.GetLeaguesBySport(new LeagueMetaModel());
            var firstItem = result.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.LeagueId, Is.EqualTo(LeagueId));
                Assert.That(firstItem.LeagueName, Is.EqualTo(LeagueName));
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