// --------------------------------------------------------------------------
// <copyright file="OutstandingRunningBBetListController.cs" company="Nexcel Solutions Vietnam">
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
    using Web.ActionFilter;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutstandingRunningBetListController : ApiBetListBaseController
    {
        [HttpPost]
        [ApiFunctionAuthorization]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);
            return GetBetListResult(metaModel, UspKey);
        }
    }
}