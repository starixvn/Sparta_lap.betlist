// --------------------------------------------------------------------------
// <copyright file="ReportController.WinLossByLeague.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
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
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
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
            ILanguageService langService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IWinLossByLeagueDataAccess winlossByLeagueService)
            : base(sessionService, productService, langService)
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

            var resultModel = GetData(metaModel);
            return new
            {
                TotalRows = resultModel.TotalRows,
                Data = resultModel.DataList,
                Total = resultModel.TotalData
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

            var resultModel = GetData(metaModel);
            const string FileName = "WinLossByLeague.xls";
            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/WinLossByLeagues.xls");

            var winlossByLeague = CreateExcel(templatePath, resultModel, metaModel);
            winlossByLeague.Export();

            return ExcelStream(winlossByLeague.Workbook, FileName);
        }

        protected internal virtual WinlossByLeagueExcel CreateExcel(
            string templatePath,
            ReportResultModel<WinLossByLeague> resultModel,
            WinLossByLeagueMetaModel metaModel)
        {
            return new WinlossByLeagueExcel(templatePath, resultModel, metaModel)
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

        private ReportResultModel<WinLossByLeague> GetData(WinLossByLeagueMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;

            ReportResultModel<WinLossByLeague> resultModel;

            if (metaModel.Level == 0)
            {
                var dataList = metaModel.IsValid
                       ? this._winlossByLeagueDataAccess.GetSummary(metaModel)
                       : new List<WinLossByLeague>();
                resultModel = new ReportResultModel<WinLossByLeague>
                {
                    DataList = dataList,
                    TotalData = CalculateTotal(dataList)
                };
            }
            else
            {
                resultModel = metaModel.IsValid ?
                    _winlossByLeagueDataAccess.GetCustomers(metaModel) :
                    new ReportResultModel<WinLossByLeague>
                    {
                        DataList = new List<WinLossByLeague>(),
                        TotalData = new WinLossByLeague()
                    };
            }

            return resultModel;
        }
    }
}