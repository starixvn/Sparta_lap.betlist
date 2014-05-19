// -----------------------------------------------------------------------
// <copyright file="ReportController.PlaytechCasino.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    /// The PLAYTECH Casino controller.
    /// </summary>
    public partial class ReportController
    {
        #region Web Controller

        /// <summary>
        /// The PLAYTECH Casino report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult PlaytechCasinoWinloss()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var metaModel = new PlaytechCasinoMetaModel
            {
                Level = 0,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                LicenseeId = licenseeSession.LicenseeId,
                ReportBy = ReportByType.Daily,
                IsExcludedTestData = true,
            };

            var model = new PlaytechCasinoViewModel
            {
                Param = metaModel,
                ShowEmptyContent = true,
                CurrencyList = SessionService.UserSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
            };

            return View("~/Views/Report/PlaytechCasino/Main.cshtml", model);
        }

        /// <summary>
        /// The PLAYTECH Casino report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        [ValidateInput(false)]
        public ActionResult PlaytechCasinoWinloss(PlaytechCasinoMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            IPlaytechCasinoService playtechCasinoService = new PlaytechCasinoService();
            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BaseCurrency = metaModel.CurrencyId == BaseCurrencyType.Eur ? "EUR" : metaModel.CurrencyId == BaseCurrencyType.Usd ? "USD" : string.Empty;
            var currencyList = userSession.CurrencyList;
            metaModel.CurrencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.Level == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrency)).Key;
            IList<PlaytechCasino> dataList = new List<PlaytechCasino>();
            var totalInEur = new PlaytechCasino();
            var totalInUsd = new PlaytechCasino();
            if (metaModel.ReportBy == ReportByType.Monthly && metaModel.Level > 0)
            {
                metaModel.DrillDownToDate = !metaModel.IsExportExcel ? metaModel.DrillDownToDate.AddMonths(1).AddDays(-1) : metaModel.DrillDownToDate;
                metaModel.DrillDownFromDate = metaModel.FromDate > metaModel.DrillDownFromDate ? metaModel.FromDate : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = metaModel.ToDate < metaModel.DrillDownToDate ? metaModel.ToDate : metaModel.DrillDownToDate;
            }

            switch (metaModel.Level)
            {
                case 0:
                    dataList = playtechCasinoService.GetMainWinLoss(metaModel, ref totalInEur, ref totalInUsd);
                    break;
                case 1:
                    dataList = playtechCasinoService.GetAgentWinLoss(metaModel, ref totalInEur, ref totalInUsd);
                    break;
                default:
                    dataList = playtechCasinoService.GetMemberWinLoss(metaModel, ref totalInEur, ref totalInUsd);
                    break;
            }

            var model = new PlaytechCasinoViewModel
            {
                Param = metaModel,
                MainData = dataList,
                TotalData = GetTotalDataList(dataList),
                TotalInEur = totalInEur,
                TotalInUsd = totalInUsd,
                ShowEmptyContent = false,
                CurrencyList = currencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
            };

            if (metaModel.IsExportExcel)
            {
                return PlaytechCasinoWinlossExcel(model);
            }

            return View("~/Views/Report/PlaytechCasino/Main.cshtml", model);
        }

        #endregion Web Controller

        #region Export excel

        /// <summary>
        /// PLAYTECH Casino excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult PlaytechCasinoWinlossExcel(PlaytechCasinoViewModel viewModel)
        {
            const string FileName = "PlaytechCasino";

            var templatePath = Server.MapPath(@"~/App_Data/Template/Reports/PlaytechCasino.xls");
            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException(templatePath);
            }

            var convertingOption = new ConvertingOption
                {
                    IsConvert = viewModel.Param.IsConvert,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.CurrencyName
                };

            var playtechCasinoExcel = new PlaytechCasinoExcel(templatePath, FileName, convertingOption, viewModel);
            playtechCasinoExcel.Export();

            return new ExcelResult(playtechCasinoExcel.Workbook, FileName);
        }

        #endregion Export excel

        /// <summary>
        /// Get total data list.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Data list.</returns>
        private IList<PlaytechCasino> GetTotalDataList(IList<PlaytechCasino> dataList)
        {
            return dataList.GroupBy(d => d.CurrencyName)
                           .Select(g => new PlaytechCasino
                           {
                               CurrencyName = g.Key,
                               TotalTrans = g.Sum(m => m.TotalTrans),
                               Turnover = g.Sum(m => m.Turnover),
                               CustomerWinLoss = g.Sum(m => m.CustomerWinLoss),
                               LicenseeWinLoss = g.Sum(m => m.LicenseeWinLoss),
                               ProgressiveShare = g.Sum(m => m.ProgressiveShare),
                               ProgressiveWin = g.Sum(m => m.ProgressiveWin),
                               NetGaming = g.Sum(m => m.NetGaming),
                           })
                           .ToList();
        }
    }
}