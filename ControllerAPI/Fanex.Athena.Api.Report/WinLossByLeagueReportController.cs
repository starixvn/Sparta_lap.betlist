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
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using DataAccess.Report.Interfaces;
    using Extension.Utility;
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

        private readonly IAppSettings _appSettings;

        public WinLossByLeagueReportController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IWinLossByLeagueDataAccess winlossByLeagueService,
            IAppSettings appSetting)
            : base(sessionService, productService, langService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _winlossByLeagueDataAccess = winlossByLeagueService;
            _appSettings = appSetting;
        }

        [HttpPost]
        public async Task<dynamic> Index([FromBody] WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel.CurrencyId))
            {
                return BadRequest(ViewResources.InvalidParams);
            }

            var resultModel = await GetData(metaModel).ConfigureAwait(false);
            return new
            {
                TotalRows = resultModel.TotalRows,
                Data = resultModel.DataList,
                Total = resultModel.TotalData
            };
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Excel(WinLossByLeagueMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel.CurrencyId))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ViewResources.InvalidParams + "1");
            }

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;

            var resultModel = await GetData(metaModel).ConfigureAwait(false);
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
            return new WinlossByLeagueExcel(templatePath, resultModel, metaModel, _appSettings.HasAmountDueToColumn)
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

        private async Task<ReportResultModel<WinLossByLeague>> GetData(WinLossByLeagueMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;

            ReportResultModel<WinLossByLeague> resultModel;

            if (metaModel.Level == 0)
            {
                var dataList = metaModel.IsValid
                       ? await this._winlossByLeagueDataAccess.GetSummary(metaModel).ConfigureAwait(false)
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
                    await _winlossByLeagueDataAccess.GetCustomers(metaModel).ConfigureAwait(false) :
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