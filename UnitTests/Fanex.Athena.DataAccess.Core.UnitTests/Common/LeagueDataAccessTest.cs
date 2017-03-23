namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using Core.Common.Interfaces;
    using Data;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LeagueDataAccessTest
    {
        [Test]
        public void GetLeaguesBySport_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<LeagueDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelLeagueBySport")).Returns(objectDb);
            var metaModel = new LeagueMetaModel
            {
                AgentListXml = "hello",
                SportId = 4,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 11)
            };

            // Action
            dataAccess.GetLeaguesBySport(metaModel);

            // Assert
            objectDb.Received().AddInParameter("sportId", DbType.Int32, metaModel.SportId);
            objectDb.Received().AddInParameter("Fromdate", DbType.DateTime, metaModel.FromDate);
            objectDb.Received().AddInParameter("Todate", DbType.DateTime, metaModel.ToDate);
            objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}