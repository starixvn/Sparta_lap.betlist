// --------------------------------------------------------------------------
// <copyright file="MicroGamingRngBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;

    #endregion Using

    public class MicroGamingRngBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Detail(BetListMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            const string UspKey = "Lrf_BetList_RNG_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}