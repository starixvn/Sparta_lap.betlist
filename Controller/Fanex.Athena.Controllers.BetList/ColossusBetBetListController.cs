// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI;
    using Athena.BetList.Model.ViewModel;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportColossusBetsWinLoss)]
    public class ColossusBetBetListController : BetListBaseController
    {
#pragma warning disable S107 // Methods should not have too many parameters

        public ColossusBetBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            ICoreLogger logger,
            IBetListApiClient betListApiClient)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter, betListApiClient)
        {
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public async Task<ActionResult> Detail(ColossusBetDetailMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            metaModel.UserId = SessionService.UserSession.UserId;

            var viewModel = await BetListApiClient.PostAsync<ColossusBetDetailViewModel>("/ColossusBetBetList/Detail", metaModel).ConfigureAwait(false);
            return this.View("~/Views/Desktop/BetList/ColossusBetDetail/Main.cshtml", viewModel);
        }
    }
}