namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Web.Mvc;
    using Fanex.Athena.Service.BetSettings.Interfaces;
    using Model.BetSettings.MetaModel;
    using Service.BetSettings;

    public class BetradarVsBetSettingController : BetSettingBaseController
    {
        private readonly IBetradarVsBetSettingService _betSettingService;

        public BetradarVsBetSettingController()
        {
            _betSettingService = new BetradarVsBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId);
            AssignRequiredProperties(viewModel, metaModel);
            return View("~/Views/Customer/BetSetting/BetradarVirtualSports.cshtml", viewModel);
        }
    }
}