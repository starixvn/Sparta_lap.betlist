namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class OutrightDataAccessTest : BaseDataAccessHelperTest<OutrightDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel10")).Returns(ObjectDb);
        }

        [Test]
        public void GetOutrightDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new OutrightMetaModel();
            var expected = new Outright
            {
                LeagueId = 1,
                TeamId = 2,
                MatchId = 3,
                Forecast = 10m,
                Balance = 10.5m
            };
            ConfigureReaderForGetOutrightDataListMethod(expected);

            // Action
            var result = DataAccess.GetOutrightDataList(metaModel)[0];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.LeagueId, Is.EqualTo(expected.LeagueId));
                Assert.That(result.TeamId, Is.EqualTo(expected.TeamId));
                Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
                Assert.That(result.Forecast, Is.EqualTo(expected.Forecast));
                Assert.That(result.Balance, Is.EqualTo(expected.Balance));
            });
        }

        private void ConfigureReaderForGetOutrightDataListMethod(Outright expected)
        {
            Reader.Read().Returns(true, false);

            MapReaderData(new
            {
                leagueid = expected.LeagueId,
                teamid = expected.TeamId,
                orid = expected.MatchId,
                totalwinlost = expected.Forecast,
                balance = expected.Balance
            });
        }
    }
}