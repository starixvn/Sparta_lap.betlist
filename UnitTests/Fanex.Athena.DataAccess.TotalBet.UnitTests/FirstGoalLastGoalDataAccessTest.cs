namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Data;
    using System.Linq;
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
            var result = _dataAccess.GetFirstGoalLastGoalDataList(metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.That(firstItem.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(firstItem.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(firstItem.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(firstItem.IsNeutral, Is.EqualTo(expected.IsNeutral));
            Assert.That(firstItem.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(firstItem.AwayId, Is.EqualTo(expected.AwayId));

            Assert.That(firstItem.HomeFirstGoal, Is.EqualTo(expected.HomeFirstGoal));
            Assert.That(firstItem.HomeLastGoal, Is.EqualTo(expected.HomeLastGoal));
            Assert.That(firstItem.AwayFirstGoal, Is.EqualTo(expected.AwayFirstGoal));
            Assert.That(firstItem.AwayLastGoal, Is.EqualTo(expected.AwayLastGoal));
            Assert.That(firstItem.NoGoal, Is.EqualTo(expected.NoGoal));
            Assert.That(firstItem.HomeFirstHalfFirstGoal, Is.EqualTo(expected.HomeFirstHalfFirstGoal));
            Assert.That(firstItem.HomeFirstHalfLastGoal, Is.EqualTo(expected.HomeFirstHalfLastGoal));
            Assert.That(firstItem.AwayFirstHalfFirstGoal, Is.EqualTo(expected.AwayFirstHalfFirstGoal));
            Assert.That(firstItem.AwayFirstHalfLastGoal, Is.EqualTo(expected.AwayFirstHalfLastGoal));
            Assert.That(firstItem.FirstHalfNoGoal, Is.EqualTo(expected.FirstHalfNoGoal));
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