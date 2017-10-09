// --------------------------------------------------------------------------
// <copyright file="OutstandingDailyStatementBetListController.cs" company="">
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
    public class OutstandingDailyStatementBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUp = GetBalanceUp();
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);

            return new
            {
                Main = BetListService.GetDailyStatementData(metaModel, licenseeSession.IsAllowedCustomerComm),
                Total = new
                {
                    SumWinLoss = 0,
                    SumLicenseeWl = 0
                }
            };
        }
    }
}