// --------------------------------------------------------------------------
// <copyright file="RunningMatchBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RunningMatchBetListController : ApiBetListBaseController
    {
        /// <summary>
        /// Get data for running bet list of a match.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            string uspKey = IsRacing(metaModel.BetType)
                ? "Lrf_BetList_SelAllByDateHR"
                : "Lrf_BetList_SelRunningByBettypeMatch";
            return GetBetListResult(metaModel, uspKey);
        }
    }
}