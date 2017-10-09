namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using Data;
    using Interfaces;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RngCasinoBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private ICoreCasinoBetSettingDataAccess _coreCasinoBetSettingDataAccess;
        private RngCasinoBetSettingDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _coreCasinoBetSettingDataAccess = Substitute.For<ICoreCasinoBetSettingDataAccess>();
            _dataAccess = Substitute.ForPartsOf<RngCasinoBetSettingDataAccess>(_coreCasinoBetSettingDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void UpdateStatus_WhenCall_CallCoreCasinoBetSettingDataAccess()
        {
            // Arrange
            const int UserId = 1;
            const int CustId = 2;
            const int Status = 3;
            const string Language = "Lao";
            const string Ip = "123";
            const int RngCasinoSportId = 251;

            // Action
            _dataAccess.UpdateStatus(UserId, CustId, Status, Language, Ip);

            // Assert
            _coreCasinoBetSettingDataAccess.Received().UpdateCasinoStatus(UserId, CustId, RngCasinoSportId, Status, Language, Ip);
        }
    }
}