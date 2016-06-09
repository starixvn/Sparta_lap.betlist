// -----------------------------------------------------------------------
// <copyright file="ReportController.PlaytechCasino.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    public class PlaytechCasinoWinlossReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
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

        [HttpPost]
        [FunctionAuthorization]
        [ValidateInput(false)]
        [SuppressMessage(
          "StyleCop.CSharp.StylecopPlus.Rules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Index(PlaytechCasinoMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            IPlaytechCasinoService playtechCasinoService = new PlaytechCasinoService();
            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BaseCurrency = GetBaseCurrencyName(metaModel.CurrencyId);
            var currencyList = userSession.CurrencyList;
            metaModel.CurrencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.Level == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrency)).Key;
            IList<PlaytechCasino> dataList = new List<PlaytechCasino>();
            var totalInEur = new PlaytechCasino();
            var totalInUsd = new PlaytechCasino();
            var totalInGbp = new PlaytechCasino();
            if (metaModel.ReportBy == ReportByType.Monthly && metaModel.Level > 0)
            {
                metaModel.DrillDownToDate = !metaModel.IsExportExcel ? metaModel.DrillDownToDate.AddMonths(1).AddDays(-1) : metaModel.DrillDownToDate;
                metaModel.DrillDownFromDate = metaModel.FromDate > metaModel.DrillDownFromDate ? metaModel.FromDate : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = metaModel.ToDate < metaModel.DrillDownToDate ? metaModel.ToDate : metaModel.DrillDownToDate;
            }

            switch (metaModel.Level)
            {
                case 0:
                    dataList = playtechCasinoService.GetMainWinLoss(metaModel, ref totalInEur, ref totalInUsd, ref totalInGbp);
                    break;
                case 1:
                    dataList = playtechCasinoService.GetAgentWinLoss(metaModel, ref totalInEur, ref totalInUsd, ref totalInGbp);
                    break;
                default:
                    dataList = playtechCasinoService.GetMemberWinLoss(metaModel, ref totalInEur, ref totalInUsd, ref totalInGbp);
                    break;
            }

            var model = new PlaytechCasinoViewModel
            {
                Param = metaModel,
                MainData = dataList,
                TotalData = GetTotalDataList(dataList),
                TotalInEur = totalInEur,
                TotalInUsd = totalInUsd,
                TotalInGbp = totalInGbp,
                ShowEmptyContent = false,
                CurrencyList = currencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
            };

            if (metaModel.IsExportExcel)
            {
                return Excel(model);
            }

            return View("~/Views/Report/PlaytechCasino/Main.cshtml", model);
        }

        [HttpPost]
        public ActionResult Excel(PlaytechCasinoViewModel viewModel)
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

        private static IList<PlaytechCasino> GetTotalDataList(IList<PlaytechCasino> dataList)
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

        private static string GetBaseCurrencyName(int currencyId)
        {
            switch (currencyId)
            {
                case BaseCurrencyType.Eur:
                    return "EUR";

                case BaseCurrencyType.Usd:
                    return "USD";

                case BaseCurrencyType.Gbp:
                    return "GBP";

                default:
                    return string.Empty;
            }            
        }
    }
}