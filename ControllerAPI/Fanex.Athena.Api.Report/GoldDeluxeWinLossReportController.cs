namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportGoldDeluxeWinloss)]
    public class GoldDeluxeWinLossReportController : ApiReportBaseController
    {
        private readonly IGoldDeluxeWinLossDataAccess _dataAccess;

        public GoldDeluxeWinLossReportController(
            IGoldDeluxeWinLossDataAccess dataAccess,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _dataAccess = dataAccess;
        }

        [HttpPost]
        public Task<dynamic> Index(
            ThirdPartyWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        [HttpPost]
        public Task<dynamic> Index([FromBody] ThirdPartyWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        protected internal virtual GoldDeluxeWinLossExcel CreateExcel(
            string templatePath,
            ThirdPartyWinLossMetaModel metaModel,
            ReportResultModel<GoldDeluxeWinLoss> data)
        {
            return new GoldDeluxeWinLossExcel(templatePath, metaModel, data);
        }

        private static string GetExcelFileName(ThirdPartyWinLossMetaModel metaModel, ref KeyValuePair<int, string> currency)
        {
            var fromDate = $"{(metaModel.DrillDownLevel > 0 ? metaModel.DrillDownFromDate : metaModel.FromDate):ddMMMyyyy}";
            var currencyName = currency.Key == 0 ? "All" : currency.Value;

            var exportedFileName = $"GDWinLoss_{fromDate}_{currencyName}.xls";
            return exportedFileName;
        }

        private HttpResponseMessage ExportExcel(
            ThirdPartyWinLossMetaModel metaModel,
            ReportResultModel<GoldDeluxeWinLoss> data)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/GoldDeluxeWinLoss.xls");
            var currency = userSession.CurrencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));

            var exportedFileName = GetExcelFileName(metaModel, ref currency);

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
                metaModel.CurrencyId = metaModel.CurrencyId > 0 ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            }

            var excel = CreateExcel(templatePath, metaModel, data);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        private async Task<ReportResultModel<GoldDeluxeWinLoss>> GetData(ThirdPartyWinLossMetaModel metaModel)
        {
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            if (metaModel.DrillDownLevel == 0)
            {
                return await _dataAccess.GetSummaryWinloss(metaModel, userCurrencyList).ConfigureAwait(false);
            }
            else if (metaModel.DrillDownLevel == 1)
            {
                return await _dataAccess.GetAgentWinlossDetail(metaModel, userCurrencyList).ConfigureAwait(false);
            }
            else
            {
                return await _dataAccess.GetCustomerWinlossDetail(metaModel, userCurrencyList).ConfigureAwait(false);
            }
        }

        private async Task<dynamic> ProcessRequest(
            ThirdPartyWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            if (IsValidRequest(metaModel.CurrencyId, currencyId))
            {
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level == 0 ? metaModel.CurrencyId : currencyId, metaModel.IsExcludedTestData);
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

                var result = await GetData(metaModel);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result);
                }

                if (result.DataList != null && result.DataList.Any())
                {
                    return new
                    {
                        Data = result.DataList,
                        Total = result.TotalDataList,
                        SummaryTotal = result.SummaryTotal
                    };
                }
            }

            return new
            {
                Data = Enumerable.Empty<GoldDeluxeWinLoss>(),
                Total = Enumerable.Empty<GoldDeluxeWinLoss>(),
                SummaryTotal = new Dictionary<string, GoldDeluxeWinLoss>()
            };
        }
    }
}