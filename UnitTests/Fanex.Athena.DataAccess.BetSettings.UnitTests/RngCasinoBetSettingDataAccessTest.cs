namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using Core.UnitTests.TestHelpers;
    using Data;
    using Interfaces;
    using Model.BetSettings.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RngCasinoBetSettingDataAccessTest : BaseDataAccessHelperTest<RngCasinoBetSettingDataAccess>
    {
        private ICoreCasinoBetSettingDataAccess _coreCasinoBetSettingDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _coreCasinoBetSettingDataAccess = Substitute.For<ICoreCasinoBetSettingDataAccess>();
            DataAccess = Substitute.ForPartsOf<RngCasinoBetSettingDataAccess>(_coreCasinoBetSettingDataAccess);
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
            DataAccess.UpdateStatus(metaModel);

            // Assert
            _coreCasinoBetSettingDataAccess.Received().UpdateCasinoStatus(metaModel, RngCasinoSportId);
        }
    }
}