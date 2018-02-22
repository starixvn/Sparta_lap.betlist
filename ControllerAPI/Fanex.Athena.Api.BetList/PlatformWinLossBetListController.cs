// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.BetList.Core.Entities;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using VendorAdapter.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportPlatformWinLoss)]
    public class PlatformWinLossBetListController : ApiBetListBaseController
    {
        public PlatformWinLossBetListController()
        {
        }

        [CLSCompliant(false)]
        public PlatformWinLossBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter)
        {
        }

        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            if (!KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(SessionService.UserSession.CurrencyList, licenseeId, metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            const string UspKey = "Lrf_BetList_PlatformWinLoss_Mem";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}