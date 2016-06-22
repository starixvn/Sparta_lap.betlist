// --------------------------------------------------------------------------
// <copyright file="DailyRacingWinLossBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.DataDomain;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportDailyRacingWinLoss)]
    public class DailyRacingWinLossBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            const string UspKey = "Lrf_BetList_SelWinlostRacing";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}