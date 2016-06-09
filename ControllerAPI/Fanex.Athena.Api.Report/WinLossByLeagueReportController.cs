// --------------------------------------------------------------------------
// <copyright file="ReportController.WinLossByLeague.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Resources;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Implement.Report;
    using Service.Interface.Report;
    using Web.ActionFilter;

    public class WinLossByLeagueReportController : ApiReportBaseController
    {
        private readonly IWinLossByLeagueService _winlossByLeagueService;

        private readonly IBalanceUpService _balanceUpService;

        public WinLossByLeagueReportController()
        {
            _winlossByLeagueService = new WinLossByLeagueService();
            _balanceUpService = new BalanceUpService();
        }

        [HttpPost]
        [ApiFunctionAuthorization]
        public dynamic WinLossByLeague([FromBody] WinLossByLeagueMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        [HttpGet]
        [ApiFunctionAuthorization]
        public HttpResponseMessage WinLossByLeagueExcel([FromUri] WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ViewResources.InvalidParams));
            }

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            WinLossByLeague grandTotal;
            var totalrows = 0;
            var dataList = GetData(metaModel, out grandTotal, ref totalrows);
            const string FileName = "WinLossByLeague.xls";
            var templatePath = HttpContext.Current.Server.MapPath("~/App_Data/Template/Reports/WinLossByLeagues.xls");

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

        private static WinLossByLeague CalculateTotal(IEnumerable<WinLossByLeague> dataList)
        {
            var total = new WinLossByLeague();
            foreach (var item in dataList)
            {
                total.BetCount += item.BetCount;
                total.TurnOver += item.TurnOver;
                total.CustomerWinLoss += item.CustomerWinLoss;
                total.LicenseeWL += item.LicenseeWL;
                total.BACommission += item.BACommission;
                total.AmountDue += item.AmountDue;
            }

            return total;
        }

        private IEnumerable<WinLossByLeague> GetData(WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalrows)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.MoveBetsDay = _balanceUpService.BalanceUpInfo.MoveBetsDay;
            metaModel.BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay;

            IEnumerable<WinLossByLeague> dataList = null;
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

        private dynamic ProcessRequest(WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ViewResources.InvalidParams));
            }

            WinLossByLeague grandTotal;
            var totalrows = 0;
            var dataList = GetData(metaModel, out grandTotal, ref totalrows);
            return new
            {
                TotalRows = totalrows,
                Data = dataList,
                Total = grandTotal
            };
        }
    }
}