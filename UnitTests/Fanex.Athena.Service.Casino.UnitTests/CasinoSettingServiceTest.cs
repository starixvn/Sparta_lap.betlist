namespace Fanex.Athena.Service.Casino.UnitTests
{
    using System;
    using System.Data;
    using Casino;
    using Casino.Wrappers;
    using Core.Exception;
    using Core.Utility.Interfaces;
    using Fanex.Athena.Model.Casino.MetaModel.ApiInputParameter;
    using Model.BetSettings.DTO.RngCasino;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class CasinoSettingServiceTest
    {
        private const string CasinoNoDeposit = "01";

        private ICustomerApiWrapper _customerApiWrapper;

        private IBasicInfoApiWrapper _basicInfoApiWrapper;

        private ICoreLogger _logger;

        private CasinoSettingService _casinoSettingService;

        [SetUp]
        public void SetUp()
        {
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();
            _basicInfoApiWrapper = Substitute.For<IBasicInfoApiWrapper>();
            _logger = Substitute.For<ICoreLogger>();

            _casinoSettingService = new CasinoSettingService(
                _logger,
                _customerApiWrapper,
                _basicInfoApiWrapper);
        }

        [TearDown]
        public void TearDown()
        {
            _customerApiWrapper = null;
        }

        [Test]
        public void GetRngCasinoSettings_GetGameKind_ReturnCorrectData()
        {
            // Arrange
            var custId = FakeHelper.Create<int>();
            var currencyId = FakeHelper.Create<int>();
            const string Language = "En";

            var gameKind = SetUpForGetGameKind();
            var gameType = SetUpForGetGameType(gameKind.GameKindId, custId, currencyId);
            SetUpForGetCustStakeLevel(custId, gameType.GameTypeId);

            // Action
            var result = _casinoSettingService.GetRngCasinoSettings(custId, currencyId, Language);

            // Assert
            Assert.That(result[0].GameKindId, Is.EqualTo(gameKind.GameKindId));
        }

        [Test]
        public void GetRngCasinoSettings_GetGameType_ReturnCorrectData()
        {
            // Arrange
            var custId = FakeHelper.Create<int>();
            var currencyId = FakeHelper.Create<int>();
            const string Language = "En";

            var gameKind = SetUpForGetGameKind();
            var gameType = SetUpForGetGameType(gameKind.GameKindId, custId, currencyId);
            SetUpForGetCustStakeLevel(custId, gameType.GameTypeId);

            // Action
            var result = _casinoSettingService.GetRngCasinoSettings(custId, currencyId, Language);

            // Assert
            Assert.That(result[0].GameTypeList[0].GameTypeId, Is.EqualTo(gameType.GameTypeId));
        }

        [Test]
        public void GetRngCasinoSettings_GetCustStakeLevel_ReturnCorrectData()
        {
            // Arrange
            var custId = FakeHelper.Create<int>();
            var currencyId = FakeHelper.Create<int>();
            const string Language = "En";

            var gameKind = SetUpForGetGameKind();
            var gameType = SetUpForGetGameType(gameKind.GameKindId, custId, currencyId);
            var stakeLevel = SetUpForGetCustStakeLevel(custId, gameType.GameTypeId);

            // Action
            var result = _casinoSettingService.GetRngCasinoSettings(custId, currencyId, Language);

            // Assert
            Assert.That(result[0].GameTypeList[0].StakeLevelList[0].StakeLevelId, Is.EqualTo(stakeLevel.StakeLevelId));
        }

        [Test]
        public void GetRngCasinoSettings_GetErrorWhileProcess_ReturnEmptyDataAndWriteErrorLog()
        {
            // Arrange
            var custId = FakeHelper.Create<int>();
            var currencyId = FakeHelper.Create<int>();
            const string Language = "En";

            // Action
            var result = _casinoSettingService.GetRngCasinoSettings(custId, currencyId, Language);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Empty);
                _logger.Received().Error(Arg.Any<Exception>());
            });
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void UpdateSetting_WhenCall_ReturnCorrectData(bool hasRows, bool expectedResult)
        {
            // Arrange
            var param = new DataTable();

            var table = new DataTable();
            table.Columns.Add("test", typeof(int));
            if (hasRows)
            {
                table.Rows.Add(0);
            }

            _customerApiWrapper.UpdateCustGameTypeStakeLevel(param).ReturnsForAnyArgs(table);

            // Action
            var result = _casinoSettingService.UpdateSetting(param);

            // Assert
            Assert.Multiple(() =>
            {
                _customerApiWrapper.ReceivedWithAnyArgs().UpdateCustGameTypeStakeLevel(param);
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

        private GameKind SetUpForGetGameKind()
        {
            var data = new GameKind
            {
                GameKindId = FakeHelper.Create<int>(),
                GameKindName = FakeHelper.Create<string>()
            };

            var gameKindTable = new DataTable();
            gameKindTable.Columns.Add("CodeId", typeof(int));
            gameKindTable.Columns.Add("CodeName_En", typeof(string));
            gameKindTable.Rows.Add(data.GameKindId, data.GameKindName);

            _basicInfoApiWrapper.GetGameKind(CasinoNoDeposit).Returns(gameKindTable);

            return data;
        }

        private GameType SetUpForGetGameType(int gameKind, int custId, int currency)
        {
            var data = new GameType
            {
                GameTypeId = FakeHelper.Create<int>(),
                GameTypeName = FakeHelper.Create<string>()
            };

            var gameTypeTable = new DataTable();
            gameTypeTable.Columns.Add("GameTypeId", typeof(int));
            gameTypeTable.Columns.Add("GameTypeName_En", typeof(string));
            gameTypeTable.Rows.Add(data.GameTypeId, data.GameTypeName);

            _basicInfoApiWrapper.GetGameType(
                Arg.Is<GetGameTypeMetalModel>(x => x.GameKind == gameKind && x.SbCustID == custId && x.SbExchangeID == currency))
            .Returns(gameTypeTable);

            return data;
        }

        private StakeLevel SetUpForGetCustStakeLevel(int custId, int gameTypeId)
        {
            var data = new StakeLevel
            {
                StakeLevelId = FakeHelper.Create<int>(),
                StakeLevelName = FakeHelper.Create<string>(),
                IsCheck = FakeHelper.Create<bool>()
            };

            var stakeLevelTable = new DataTable();
            stakeLevelTable.Columns.Add("StakeLevelID", typeof(int));
            stakeLevelTable.Columns.Add("StakeLevelName", typeof(string));
            stakeLevelTable.Columns.Add("IsChecked", typeof(bool));
            stakeLevelTable.Rows.Add(data.StakeLevelId, data.StakeLevelName, data.IsCheck);

            _customerApiWrapper.GetCustStakeLevel(custId, gameTypeId).Returns(stakeLevelTable);

            return data;
        }
    }
}