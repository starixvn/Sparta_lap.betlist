namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Common;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelSportDetail")).Returns(_objectDb);
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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelSportDetail")).Returns(_objectDb);
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
        public void GetAllSports_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SportsGetAll")).Returns(_objectDb);

            // Action
            _dataAccess.GetAllSports();

            // Assert
            _objectDb.Received().ExecuteReader();
        }

        [Test]
        public void GetAllWHSports_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SportSel")).Returns(_objectDb);

            // Action
            _dataAccess.GetAllWHSports();

            // Assert
            _objectDb.Received().ExecuteReader();
        }
    }
}