// --------------------------------------------------------------------------
// <copyright file="CorrectScoreBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.DataDomain;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.TotalBetCorrectScore)]
    public class CorrectScoreBetListController : ApiBetListBaseController
    {
        /// <summary>
        /// Get data for correct score bet list.
        /// [Used in TOTAL BETS].
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>Return bet list result list.</returns>
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            const string UspKey = "Lrf_BetList_CorrectScore";

            return GetBetListResult(metaModel, UspKey);
        }
    }
}