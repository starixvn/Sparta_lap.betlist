// --------------------------------------------------------------------------
// <copyright file="CustomerWinLossBetListController.cs" company="">
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

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            const string UspKey = "Lrf_BetList_SelWinLossByDate_Mem";

            return GetBetListResult(metaModel, UspKey);
        }
    }
}