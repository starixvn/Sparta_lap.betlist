namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
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
            base.FixtureSetUp();
        }

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<RngCasinoBetSettingDataAccess>(
                _coreCasinoBetSettingDataAccess);
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_CallCoreCasinoBetSettingDataAccess()
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
            await DataAccess.UpdateStatusAsync(metaModel);

            // Assert
            _coreCasinoBetSettingDataAccess.Received().UpdateCasinoStatusAsync(metaModel, RngCasinoSportId);
        }
    }
}