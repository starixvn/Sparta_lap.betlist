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
    using System.Web;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
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

        private static dynamic GetEmptyResult()
        {
            return new
            {
                Data = new List<AgCasinoWinLoss>(),
                Total = new List<AgCasinoWinLoss>(),
                SummaryTotal = new Dictionary<string, AgCasinoWinLoss>()
            };
        }

        private HttpResponseMessage ExportExcel(AgCasinoWinLossMetaModel metaModel, IList<AgCasinoWinLoss> dataList, IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/AgCasinoWinLoss.xls");

            var currencyList = userSession.CurrencyList;

            var currency = currencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));
            var exportedFileName = string.Format(
                "AGWinLoss_{0:ddMMMyyyy}_{1}.xls",
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

            var exccelModel = new AgCasinoWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = new AgCasinoWinLossExcel(templatePath, exportedFileName, convertingOption, metaModel, exccelModel)
            {
                Level = metaModel.DrillDownLevel,
            };

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
                IDictionary<string, AgCasinoWinLoss> summaryTotal;
                SortableList<AgCasinoWinLoss> dataList;
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
                        Total = _agcasinoWinLossService.TotalCalc(dataList),
                        SummaryTotal = summaryTotal
                    };
                }
            }

            return GetEmptyResult();
        }

        private void GetData(
                AgCasinoWinLossMetaModel metaModel,
                out SortableList<AgCasinoWinLoss> dataList,
                out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            dataList = metaModel.DrillDownLevel == 0 ? _agcasinoWinLossService.GetSummaryWinloss(metaModel, out summaryTotal) : metaModel.DrillDownLevel == 2
                               ? _agcasinoWinLossService.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                               : _agcasinoWinLossService.GetAgentWinlossDetail(metaModel, out summaryTotal);
        }
    }
}