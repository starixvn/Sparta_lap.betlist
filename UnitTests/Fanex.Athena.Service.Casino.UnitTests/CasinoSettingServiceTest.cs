namespace Fanex.Athena.Service.Casino.UnitTests
{
    using System;
    using System.Data;
    using Casino;
    using Casino.Wrappers;
    using Core.Exception;
    using Fanex.Athena.Model.Casino.MetaModel.ApiInputParameter;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoSettingServiceTest
    {
        private ICustomerApiWrapper _customerApiWrapper;

        private IBasicInfoApiWrapper _basicInfoApiWrapper;

        private CasinoSettingService _casinoSettingService;

        [SetUp]
        public void SetUp()
        {
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();
            _basicInfoApiWrapper = Substitute.For<IBasicInfoApiWrapper>();

            _casinoSettingService = new CasinoSettingService(_customerApiWrapper, _basicInfoApiWrapper);
        }

        [TearDown]
        public void TearDown()
        {
            _customerApiWrapper = null;
        }

        [Test]
        public void GetRngCasinoSettings_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int CustId = 1;
            const int CurrencyId = 1;
            const string Language = "Hehe";
            const int GameKindId = 111;
            const int GameTypeId = 222;
            const int StakeLevelId = 333;
            MockServices(CustId, CurrencyId, GameKindId, GameTypeId, StakeLevelId);

            // Action
            var result = _casinoSettingService.GetRngCasinoSettings(CustId, CurrencyId, Language);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].GameKindId, Is.EqualTo(GameKindId));
                Assert.That(result[0].GameTypeList[0].GameTypeId, Is.EqualTo(GameTypeId));
                Assert.That(result[0].GameTypeList[0].StakeLevelList[0].StakeLevelId, Is.EqualTo(StakeLevelId));
            });
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void UpdateSetting_WhenCall_ReturnCorrectData(bool hasRows, bool expectedResult)
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("test", typeof(int));
            if (hasRows)
            {
                table.Rows.Add(0);
            }

            _customerApiWrapper.UpdateCustGameTypeStakeLevel(null).ReturnsForAnyArgs(table);

            // Action
            var result = _casinoSettingService.UpdateSetting(null);

            // Assert
            Assert.Multiple(() =>
            {
                _customerApiWrapper.ReceivedWithAnyArgs().UpdateCustGameTypeStakeLevel(null);
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public void UpdateSetting_ApiThrowException_ThrowServiceException()
        {
            // Arrange
            _customerApiWrapper.UpdateCustGameTypeStakeLevel(null).ReturnsForAnyArgs(x =>
            {
                throw new Exception();
            });

            // Action
            TestDelegate result = () => _casinoSettingService.UpdateSetting(null);

            // Assert
            Assert.Throws<ServiceException>(result);
        }

        private void MockServices(int custId, int currencyId, int gameKindId, int gameTypeId, int stakeLevelId)
        {
            var gameKindTable = new DataTable();
            gameKindTable.Columns.Add("CodeId", typeof(int));
            gameKindTable.Columns.Add("CodeName_En", typeof(string));
            gameKindTable.Rows.Add(gameKindId, string.Empty);
            _basicInfoApiWrapper.GetGameKind(null).ReturnsForAnyArgs(gameKindTable);

            var gameTypeTable = new DataTable();
            var gameTypeInput = new GetGameTypeMetalModel
            {
                GameKind = gameKindId,
                SbCustID = custId,
                SbExchangeID = currencyId
            };
            gameTypeTable.Columns.Add("GameTypeId", typeof(int));
            gameTypeTable.Columns.Add("GameTypeName_En", typeof(string));
            gameTypeTable.Rows.Add(gameTypeId, string.Empty);
            _basicInfoApiWrapper.GetGameType(gameTypeInput).ReturnsForAnyArgs(gameTypeTable);

            var stakeLevelTable = new DataTable();
            stakeLevelTable.Columns.Add("StakeLevelID", typeof(int));
            stakeLevelTable.Columns.Add("StakeLevelName", typeof(string));
            stakeLevelTable.Columns.Add("IsChecked", typeof(bool));
            stakeLevelTable.Rows.Add(stakeLevelId, string.Empty, false);
            _customerApiWrapper.GetCustStakeLevel(custId, gameTypeId).ReturnsForAnyArgs(stakeLevelTable);
        }
    }
}