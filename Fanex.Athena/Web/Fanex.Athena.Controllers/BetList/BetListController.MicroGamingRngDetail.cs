// -----------------------------------------------------------------------
// <copyright file="BetListController.MicroGamingRngDetail.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        [HttpGet]
        public ActionResult MicroGamingRngDetail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                return this.View("~/Views/BetList/MicroGamingRngDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return this.LicenseeWLExcel(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }
    }
}