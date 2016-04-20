// -----------------------------------------------------------------------
// <copyright file="BetSettingController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Web.Mvc;
    using Fanex.Athena.Models.MetaModel.BetSettings;
    using Fanex.Athena.Web.ActionFilter;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class BetSettingController : BetSettingBaseController
    {
        private readonly IBetSettingService _betSettingService;

        public BetSettingController()
        {
            this._betSettingService = new BetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetSportBookViewModel(metaModel.CustId);

            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/Main.cshtml", viewModel);
        }
    }
}