// --------------------------------------------------------------------------
// <copyright file="PendingFundsBetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Web.ActionFilter;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PendingFundsBetListController : ApiBetListBaseController
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

            metaModel.FromDate = DateTime.Today.AddYears(-1);
            metaModel.ToDate = DateTime.Today.AddYears(1);

            return GetBetListResult(metaModel, UspKey);
        }
    }
}