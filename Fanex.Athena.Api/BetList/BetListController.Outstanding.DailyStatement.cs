// --------------------------------------------------------------------------
// <copyright file="BetListController.Outstanding.DailyStatement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for daily statement bet list.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        [HttpPost]
        public dynamic OutstandingDailyStatementBetList(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;

            metaModel.BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay;
            metaModel.MoveBetsDay = _balanceUpService.BalanceUpInfo.MoveBetsDay;
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);

            var isAllowCustComm = SessionService.LicenseeSession.IsAllowedCustomerComm;

            return new
            {
                Main = _betListService.GetDailyStatementData(metaModel, isAllowCustComm),
                Total = new
                {
                    SumWinLoss = 0,
                    SumLicenseeWl = 0
                }
            };
        }
    }
}