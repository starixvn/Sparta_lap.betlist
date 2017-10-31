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
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportAGCasinoWinLoss)]
    public class AgCasinoWinLossReportController : ApiReportBaseController
    {
        private readonly IAgCasinoWinLossService _agcasinoWinLossService;

        public AgCasinoWinLossReportController()
        {
            _agcasinoWinLossService = new AgCasinoWinLossService();
        }

        public AgCasinoWinLossReportController(IAgCasinoWinLossService agcasinoWinLossService, ISessionService sessionService, IProductService productService)
            : base(sessionService, productService)
        {
            _agcasinoWinLossService = agcasinoWinLossService;
        }

        [HttpPost]
        public dynamic Index(
            AgCasinoWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        [HttpPost]
        public dynamic Index([FromBody] AgCasinoWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        [CLSCompliant(false)]
        protected internal virtual AgCasinoWinLossExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            AgCasinoWinLossViewModel excelModel)
        {
            return new AgCasinoWinLossExcel(templatePath, convertingOption, excelModel)
            {
                Level = excelModel.Param.DrillDownLevel
            };
        }

        private static dynamic GetEmptyResult()
        {
            return new
            {
                Data = new List<AgCasinoWinLoss>(),
                Total = new List<AgCasinoWinLoss>(),
                SummaryTotal = new Dictionary<string, AgCasinoWinLoss>()
            };
        }

        private HttpResponseMessage ExportExcel(
            AgCasinoWinLossMetaModel metaModel,
            IEnumerable<AgCasinoWinLoss> dataList,
            IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/AgCasinoWinLoss.xls");
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

            var exccelModel = new AgCasinoWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
                Param = metaModel
            };

            var excel = CreateExcel(templatePath, convertingOption, exccelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        private dynamic ProcessRequest(
            AgCasinoWinLossMetaModel metaModel,
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
                        Total = _agcasinoWinLossService.CalculateTotal(data.DataList),
                        SummaryTotal = data.SummaryTotal
                    };
                }
            }

            return GetEmptyResult();
        }

        private ReportResultModel<AgCasinoWinLoss> GetData(AgCasinoWinLossMetaModel metaModel)
        {
            return metaModel.DrillDownLevel == 0 ? _agcasinoWinLossService.GetSummaryWinloss(metaModel) : metaModel.DrillDownLevel == 2
                               ? _agcasinoWinLossService.GetCustomerWinlossDetail(metaModel)
                               : _agcasinoWinLossService.GetAgentWinlossDetail(metaModel);
        }
    }
}