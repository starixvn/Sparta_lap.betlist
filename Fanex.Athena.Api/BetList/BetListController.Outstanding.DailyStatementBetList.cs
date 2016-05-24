// --------------------------------------------------------------------------
// <copyright file="BetListController.Outstanding.DailyStatementBetList.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Athena.Model.BetList.MetaModel;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for daily statement.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        [HttpPost]
        public dynamic BetListDailyStatement(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            const string UspKey = "Lrf_BetList_SelByDayAll_Mem";
            const string UspKeyOld = "Lrf_BetList_SelByDayAll_Mem_Old";

            var balanceUpInfo = BalanceUpService.BalanceUpInfo;
            var uspKey = metaModel.FromDate < balanceUpInfo.BalanceUpDay ? UspKeyOld : UspKey;

            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);

            return GetBetListResult(metaModel, uspKey);
        }
    }
}