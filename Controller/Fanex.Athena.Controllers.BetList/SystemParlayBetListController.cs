// -----------------------------------------------------------------------
// <copyright file="SystemParlayBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class SystemParlayBetListController : BetListBaseController
    {
        private readonly ITicketHelper _ticketHelper;
        private readonly IBetListHTMLBuilder _betListBuilder;

#pragma warning disable S107 // Methods should not have too many parameters
        public SystemParlayBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            ITicketHelper ticketHelper,
            IBetListHTMLBuilder betListBuilder)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
            _ticketHelper = ticketHelper;
            _betListBuilder = betListBuilder;
        }

        [HttpGet]
        public ActionResult Detail(SystemParlayDetailMetaModel metaModel)
        {
            var balanceUp = GetBalanceUp();
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var systemParlayData = this.BetListService.GetSystemParlayDetail(metaModel);

            var systemParlayText = _betListBuilder.BuildSystemParlayDetail(this._ticketHelper, systemParlayData);
            var viewModel = new SystemParlayDetailViewModel
            {
                DetailText = systemParlayText
            };

            return View("~/Views/BetList/SystemParlayDetail/Main.cshtml", viewModel);
        }
    }
}