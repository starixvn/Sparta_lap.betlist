namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Data;
    using Data;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TotalGoalDataAccessTest
    {
        private IObjectDb _objectDb;
        private TotalGoalDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<TotalGoalDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [Test]
        public void GetTotalGoalDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_TotalBets_Sel6")).Returns(_objectDb);
            var metaModel = new TotalGoalMetaModel();
            var totalData = new TotalGoal();
            var expected = new TotalGoal
            {
                MatchId = 1,
                HomeId = 2,
                AwayId = 3,
                Total01 = 10m,
                Total23 = 11m,
                Total46 = 12m,
                Total7Over = 13m,
                TotalFirstHalf01 = 20m,
                TotalFirstHalf23 = 20.5m,
                Total4Over = 21m
            };
            ConfigureReaderForGetTotalGoalDataListMethod(expected);

            // Action
            var result = _dataAccess.GetTotalGoalDataList(metaModel, out totalData)[0];

            // Assert
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));
            Assert.That(result.Total01, Is.EqualTo(expected.Total01));
            Assert.That(result.Total23, Is.EqualTo(expected.Total23));
            Assert.That(result.Total46, Is.EqualTo(expected.Total46));
            Assert.That(result.Total7Over, Is.EqualTo(expected.Total7Over));
            Assert.That(result.TotalFirstHalf01, Is.EqualTo(expected.TotalFirstHalf01));
            Assert.That(result.TotalFirstHalf23, Is.EqualTo(expected.TotalFirstHalf23));
            Assert.That(result.Total4Over, Is.EqualTo(expected.Total4Over));
        }

        private void ConfigureReaderForGetTotalGoalDataListMethod(TotalGoal expected)
        {
            _reader.Read().Returns(true, false);

            _reader["matchid"].Returns(expected.MatchId);
            _reader["homeID"].Returns(expected.HomeId);
            _reader["awayID"].Returns(expected.AwayId);
            _reader["isNeutral"].Returns(expected.IsNeutral);
            _reader["total01"].Returns(expected.Total01);
            _reader["total23"].Returns(expected.Total23);
            _reader["total46"].Returns(expected.Total46);
            _reader["total7over"].Returns(expected.Total7Over);
            _reader["fhtotal01"].Returns(expected.TotalFirstHalf01);
            _reader["fhtotal23"].Returns(expected.TotalFirstHalf23);
            _reader["fhtotal4over"].Returns(expected.Total4Over);
        }
    }
}