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
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByLeague)]
    public class WinLossByLeagueReportController : ApiReportBaseController
    {
        private readonly IWinLossByLeagueDataAccess _winlossByLeagueDataAccess;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public WinLossByLeagueReportController()
        {
            _winlossByLeagueDataAccess = new WinLossByLeagueDataAccess();
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public WinLossByLeagueReportController(
            ISessionService sessionService,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IWinLossByLeagueDataAccess winlossByLeagueService)
            : base(sessionService, productService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _winlossByLeagueDataAccess = winlossByLeagueService;
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
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;

            IEnumerable<WinLossByLeague> dataList = null;
            if (metaModel.Level == 0)
            {
                dataList = metaModel.IsValid
                       ? this._winlossByLeagueDataAccess.GetSummary(metaModel)
                       : new List<WinLossByLeague>();
                grandTotal = CalculateTotal(dataList);
            }
            else
            {
                grandTotal = new WinLossByLeague();
                dataList = metaModel.IsValid ?
                    _winlossByLeagueDataAccess.GetCustomers(metaModel, out grandTotal, ref totalrows) :
                    new List<WinLossByLeague>();
            }

            return dataList;
        }
    }
}