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
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportColossusBetsWinLoss)]
    public class ColossusBetsWinLossReportController : ApiReportBaseController
    {
        private readonly IColossusBetsWinLossService _colossusBetsWinLossService;

        public ColossusBetsWinLossReportController()
        {
            _colossusBetsWinLossService = new ColossusBetsWinLossService();
        }

        public ColossusBetsWinLossReportController(
            IColossusBetsWinLossService colossusBetsWinLossService,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _colossusBetsWinLossService = colossusBetsWinLossService;
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

        [CLSCompliant(false)]
        protected internal virtual ColossusBetsWinLossExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ThirdPartyWinLossMetaModel metaModel,
            ColossusBetWinLossViewModel excelModel)
        {
            return new ColossusBetsWinLossExcel(templatePath, convertingOption, metaModel, excelModel)
            {
                Level = metaModel.DrillDownLevel
            };
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
            ThirdPartyWinLossMetaModel metaModel,
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

        private void GetData(
                ThirdPartyWinLossMetaModel metaModel,
                out SortableList<ColossusBetWinLoss> dataList,
                out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            if (metaModel.DrillDownLevel == 0)
            {
                dataList = _colossusBetsWinLossService.GetSummaryWinloss(metaModel, out summaryTotal);
            }
            else
            {
                dataList = _colossusBetsWinLossService.GetWinlossDetail(metaModel, out summaryTotal);
            }
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
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level > 0 ? currencyId : metaModel.CurrencyId, metaModel.IsExcludedTestData);
                IDictionary<string, ColossusBetWinLoss> summaryTotal;
                SortableList<ColossusBetWinLoss> dataList;
                GetData(metaModel, out dataList, out summaryTotal);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, summaryTotal);
                }

                if (dataList != null && dataList.Count > 0)
                {
                    return new
                    {
                        Data = dataList,
                        Total = _colossusBetsWinLossService.CalculateTotal(dataList),
                        SummaryTotal = summaryTotal
                    };
                }
            }

            return GetEmptyThirdPartyWinLossResult();
        }
    }
}