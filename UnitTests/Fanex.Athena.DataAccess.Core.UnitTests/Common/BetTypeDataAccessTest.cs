namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Data;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BetTypeDataAccessTest
    {
        private IObjectDb _objectDb;
        private BetTypeDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<BetTypeDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetBettypes_WhenCall_PassCorrectParameters()
        {
            // Arrange
            int licenseeId = 1;
            var sports = "1,2,3";
            var languageCode = "en-US";
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SelBetTypeBySport")).Returns(_objectDb);

            // Action
            _dataAccess.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("sportlist", DbType.String, sports);
            _objectDb.Received().AddInParameter("langkey", DbType.String, languageCode);
        }

        [Test]
        public void GetBettypes_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            int licenseeId = 1;
            var sports = "1,2,3";
            var languageCode = "en-US";
            var expected = new CheckedItem { Checked = true, ID = 1, Name = "BettypeNameTest" };

            _dataAccess.GetObjectDb(Arg.Is("Lrf_SelBetTypeBySport")).Returns(_objectDb);
            _reader.Read().Returns(true, false);
            _reader["BettypeID"].Returns(expected.ID);
            _reader["BettypeName"].Returns(expected.Name);

            // Action
            var result = _dataAccess.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result[0].Checked, Is.True);
            Assert.That(result[0].ID, Is.EqualTo(expected.ID));
            Assert.That(result[0].Name, Is.EqualTo(expected.Name));
        }
    }
}