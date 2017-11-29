namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using Data;
    using Interfaces;
    using Model.BetSettings.MetaModel;
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
            var metaModel = new BetSettingCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = 3,
                Language = "Lao",
                Ip = "123"
            };
            const int RngCasinoSportId = 251;

            // Action
            _dataAccess.UpdateStatus(metaModel);

            // Assert
            _coreCasinoBetSettingDataAccess.Received().UpdateCasinoStatus(metaModel, RngCasinoSportId);
        }
    }
}