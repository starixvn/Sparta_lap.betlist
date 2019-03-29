// -----------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportColossusBetsWinLoss)]
    public class ColossusBetsWinLossReportController : ApiReportBaseController
    {
        private readonly IColossusBetsWinLossService _colossusBetsWinLossService;

        public ColossusBetsWinLossReportController(
            IColossusBetsWinLossService colossusBetsWinLossService,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _colossusBetsWinLossService = colossusBetsWinLossService;
        }

        [HttpPost]
        public Task<dynamic> Index(
            ColossusBetsWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        [HttpPost]
        public Task<dynamic> Index([FromBody] ColossusBetsWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        protected internal virtual ColossusBetsWinLossExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ColossusBetsWinLossMetaModel metaModel,
            ColossusBetWinLossViewModel excelModel)
        {
            return new ColossusBetsWinLossExcel(templatePath, convertingOption, metaModel, excelModel);
        }

        private static dynamic GetEmptyThirdPartyWinLossResult()
        {
            return new
            {
                Data = new List<ColossusBetWinLoss>(),
                Total = new List<ColossusBetWinLoss>(),
                SummaryTotal = new Dictionary<string, ColossusBetWinLoss>()
            };
        }

        private HttpResponseMessage ExportExcel(
            ColossusBetsWinLossMetaModel metaModel,
            IList<ColossusBetWinLoss> dataList,
            IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/ColossusBetsWinLoss.xls");
            var currency = userSession.CurrencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));

            var exportedFileName = string.Format(
                "ColossusBetsWinLoss_{0:ddMMMyyyy}_{1}.xls",
                metaModel.DrillDownLevel > 0 ? metaModel.DrillDownFromDate : metaModel.FromDate,
                currency.Key == 0 ? "All" : currency.Value);
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currency.Value
            };

            if (metaModel.DrillDownLevel > 0 && metaModel.CurrencyId > 0)
            {
                metaModel.CurrencyId = metaModel.DrillDownCurrencyId;
            }

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
            }

            var excelModel = new ColossusBetWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = CreateExcel(templatePath, convertingOption, metaModel, excelModel);
            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        private async Task<SortableReportResultModel<ColossusBetWinLoss>> GetData(ColossusBetsWinLossMetaModel metaModel)
        {
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            return metaModel.DrillDownLevel == 0 ?
                await _colossusBetsWinLossService.GetSummaryWinloss(metaModel, userCurrencyList).ConfigureAwait(false) :
                await _colossusBetsWinLossService.GetWinlossDetail(metaModel, userCurrencyList).ConfigureAwait(false);
        }

        private async Task<dynamic> ProcessRequest(
            ColossusBetsWinLossMetaModel metaModel,
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
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level > 0 ? currencyId : metaModel.CurrencyId, metaModel.IsExcludedTestData);

                var result = await GetData(metaModel).ConfigureAwait(false);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result.DataList, result.SummaryTotal);
                }

                if (result.DataList != null && result.DataList.Any())
                {
                    return new
                    {
                        Data = result.DataList,
                        Total = _colossusBetsWinLossService.CalculateTotal(result.DataList),
                        SummaryTotal = result.SummaryTotal
                    };
                }
            }

            return GetEmptyThirdPartyWinLossResult();
        }
    }
}