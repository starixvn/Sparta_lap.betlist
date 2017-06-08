// --------------------------------------------------------------------------
// <copyright file="RunningMatchBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;

    #endregion Using

    public class RunningMatchBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            string uspKey = IsRacing(metaModel.BetType)
                ? "Lrf_BetList_SelAllByDateHR"
                : "Lrf_BetList_SelRunningByBettypeMatch";
            return GetBetListResult(metaModel, uspKey);
        }
    }
}