namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Common;
    using Extension.Utility;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;
    using VendorAdapter.Interfaces;

    public class LeagueDataAccessTest : BaseDataAccessHelperTest<LeagueDataAccess>
    {
        private ICacheRefAdapter _cacheRef;

        private IAppSettings _appSettings;

        protected override void SetUpDataAccess()
        {
            _cacheRef = Substitute.For<ICacheRefAdapter>();
            _appSettings = Substitute.For<IAppSettings>();
            DataAccess = Substitute.ForPartsOf<LeagueDataAccess>(_cacheRef, _appSettings);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void GetLeaguesBySport_WhenCall_ParametersPassedCorrectly(bool isProduction)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_SelLeagueBySport")).Returns(ObjectDb);
            _appSettings.IsProduction.Returns(isProduction);
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
                ObjectDb.Received(1).AddInParameter("IsIncludeTestLeague", DbType.Boolean, !isProduction);
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

        [TestCase(false)]
        [TestCase(true)]
        public void GetRunningLeagues_WhenCall_ParametersPassedCorrectly(bool isProduction)
        {
            // Arrange
            const string AgentList = "<sdfsdfml>";
            const string BetTypes = "1,2,3,4,5";
            DataAccess.GetObjectDb(Arg.Is("Lrf_LeagueSelRunning")).Returns(ObjectDb);
            _appSettings.IsProduction.Returns(isProduction);

            // Action
            DataAccess.GetRunningLeagues(AgentList, BetTypes);

            // Assert
            ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, AgentList);
            ObjectDb.Received(1).AddInParameter("betTypes", DbType.String, BetTypes);
            ObjectDb.Received(1).AddInParameter("IsIncludeTestLeague", DbType.Boolean, !isProduction);
        }
    }
}