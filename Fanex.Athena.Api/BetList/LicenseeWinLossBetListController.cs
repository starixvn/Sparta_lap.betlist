// --------------------------------------------------------------------------
// <copyright file="LicenseeWinLossBetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;

    public class LicenseeWinLossBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic LicenseeWL(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 &&
                !currencyList.Values.Contains(metaModel.CurrencyId) &&
                (licenseeId == Licensees.KeenOcean && metaModel.CurrencyId != CurrencyType.Cny))
            {
                return null;
            }

            metaModel.LicenseeId = licenseeId;
            metaModel.UserId = userSession.UserId;

            const string UspKey = "Lrf_BetList_SelWinloss_Mem";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}