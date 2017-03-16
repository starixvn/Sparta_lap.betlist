// --------------------------------------------------------------------------
// <copyright file="RunningBettypesBetListController.cs" company="">
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
    public class RunningBettypesBetListController : ApiBetListBaseController
    {
        private const string DoubleChangeBettypes = "24,151,186,431,410";

        private const string BONTeamToScoreBettypes = "26,145,146,433,170,188,427";

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetMixParlay)]
        public dynamic MixParlay(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = ((int)BetType.MixParlay).ToString();
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetMixParlay)]
        public dynamic SystemParlay(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = ((int)BetType.CombinationMixParlay).ToString();
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetDoubleChance)]
        public dynamic DoubleChange(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = DoubleChangeBettypes;
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetBONTeamToScore)]
        public dynamic BONTeamToScore(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = BONTeamToScoreBettypes;
            return ProcessRequest(metaModel);
        }

        private dynamic ProcessRequest(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            const string UspKey = "Lrf_BetList_SelRunningByBettypeList";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}