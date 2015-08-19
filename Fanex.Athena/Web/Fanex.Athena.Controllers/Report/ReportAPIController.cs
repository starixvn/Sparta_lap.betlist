// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportAPIController : DefaultCultureBaseController
    {
        private readonly IBetTypeService _betTypeService = new BetTypeService();
       
        [HttpPost]
        [AjaxRequest]
        public ActionResult GetBettypesBySport(IList<SportProductItem> sports)
        {


            if (sports == null || sports.Count == 0)
            {
                return Json(new List<SportProductItem>());
            }

            var data = _betTypeService.GetBettypes(SessionService.LicenseeSession.LicenseeId, sports, SessionService.UserSession.Language);


            return Json(data);
        }
    }
}
