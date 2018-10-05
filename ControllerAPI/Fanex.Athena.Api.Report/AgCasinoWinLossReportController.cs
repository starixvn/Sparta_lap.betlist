// --------------------------------------------------------------------------
// <copyright file="ReportController.LicenseeWl.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportAGCasinoWinLoss)]
    public class AgCasinoWinLossReportController : ApiReportBaseController
    {
        private readonly IThirdPartyWinLossService _thirdPartyWinLossService;

        public AgCasinoWinLossReportController(
            IThirdPartyWinLossService thirdPartyWinLossService,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _thirdPartyWinLossService = thirdPartyWinLossService;
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

        protected internal virtual ThirdPartyWinLossExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ThirdPartyWinLossViewModel excelModel)
        {
            return new ThirdPartyWinLossExcel(templatePath, convertingOption, excelModel.Param, excelModel)
            {
                Level = excelModel.Param.DrillDownLevel
            };
        }

        private static dynamic GetEmptyResult()
        {
            return new
            {
                Data = new List<ThirdPartyWinLoss>(),
                Total = new List<ThirdPartyWinLoss>(),
                SummaryTotal = new Dictionary<string, ThirdPartyWinLoss>()
            };
        }

        private HttpResponseMessage ExportExcel(
            ThirdPartyWinLossMetaModel metaModel,
            IEnumerable<ThirdPartyWinLoss> dataList,
            IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/ThirdPartyWinLoss.xls");
            var currency = userSession.CurrencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));
            var fromDate = metaModel.DrillDownLevel > 0 ? metaModel.DrillDownFromDate : metaModel.FromDate;
            var currencyText = currency.Key == 0 ? "All" : currency.Value;
            var exportedFileName = $"AGWinLoss_{fromDate:ddMMMyyyy}_{currencyText}.xls";
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

            metaModel.ProductName = ViewResources.ProductAGC;
            var excelModel = new ThirdPartyWinLossViewModel
            {
                MainResult = dataList.ToList(),
                SummaryTotal = summaryTotal,
                Param = metaModel
            };

            var excel = CreateExcel(templatePath, convertingOption, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
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
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level > 0 ? currencyId : metaModel.CurrencyId, metaModel.IsExcludedTestData);

                var data = GetData(metaModel);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, data.DataList, data.SummaryTotal);
                }

                if (data.DataList != null && data.DataList.Any())
                {
                    return new
                    {
                        Data = data.DataList,
                        Total = _thirdPartyWinLossService.TotalCalc(data.DataList),
                        SummaryTotal = data.SummaryTotal
                    };
                }
            }

            return GetEmptyResult();
        }

        private SortableReportResultModel<ThirdPartyWinLoss> GetData(ThirdPartyWinLossMetaModel metaModel)
        {
            switch (metaModel.DrillDownLevel)
            {
                case 0:
                    return _thirdPartyWinLossService.GetSummaryWinloss(metaModel, "Lrf_Rpt_AGCasinoWinloss");

                case 1:
                    return _thirdPartyWinLossService.GetAgentWinlossDetail(metaModel, "LRF_Rpt_AGCasinoWinloss_Ag");

                case 2:
                    return _thirdPartyWinLossService.GetCustomerWinlossDetail(metaModel, "LRF_Rpt_AGCasinoWinloss_Mem");

                default:
                    throw new ArgumentException("Invalid level: " + metaModel.DrillDownLevel);
            }
        }
    }
}