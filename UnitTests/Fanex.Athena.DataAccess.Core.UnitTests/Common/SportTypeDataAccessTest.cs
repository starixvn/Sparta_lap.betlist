namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SportTypeDataAccessTest
    {
        private IObjectDb _objectDb;
        private SportTypeDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<SportTypeDataAccess>();
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