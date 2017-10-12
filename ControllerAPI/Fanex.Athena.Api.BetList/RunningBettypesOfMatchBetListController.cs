// --------------------------------------------------------------------------
// <copyright file="RunningBettypesOfMatchBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
namespace Fanex.Athena.Api.BetList
{
    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;

    public class RunningBettypesOfMatchBetListController : ApiBetListBaseController
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

            return GetBetListResult(metaModel, "Lrf_BetList_SelRunningByMultipleBettypeMatch");
        }
    }
}