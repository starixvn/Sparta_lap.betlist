// --------------------------------------------------------------------------
// <copyright file="OutStandingCompletedBetListController.cs" company="">
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
    public class OutStandingCompletedBetListController : ApiBetListBaseController
    {
        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";

        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            if (!metaModel.IsValid ||
                (metaModel.CurrencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId)))
            {
                return null;
            }

            metaModel.ProductCodeList = RemoveInvalidProducts(metaModel.ProductCodeList, BetListCustomerOutstandingName);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            const string UspKey = "Lrf_BetList_SelAllByDate_Mem";
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);    // Need -2 miliseconds, it better than -1.
            var result = GetBetListResult(metaModel, UspKey);
            return new
            {
                result.Main,
                result.LeagueName,
                result.Total,
                IsShowTotal = true
            };
        }
    }
}