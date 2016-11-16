namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Data;
    using Data;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using TotalBet;

    [TestFixture]
    public class FirstGoalLastGoalDataAccessTest
    {
        private IObjectDb _objectDb;
        private FirstGoalLastGoalDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<FirstGoalLastGoalDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetFirstGoalLastGoalDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_TotalBets_Sel14")).Returns(_objectDb);
            var metaModel = new FirstGoalLastGoalMetaModel();
            var totalData = new FirstGoalLastGoal();
            var expected = new FirstGoalLastGoal
            {
                LeagueId = 1,
                MatchId = 2,
                ShowTime = "ShowTimeTest",
                IsNeutral = 0,
                HomeId = 3,
                AwayId = 4,
                HomeFirstGoal = 10m,
                HomeLastGoal = 10.5m,
                HomeFirstHalfFirstGoal = 11m,
                HomeFirstHalfLastGoal = 11.5m,
                AwayFirstHalfFirstGoal = 12m,
                AwayFirstHalfLastGoal = 12.5m,
                FirstHalfNoGoal = 13m
            };

            ConfigureReaderForGetFirstGoalLastGoalDataListMethod(expected);

            // Action
            var result = _dataAccess.GetFirstGoalLastGoalDataList(metaModel, out totalData)[0];

            // Assert
            Assert.That(result.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));

            Assert.That(result.HomeFirstGoal, Is.EqualTo(expected.HomeFirstGoal));
            Assert.That(result.HomeLastGoal, Is.EqualTo(expected.HomeLastGoal));
            Assert.That(result.AwayFirstGoal, Is.EqualTo(expected.AwayFirstGoal));
            Assert.That(result.AwayLastGoal, Is.EqualTo(expected.AwayLastGoal));
            Assert.That(result.NoGoal, Is.EqualTo(expected.NoGoal));
            Assert.That(result.HomeFirstHalfFirstGoal, Is.EqualTo(expected.HomeFirstHalfFirstGoal));
            Assert.That(result.HomeFirstHalfLastGoal, Is.EqualTo(expected.HomeFirstHalfLastGoal));
            Assert.That(result.AwayFirstHalfFirstGoal, Is.EqualTo(expected.AwayFirstHalfFirstGoal));
            Assert.That(result.AwayFirstHalfLastGoal, Is.EqualTo(expected.AwayFirstHalfLastGoal));
            Assert.That(result.FirstHalfNoGoal, Is.EqualTo(expected.FirstHalfNoGoal));
        }

        private void ConfigureReaderForGetFirstGoalLastGoalDataListMethod(FirstGoalLastGoal expected)
        {
            _reader.Read().Returns(true, false);

            _reader["leagueid"].Returns(expected.LeagueId);
            _reader["matchid"].Returns(expected.MatchId);
            _reader["showtime"].Returns(expected.ShowTime);
            _reader["isNeutral"].Returns(expected.IsNeutral);
            _reader["homeid"].Returns(expected.HomeId);
            _reader["awayid"].Returns(expected.AwayId);

            _reader["winlost_homeFG"].Returns(expected.HomeFirstGoal);
            _reader["winlost_homeLG"].Returns(expected.HomeLastGoal);
            _reader["winlost_awayFG"].Returns(expected.AwayFirstGoal);
            _reader["winlost_awayLG"].Returns(expected.AwayLastGoal);
            _reader["winlost_NoGoal"].Returns(expected.NoGoal);
            _reader["winlost_1HhomeFG"].Returns(expected.HomeFirstHalfFirstGoal);
            _reader["winlost_1HhomeLG"].Returns(expected.HomeFirstHalfLastGoal);
            _reader["winlost_1HawayFG"].Returns(expected.AwayFirstHalfFirstGoal);
            _reader["winlost_1HawayLG"].Returns(expected.AwayFirstHalfLastGoal);
            _reader["winlost_1HNoGoal"].Returns(expected.FirstHalfNoGoal);
        }
    }
}