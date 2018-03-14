// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ActionFilter;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    #endregion Using

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.ReportLastBetsMonitoring)]
    public class LastBetsMonitoringBetListController : BaseController
    {
        private readonly ISportTypeDataAccess _sportTypeDataAccess;

        public LastBetsMonitoringBetListController()
        {
            _sportTypeDataAccess = new SportTypeDataAccess();
        }

        public LastBetsMonitoringBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            ISportTypeDataAccess sportTypeDataAccess)
        : base(
            languageService,
            sessionService,
            appSetting,
            clientViewHelper)
        {
            _sportTypeDataAccess = sportTypeDataAccess;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = new BetListMetaModel { LicenseeId = SessionService.LicenseeSession.LicenseeId };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new LastBetsMonitoringViewModel
            {
                CurrencyList = SessionService.UserSession.CurrencyList,
                SportsList = GetSports(),
                Param = metaModel
            };

            return View("~/Views/BetList/LastBetsMonitoringNew/Main.cshtml", model);
        }

        private IEnumerable<SelectListItem> GetSports()
        {
            const string ParlayId = "999";
            var sports = this._sportTypeDataAccess.GetAllSports();
            var sportListItems = sports.Select(item => new SelectListItem { Value = item.Key.ToString(), Text = item.Value }).ToList();
            sportListItems.Insert(0, new SelectListItem { Value = "0", Text = ViewResources.All, Selected = true });
            sportListItems.Add(new SelectListItem { Value = ParlayId, Text = ViewResources.Parlay });

            return sportListItems;
        }
    }
}