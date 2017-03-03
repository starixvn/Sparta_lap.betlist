// --------------------------------------------------------------------------
// <copyright file="RunningBetListController.cs" company="">
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

    /// <summary>
    /// RunningBetList contains bet list:
    /// Mix Parlay              - bet type: 9
    /// Clean Sheet             - bet type: 13
    /// Double Chance           - bet type: 24
    /// B/O/N team to score     - bet type: 26
    /// To win to nil           - bet type: 27
    /// 3-Way Handicap          - bet type: 28.
    /// </summary>
    public class RunningBetListController : ApiBetListBaseController
    {
        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetCleanSheet)]
        public dynamic CleanSheet(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.CleanSheet;
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetToWinToNil)]
        public dynamic ToWinToNil(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.ToWinToNil;
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBet3WayHandicap)]
        public dynamic ThreeWayHandicap(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.ThreeWayHandicap;
            return ProcessRequest(metaModel);
        }

        private dynamic ProcessRequest(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            const string UspKey = "Lrf_BetList_SelRunningByBettype";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}