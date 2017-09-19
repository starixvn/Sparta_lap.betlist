// --------------------------------------------------------------------------
// <copyright file="ReportController.WinLossByLeague.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByLeague)]
    public class WinLossByLeagueReportController : ApiReportBaseController
    {
        private readonly IWinLossByLeagueService _winlossByLeagueService;

        private readonly IBalanceUpService _balanceUpService;

        public WinLossByLeagueReportController()
        {
            _winlossByLeagueService = new WinLossByLeagueService();
            _balanceUpService = new BalanceUpService();
        }

        public WinLossByLeagueReportController(
            ISessionService sessionService,
            IProductService productService,
            IBalanceUpService balanceUpService,
            IWinLossByLeagueService winlossByLeagueService)
            : base(sessionService, productService)
        {
            _balanceUpService = balanceUpService;
            _winlossByLeagueService = winlossByLeagueService;
        }

        [HttpPost]
        public dynamic Index([FromBody] WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                return BadRequest(ViewResources.InvalidParams);
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

        [HttpPost]
        public HttpResponseMessage Excel(WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ViewResources.InvalidParams + "1");
            }

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            WinLossByLeague grandTotal;
            var totalrows = 0;
            var dataList = GetData(metaModel, out grandTotal, ref totalrows);
            const string FileName = "WinLossByLeague.xls";
            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/WinLossByLeagues.xls");

            var viewModel = new WinLossByLeagueViewModel
            {
                MainResult = dataList
            };

            var winlossByLeague = CreateExcel(templatePath, grandTotal, metaModel, viewModel);
            winlossByLeague.Export();

            return ExcelStream(winlossByLeague.Workbook, FileName);
        }

        [CLSCompliant(false)]
        protected internal virtual WinlossByLeagueExcel CreateExcel(
            string templatePath,
            WinLossByLeague grandTotal,
            WinLossByLeagueMetaModel metaModel,
            WinLossByLeagueViewModel viewModel)
        {
            return new WinlossByLeagueExcel(templatePath, metaModel, viewModel, grandTotal)
            {
                Level = metaModel.Level
            };
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
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.MoveBetsDay = _balanceUpService.BalanceUpInfo.MoveBetsDay;
            metaModel.BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay;

            IEnumerable<WinLossByLeague> dataList = null;
            if (metaModel.Level == 0)
            {
                dataList = _winlossByLeagueService.GetSummary(metaModel);
                grandTotal = CalculateTotal(dataList);
            }
            else
            {
                dataList = _winlossByLeagueService.GetCustomers(metaModel, out grandTotal, ref totalrows);
            }

            return dataList;
        }
    }
}