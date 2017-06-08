// --------------------------------------------------------------------------
// <copyright file="DailyStatementBetListController.cs" company="">
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

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class DailyStatementBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            const string UspKey = "Lrf_BetList_SelByDayAll_Mem";
            const string UspKeyOld = "Lrf_BetList_SelByDayAll_Mem_Old";

            var balanceUpInfo = BalanceUpService.BalanceUpInfo;
            var uspKey = metaModel.FromDate < balanceUpInfo.BalanceUpDay ? UspKeyOld : UspKey;

            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);

            const int RunningTicketTransType = 3;
            var result = GetBetListResult(metaModel, uspKey);

            return new
            {
                result.Main,
                result.Total,
                result.LeagueName,
                IsShowTotal = metaModel.TransType != RunningTicketTransType
            };
        }
    }
}