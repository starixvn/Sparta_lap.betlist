namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
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
        public dynamic Index(
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
        public dynamic Index([FromBody] ThirdPartyWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        protected internal virtual GoldDeluxeWinLossExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ThirdPartyWinLossMetaModel metaModel,
            ReportResultModel<GoldDeluxeWinLoss> data)
        {
            return new GoldDeluxeWinLossExcel(templatePath, convertingOption, metaModel, data);
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

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currency.Value
            };

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
                metaModel.CurrencyId = metaModel.CurrencyId > 0 ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            }

            var excel = CreateExcel(templatePath, convertingOption, metaModel, data);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        private ReportResultModel<GoldDeluxeWinLoss> GetData(ThirdPartyWinLossMetaModel metaModel)
        {
            return metaModel.DrillDownLevel == 0 ? _dataAccess.GetSummaryWinloss(metaModel) : metaModel.DrillDownLevel == 2
                               ? _dataAccess.GetCustomerWinlossDetail(metaModel)
                               : _dataAccess.GetAgentWinlossDetail(metaModel);
        }

        private dynamic ProcessRequest(
            ThirdPartyWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            if (IsValidRequest(metaModel, currencyId))
            {
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level == 0 ? metaModel.CurrencyId : currencyId, metaModel.IsExcludedTestData);

                var result = GetData(metaModel);

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