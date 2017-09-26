// -----------------------------------------------------------------------
// <copyright file="CasinoSetting.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Casino
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using CasinoLibrary.SBAgent;
    using Extension.Helper;
    using Fanex.Athena.Model.BetSettings.DTO.RngCasino;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Service.Core.Exception;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Common;
    using Model.Customer.MetaModel;
    using CustomerCasinoSetting = CasinoLibrary.SBAgent.Customer;

    #endregion Using

    /// <summary>
    /// Casino setting class.
    /// </summary>
    public class CasinoSetting : ICasinoSetting
    {
        public const int SystemId = 4;

        #region ICasinoSetting Members

        /// <summary>
        /// Suspend customer.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>True if suspending is successful.</returns>
        public bool UpdateCustSuspend(CustomerStatusMetaModel metaModel)
        {
            try
            {
                var casinoCustomerBo = new CustomerCasinoSetting();

                return casinoCustomerBo.UpdateCustSuspend(
                    metaModel.CustId,
                                                        metaModel.IsOn,
                                                        metaModel.Ip,
                                                        metaModel.ModifyById,
                                                        metaModel.ModifyByUsername,
                                                        SystemId);
            }
            catch (System.Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustSuspend. Message: " + e.Message, "casino api service");
            }
        }

        /// <summary>
        /// Close customer.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>True if processing is successful.</returns>
        public bool UpdateCustClosed(CustomerStatusMetaModel metaModel)
        {
            try
            {
                var casinoCustomerBo = new CustomerCasinoSetting();

                return casinoCustomerBo.UpdateCustClosed(
                    metaModel.CustId,
                                                        metaModel.IsOn,
                                                        metaModel.Ip,
                                                        metaModel.ModifyById,
                                                        metaModel.ModifyByUsername,
                                                        SystemId);
            }
            catch (System.Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustClosed. Message: " + e.Message, "Casino api service");
            }
        }

        #endregion ICasinoSetting Members

        /// <summary>
        /// Get RNG casino setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="licenseeName">The licensee name.</param>
        /// <param name="language">Language of licensee.</param>
        /// <returns>RNG casino setting.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<GameKind> GetRngCasinoSettings(int custId, string licenseeName, string language)
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

        /// <summary>
        /// Update RNG casino setting.
        /// </summary>
        /// <param name="dataTableParam">The parameters.</param>
        /// <param name="licenseeName">The licensee name.</param>
        /// <returns>True if updating is successful.</returns>
        public bool UpdateSetting(DataTable dataTableParam, string licenseeName)
        {
            try
            {
                var casinoCustomerBo = new CustomerCasinoSetting();
                var resultTable = casinoCustomerBo.UpdateCustGameTypeStakeLevel(dataTableParam);
                return resultTable.Rows.Count == 0;
            }
            catch (System.Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustSuspend. Message: " + e.Message, "casino api");
            }
        }

        #region Get Game Kind

        /// <summary>
        /// Get RNG casino game kind.
        /// </summary>
        /// <param name="language">Language of name kind.</param>
        /// <returns>The game kind list.</returns>
        private static IList<GameKind> GetGameKind(string language)
        {
            var bacsicInfo = new BasicInfo();
            var gameKindNameByLanguage = CasinoHelper.GetColumnNameByLanguage("CodeName", language);
            var gameKindTable = bacsicInfo.GetGameKind("01");

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

        #endregion Get Game Kind

        #region Get Game Type

        /// <summary>
        /// Get game types.
        /// </summary>
        /// <param name="gameKindId">The game kind id.</param>
        /// <param name="language">Language of licensee.</param>
        /// <returns>The game type list. </returns>
        private static IList<GameType> GetGameType(int gameKindId, string language)
        {
            var bacsicInfo = new BasicInfo();
            var gameTypeName = CasinoHelper.GetColumnNameByLanguage("GameTypeName", language);
            var gameTypeTable = bacsicInfo.GetGameType(gameKindId);

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

        #endregion Get Game Type

        #region Get Cust Stake Level

        /// <summary>
        /// Get stake level of customer.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="gameTypeId">The game type id.</param>
        /// <returns>The stack level list.</returns>
        private static IList<StakeLevel> GetCustStakeLevel(int custId, int gameTypeId)
        {
            var customer = new CustomerCasinoSetting();
            var stakeLevelTable = customer.GetCustStakeLevel(custId, gameTypeId);

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

        #endregion Get Cust Stake Level
    }
}