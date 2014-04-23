// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerPendingFunds.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Fanex.Athena.Models.DTO.Report;
    using System.Collections.Generic;
    using System.Linq;

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
        public ActionResult PlaytechCasino()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var metaModel = new PlaytechCasinoMetaModel
            {
                Level = 0,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                LicenseeId = licenseeSession.LicenseeId,
                ReportBy = ReportByType.Daily,
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
        public ActionResult PlaytechCasino(PlaytechCasinoMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.BaseCurrency = metaModel.CurrencyId == -1 ? "EUR" : metaModel.CurrencyId == -2 ? "USD" : string.Empty;
            var currencyList = SessionService.UserSession.CurrencyList;
            metaModel.CurrencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.Level == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrency)).Key;
            IList<PlaytechCasino> dataList = new List<PlaytechCasino>();
            var totalInEur = new PlaytechCasino();
            var totalInUsd = new PlaytechCasino();

            if (metaModel.ReportBy == ReportByType.Monthly && metaModel.Level > 0)
            {
                metaModel.DrillDownToDate = metaModel.DrillDownToDate.AddMonths(1).AddDays(-1);
            }
            
            switch (metaModel.Level)
            {
                case 0:
                    dataList = _playtechCasinoService.GetMainWinLoss(metaModel, ref totalInEur, ref totalInUsd);
                    break;
                case 1:
                    dataList = _playtechCasinoService.GetAgentWinLoss(metaModel, ref totalInEur, ref totalInUsd);
                    break;
                default:
                    dataList = _playtechCasinoService.GetMemberWinLoss(metaModel, ref totalInEur, ref totalInUsd);
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
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

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
        public ActionResult PlaytechCasinoExcel(CustomerPendingFundsViewModel viewModel)
        {
            const string FileName = "CustomerPendingFunds";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerPendingFunds.xls");
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

            var custPendingFundsExcel = new CustomerPendingFundsExcel(templatePath, FileName, convertingOption, viewModel);
            custPendingFundsExcel.Export();

            return new ExcelResult(custPendingFundsExcel.Workbook, FileName);
        }

        #endregion Export excel

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
                           })
                           .ToList();
        }
    }
}