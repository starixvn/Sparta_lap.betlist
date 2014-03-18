// --------------------------------------------------------------------------
// <copyright file="ReportController.WinLossByLeague.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;

    /// <summary>
    /// WinLossByLeagueController class.
    /// </summary>
    public partial class ReportController
    {
        #region Action

        /// <summary>
        /// Win Loss By Product - Level 0.
        /// </summary>
        /// <param name="metaModel">The level 0 MetaModel.</param>
        /// <returns>Process Request.</returns>
        [HttpPost]
        public dynamic WinLossByLeague([FromBody] WinLossByLeagueMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        #endregion Action

        #region ProcessRequest

        /// <summary>
        /// Win Loss By Product - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Http Response Message.</returns>
        [HttpGet]
        public HttpResponseMessage WinLossByLeagueExcel([FromUri] WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                RedirectToErrorPage();
            }

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            WinLossByLeague grandTotal;
            var totalrows = 0;
            var dataList = GetData(metaModel, out grandTotal, ref totalrows);
            const string FileName = "WinLossByLeague.xls";
            var templatePath = HttpContext.Current.Server.MapPath("~/App_Data/Template/Reports/WinLossByLeagues.xls");
            var licenseeSession = SessionService.LicenseeSession;

            var currencyList = SessionService.UserSession.CurrencyList;
            var currencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.Level == 0 ? metaModel.CurrencyId : metaModel.CurrencyId));

            var convertingOption = new ConvertingOption
            {
                IsConvert = false,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Key
            };

            var viewModel = new WinLossByLeagueViewModel
            {
                MainResult = dataList
            };

            var winlossByLeague = new WinlossByLeagueExcel(templatePath, FileName, convertingOption, metaModel, viewModel, grandTotal)
            {
                Level = metaModel.Level
            };
            winlossByLeague.Export();

            return ExcelStream(winlossByLeague.Workbook, FileName);
        }

        /// <summary>
        /// Win Loss By Product - Get report data.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="grandTotal">Grand total object.</param>
        /// <param name="totalrows">Total rows.</param>
        /// <returns>Report data list.</returns>
        private IList<WinLossByLeague> GetData(WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalrows)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var licenseeId = licenseeSession.LicenseeId;
            metaModel.LicenseeId = licenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.MoveBetsDay = _balanceUpService.BalanceUpInfo.MoveBetsDay;
            metaModel.BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay;

            IList<WinLossByLeague> dataList = null;
            if (metaModel.Level == 0)
            {
                dataList = _winlossByLeagueService.GetWinlossByLeagues(metaModel);
                grandTotal = CalculateTotal(dataList);
            }
            else
            {
                dataList = _winlossByLeagueService.GetWinlossByLeagueCustomers(metaModel, out grandTotal, ref totalrows);
            }

            return dataList;
        }

        /// <summary>
        ///  Process request.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic type.</returns>
        private dynamic ProcessRequest(WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                RedirectToErrorPage();
            }

            WinLossByLeague grandTotal;
            var totalrows = 0;
            var dataList = GetData(metaModel, out grandTotal, ref totalrows);
            return dataList.Count > 0
                ? (dynamic)new
                {
                    TotalRows = totalrows,
                    Data = dataList,
                    Total = grandTotal
                }
                : new
                {
                    TotalRows = totalrows,
                    Data = new List<LicenseeWinLossGridViewModel>(),
                    Total = new List<LicenseeWinLossGridViewModel>(),
                };
        }

        /// <summary>
        /// Get total data.
        /// </summary>
        /// <param name="dataList">List data.</param>
        /// <returns>Total data.</returns>
        private WinLossByLeague CalculateTotal(IList<WinLossByLeague> dataList)
        {
            return new WinLossByLeague()
            {
                BetCount = dataList.Sum(x => x.BetCount),
                TurnOver = dataList.Sum(x => x.TurnOver),
                CustomerWinLoss = dataList.Sum(x => x.CustomerWinLoss),
                LicenseeWL = dataList.Sum(x => x.LicenseeWL),
                BaCommission = dataList.Sum(x => x.BaCommission),
                AmountDue = dataList.Sum(x => x.AmountDue)
            };
        }

        #endregion ProcessRequest
    }
}