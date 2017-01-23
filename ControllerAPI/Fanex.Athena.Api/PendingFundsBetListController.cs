// --------------------------------------------------------------------------
// <copyright file="PendingFundsBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.DataDomain;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerPendingFunds)]
    public class PendingFundsBetListController : ApiBetListBaseController
    {
        [HttpPost]
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