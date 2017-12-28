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
            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelLeagueBySport")).Returns(ObjectDb);
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
                ObjectDb.Received().AddInParameter("sportId", DbType.Int32, metaModel.SportId);
                ObjectDb.Received().AddInParameter("Fromdate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received().AddInParameter("Todate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            });
        }
    }
}