// -----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ResultExtension;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        [HttpGet]
        public ActionResult AGCasinoDetail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                ViewBag.BetListTitle = GetBetListTitleForAGCasino(metaModel.TransType);
                return this.View("~/Views/BetList/AGCasinoDetail/Main.cshtml", metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        private string GetBetListTitleForAGCasino(long transType)
        {
            string title = ViewResources.AGCasino + " - " + ViewResources.BetList + " - " + 
                ViewResources.DetailTickets + " - ";
            switch (transType)
            {
                case MicroGamingRngTransTypeYesterday:
                    title += ViewResources.Yesterday;
                    break;

                case MicroGamingRngTransTypeTodayRunning:
                    title += ViewResources.Today + " - " + ViewResources.Running;
                    break;

                default:
                    title += ViewResources.Today + " - " + ViewResources.Settled;
                    break;
            }

            return title;
        }
    }
}
