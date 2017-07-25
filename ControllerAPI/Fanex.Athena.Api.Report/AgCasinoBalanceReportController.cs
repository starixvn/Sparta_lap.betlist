// --------------------------------------------------------------------------
// <copyright file="ReportController.AgCasinoBalance.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
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
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportAGCasinoBalance)]
    public class AgCasinoBalanceReportController : ApiReportBaseController
    {
        private readonly IAgCasinoBalanceService _agcasinoBalanceService;

        public AgCasinoBalanceReportController()
        {
            _agcasinoBalanceService = new AgCasinoBalanceService();
        }

        public AgCasinoBalanceReportController(
            IAgCasinoBalanceService agcasinoBalanceService,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _agcasinoBalanceService = agcasinoBalanceService;
        }

        [HttpPost]
        public dynamic Index(AgCasinoBalanceMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        private static dynamic GetAgCasinoBalanceEmptyResult()
        {
            return new
            {
                TotalRows = 0,
                Data = new List<AgCasinoBalance>(),
                Total = new List<AgCasinoBalance>(),
                SummaryTotal = new Dictionary<string, AgCasinoBalance>()
            };
        }

        private HttpResponseMessage ExportExcel(AgCasinoBalanceMetaModel metaModel, IList<AgCasinoBalance> dataList, IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            const string FileName = "AgCasinoBalance.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/AgCasinoBalance.xls");

            var currencyList = userSession.CurrencyList;

            var currencyName = currencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Value
            };

            // Update metalModel Value
            if (metaModel.DrillDownLevel > 0 && metaModel.CurrencyId > 0)
            {
                metaModel.CurrencyId = metaModel.DrillDownCurrencyId;
            }

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
            }

            var excelModel = new AgCasinoBalanceViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = new AgCasinoBalanceExcel(templatePath, convertingOption, metaModel, excelModel)
            {
                Level = metaModel.DrillDownLevel,
            };

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private void GetData(
                AgCasinoBalanceMetaModel metaModel,
                out SortableList<AgCasinoBalance> dataList,
                out IDictionary<string, AgCasinoBalance> summaryTotal,
                out int totalRows)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(
                metaModel.DrillDownCurrencyId == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId,
                metaModel.IsExcludedTestData);
            totalRows = 0;
            dataList = metaModel.DrillDownLevel == 0 ? _agcasinoBalanceService.GetSummaryBalance(metaModel, out summaryTotal) : metaModel.DrillDownLevel == 2
                               ? _agcasinoBalanceService.GetCustomerBalanceDetail(metaModel, out summaryTotal, out totalRows)
                               : _agcasinoBalanceService.GetAgentBalanceDetail(metaModel, out summaryTotal);
        }

        private dynamic ProcessRequest(AgCasinoBalanceMetaModel metaModel)
        {
            if (IsValidRequest(metaModel, metaModel.DrillDownCurrencyId))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

                IDictionary<string, AgCasinoBalance> summaryTotal;
                SortableList<AgCasinoBalance> dataList;
                int totalRows;
                GetData(metaModel, out dataList, out summaryTotal, out totalRows);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, summaryTotal);
                }

                if (dataList != null && dataList.Count > 0)
                {
                    return new
                    {
                        TotalRows = totalRows,
                        Data = dataList,
                        Total = _agcasinoBalanceService.TotalCalc(dataList),
                        SummaryTotal = summaryTotal
                    };
                }
            }

            return GetAgCasinoBalanceEmptyResult();
        }
    }
}