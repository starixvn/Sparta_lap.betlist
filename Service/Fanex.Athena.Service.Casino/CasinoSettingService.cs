// -----------------------------------------------------------------------
// <copyright file="CasinoSetting.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Casino
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Extension.Helper;
    using Fanex.Athena.Model.BetSettings.DTO.RngCasino;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Service.Core.Exception;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Common;
    using Model.Customer.MetaModel;
    using Wrappers;

    public class CasinoSettingService : ICasinoSettingService
    {
        private readonly ICustomerApiWrapper _customerApiWrapper;

        private readonly IBasicInfoApiWrapper _basicInfoApiWrapper;

        public CasinoSettingService() : this(null, null)
        {
        }

        public CasinoSettingService(ICustomerApiWrapper customerApiWrapper, IBasicInfoApiWrapper basicInfoApiWrapper)
        {
            _customerApiWrapper = customerApiWrapper ?? new CustomerApiWrapper();
            _basicInfoApiWrapper = basicInfoApiWrapper ?? new BasicInfoApiWrapper();
        }

        public bool UpdateCustSuspend(CustomerStatusMetaModel metaModel)
        {
            try
            {
                return _customerApiWrapper.UpdateCustSuspend(
                            metaModel.CustId,
                            metaModel.IsOn,
                            metaModel.Ip,
                            metaModel.ModifyById,
                            metaModel.ModifyByUsername);
            }
            catch (Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustSuspend. Message: " + e.Message, "casino api service");
            }
        }

        public bool UpdateCustClosed(CustomerStatusMetaModel metaModel)
        {
            try
            {
                return _customerApiWrapper.UpdateCustClosed(
                            metaModel.CustId,
                            metaModel.IsOn,
                            metaModel.Ip,
                            metaModel.ModifyById,
                            metaModel.ModifyByUsername);
            }
            catch (Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustClosed. Message: " + e.Message, "Casino api service");
            }
        }

        public IList<GameKind> GetRngCasinoSettings(int custId, string language)
        {
            IList<GameKind> gameKinds;

            try
            {
                gameKinds = GetGameKind(language);

                foreach (var gameKind in gameKinds)
                {
                    // Get game type base on game kind
                    var gameTypeList = GetGameType(gameKind.GameKindId, language);
                    gameKind.GameTypeList = gameTypeList;

                    foreach (var gameType in gameTypeList)
                    {
                        // Get stake level base on game type
                        var stakeLevelList = GetCustStakeLevel(custId, gameType.GameTypeId);
                        gameType.StakeLevelList = stakeLevelList;
                    }
                }
            }
            catch (Exception exception)
            {
                gameKinds = new List<GameKind>();
                LogHelper.WriteError(exception);
            }

            return gameKinds;
        }

        public bool UpdateSetting(DataTable dataTableParam)
        {
            try
            {
                var resultTable = _customerApiWrapper.UpdateCustGameTypeStakeLevel(dataTableParam);
                return resultTable.Rows.Count == 0;
            }
            catch (Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustSuspend. Message: " + e.Message, "casino api");
            }
        }

        private IList<GameKind> GetGameKind(string language)
        {
            var gameKindNameByLanguage = CasinoHelper.GetColumnNameByLanguage("CodeName", language);
            var gameKindTable = _basicInfoApiWrapper.GetGameKind("01");

            var gameKindList = new List<GameKind>();

            foreach (DataRow row in gameKindTable.Rows)
            {
                var gameKind = new GameKind
                {
                    GameKindId = Converter.ToInt(row["CodeId"].ToString()),
                    GameKindName = row[gameKindNameByLanguage].ToString()
                };

                gameKindList.Add(gameKind);
            }

            return gameKindList;
        }

        private IList<GameType> GetGameType(int gameKindId, string language)
        {
            var gameTypeName = CasinoHelper.GetColumnNameByLanguage("GameTypeName", language);
            var gameTypeTable = _basicInfoApiWrapper.GetGameType(gameKindId);

            var gameTypeList = new List<GameType>();

            foreach (DataRow row in gameTypeTable.Rows)
            {
                var gameType = new GameType
                {
                    GameTypeId = Converter.ToInt(row["GameTypeId"].ToString()),
                    GameTypeName = row[gameTypeName].ToString()
                };

                gameTypeList.Add(gameType);
            }

            return gameTypeList.OrderBy(m => m.GameTypeName).ToList();
        }

        private IList<StakeLevel> GetCustStakeLevel(int custId, int gameTypeId)
        {
            var stakeLevelTable = _customerApiWrapper.GetCustStakeLevel(custId, gameTypeId);

            var stakeLevelList = new List<StakeLevel>();

            foreach (DataRow row in stakeLevelTable.Rows)
            {
                var stakeLevel = new StakeLevel
                {
                    StakeLevelId = Converter.ToInt(row["StakeLevelID"].ToString()),
                    StakeLevelName = row["StakeLevelName"].ToString(),
                    IsCheck = Converter.ToBool(row["IsChecked"])
                };

                stakeLevelList.Add(stakeLevel);
            }

            return stakeLevelList;
        }
    }
}