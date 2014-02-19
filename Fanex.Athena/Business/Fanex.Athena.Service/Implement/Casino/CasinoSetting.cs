// -----------------------------------------------------------------------
// <copyright file="CasinoSetting.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Casino
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using CasinoLibrary.SBAgent;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.RngCasino;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Service.Interface.Casino;
    using Fanex.Common;
    using Fanex.Logging;
    using CustomerCasinoSetting = CasinoLibrary.SBAgent.Customer;

    #endregion Using

    /// <summary>
    /// Casino setting class.
    /// </summary>
    public class CasinoSetting : ICasinoSetting
    {
        #region Variables

        #endregion Variables

        #region ICasinoSetting Members

        /// <summary>
        /// Suspend customer.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>True if suspending is successful.</returns>
        public bool UpdateCustSuspend(CasinoStatusMetaModel metaModel)
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
                                                        metaModel.SystemId);
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error when call casino api UpdateCustSuspend. Message: " + e.Message, e);
            }
        }

        /// <summary>
        /// Close customer.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>True if processing is successful.</returns>
        public bool UpdateCustClosed(CasinoStatusMetaModel metaModel)
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
                                                        metaModel.SystemId);
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error when call casino api UpdateCustClosed. Message: " + e.Message, e);
            }
        }

        #endregion ICasinoSetting Members

        /// <summary>
        /// Get RNG casino setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="licenseeName">The licensee name.</param>
        /// <returns>RNG casino setting.</returns>
        public RngCasinoBetSetting GetRngCasinoSetting(int custId, string licenseeName)
        {
            var gameKindList = new List<GameKind>();

            try
            {
                gameKindList = this.GetGameKind();

                foreach (var gameKind in gameKindList)
                {
                    // Get game type base on game kind
                    var gameTypeList = this.GetGameType(gameKind.GameKindId);
                    gameKind.GameTypeList = gameTypeList;

                    foreach (var gameType in gameTypeList)
                    {
                        // Get stake level base on game type
                        var stakeLevelList = this.GetCustStakeLevel(custId, gameType.GameTypeId);
                        gameType.StakeLevelList = stakeLevelList;
                    }
                }
            }
            catch (Exception exception)
            {
                gameKindList = new List<GameKind>();
                LogService.Error(exception.Message, exception);
            }            

            return new RngCasinoBetSetting
            {
                Data = gameKindList
            };
        }

        /// <summary>
        /// Update RNG casino setting.
        /// </summary>
        /// <param name="dataTableParam">The parameters.</param>
        /// <param name="licenseeName">The licensee name.</param>
        /// <returns>True if updating is successful.</returns>
        public bool UpdateRngCasinoSetting(DataTable dataTableParam, string licenseeName)
        {
            try
            {
                var casinoCustomerBo = new CustomerCasinoSetting();
                var resultTable = casinoCustomerBo.UpdateCustGameTypeStakeLevel(dataTableParam);
                return resultTable.Rows.Count == 0;
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error when call casino api UpdateCustSuspend. Message: " + e.Message, e);
            }
        }

        #region Get Game Kind

        /// <summary>
        /// Get RNG casino game kind.
        /// </summary>
        /// <returns>The game kind list.</returns>
        private List<GameKind> GetGameKind()
        {
            var bacsicInfo = new BasicInfo();
            DataTable gameKindTable = bacsicInfo.GetGameKind("01");

            var gameKindList = new List<GameKind>();

            foreach (DataRow row in gameKindTable.Rows)
            {
                var gameKind = new GameKind
                {
                    GameKindId = Converter.ToInt(row["CodeId"].ToString()),
                    GameKindName = row["CodeName_En"].ToString()
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
        /// <returns>The game type list. </returns>
        private IList<GameType> GetGameType(int gameKindId)
        {
            var bacsicInfo = new BasicInfo();
            DataTable gameTypeTable = bacsicInfo.GetGameType(gameKindId);

            var gameTypeList = new List<GameType>();

            foreach (DataRow row in gameTypeTable.Rows)
            {
                var gameType = new GameType
                {
                    GameTypeId = Converter.ToInt(row["GameTypeId"].ToString()),
                    GameTypeName = row["GameTypeName_En"].ToString()
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
        private List<StakeLevel> GetCustStakeLevel(int custId, int gameTypeId)
        {
            var customer = new CustomerCasinoSetting();
            DataTable stakeLevelTable = customer.GetCustStakeLevel(custId, gameTypeId);

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