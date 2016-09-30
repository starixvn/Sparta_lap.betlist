// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.ColossusBets.Builder;
    using Model.BetList.ViewModel;
    using Service.BetList;
    using Service.BetList.Interfaces;

    #endregion Using

    public class ColossusBetBetListController : BetListBaseController
    {
        private readonly IColossusBetBetListService _colossusBetBetListService;

        public ColossusBetBetListController()
        {
            _colossusBetBetListService = new ColossusBetBetListService();
        }

        [HttpGet]
        public ActionResult Detail(ColossusBetDetailMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var viewModel = _colossusBetBetListService.GetData(metaModel);
            
            return this.View("~/Views/BetList/ColossusBetDetail/Main.cshtml", viewModel);
        }
    }
}