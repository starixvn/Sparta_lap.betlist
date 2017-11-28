namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Common;
    using Data;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SportTypeDataAccessTest
    {
        private IObjectDb _objectDb;
        private SportTypeDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<SportTypeDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetSportsAndProducts_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SelSportDetail")).Returns(_objectDb);
            var licenseeId = 1;
            var languageCode = "en-US";

            // Action
            _dataAccess.GetSportsAndProducts(licenseeId, languageCode);

            // Assert
            _objectDb.Received().AddInParameter("licenseeId", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("langkey", DbType.String, languageCode);
        }

        [Test]
        public void GetSportsAndProducts_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SelSportDetail")).Returns(_objectDb);
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, true, false);
            reader["ParentId"].Returns(DBNull.Value, 1);
            reader["ID"].Returns(0);
            reader["Name"].Returns(string.Empty);

            // Action
            var result = _dataAccess.GetSportsAndProducts(Arg.Any<int>(), Arg.Any<string>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.ElementAt(0).ParentId, Is.EqualTo(-1));
                Assert.That(result.ElementAt(1).ParentId, Is.EqualTo(1));
            });
        }

        [Test]
        public void GetAllSports_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int SportType = 100;
            var reader = Substitute.For<IDataReader>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SportsGetAll")).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            DataAccessTestHelper.MapReaderData(reader, new { Sporttype = SportType });

            // Action
            var result = _dataAccess.GetAllSports();

            // Assert
            Assert.That(result.ContainsKey(SportType), Is.True);
        }

        [Test]
        public void GetAllWHSports_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int SportType = 100;
            var reader = Substitute.For<IDataReader>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SportSel")).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            DataAccessTestHelper.MapReaderData(reader, new { Sporttype = SportType });

            // Action
            var result = _dataAccess.GetAllWHSports();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.First().SportId, Is.EqualTo("0"));
                Assert.That(result.ElementAt(1).SportId, Is.EqualTo(SportType.ToString()));
            });
        }
    }
}