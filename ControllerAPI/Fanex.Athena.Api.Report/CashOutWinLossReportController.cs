// --------------------------------------------------------------------------
// <copyright file="CashOutWinLossReportController.cs" company="">
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
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCashOutWinLoss)]
    public class CashOutWinLossReportController : ApiReportBaseController
    {
        private readonly ICashOutWinLossService _cashOutWinLossService;

        public CashOutWinLossReportController()
        {
            _cashOutWinLossService = new CashOutWinLossService();
        }

        public CashOutWinLossReportController(
            ICashOutWinLossService cashOutWinLossService,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _cashOutWinLossService = cashOutWinLossService;
        }

        [HttpPost]
        public dynamic Index(
            CashOutWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        [HttpPost]
        public dynamic Index([FromBody] CashOutWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        [CLSCompliant(false)]
        protected internal virtual CashOutWinLossExcel CreateExcel(
            string templatePath,
            string exportingFileName,
            ConvertingOption convertingOption,
            CashOutWinLossMetaModel metaModel,
            CashOutWinLossViewModel viewModel)
        {
            return new CashOutWinLossExcel(templatePath, convertingOption, metaModel, viewModel)
            {
                Level = metaModel.DrillDownLevel,
            };
        }

        private static dynamic GetEmptyResult()
        {
            return new
            {
                Data = new List<CashOutWinLoss>(),
                Total = new List<CashOutWinLoss>(),
                SummaryTotal = new Dictionary<string, CashOutWinLoss>()
            };
        }

        private HttpResponseMessage ExportExcel(CashOutWinLossMetaModel metaModel, IList<CashOutWinLoss> dataList, IDictionary<string, CashOutWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/CashOutWinLoss.xls");

            var currencyList = userSession.CurrencyList;

            var currency = currencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));
            var exportedFileName = string.Format(
                "CashOutWinLoss_{0:ddMMMyyyy}_{1}.xls",
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

            if (metaModel.DrillDownLevel > 0 && metaModel.CurrencyId <= 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
            }

            var excelModel = new CashOutWinLossViewModel
            {
                IsLicenseeOfferBA = SessionService.LicenseeSession.IsEnableBA,
                MainResult = dataList,
                SummaryTotal = summaryTotal
            };

            var excel = CreateExcel(templatePath, exportedFileName, convertingOption, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        private dynamic ProcessRequest(
            CashOutWinLossMetaModel metaModel,
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
                var result = GetData(metaModel);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result.DataList, result.SummaryTotal);
                }

                if (result.DataList != null && result.DataList.Any())
                {
                    return new
                    {
                        Data = result.DataList,
                        Total = _cashOutWinLossService.TotalCalc(result.DataList),
                        SummaryTotal = result.SummaryTotal
                    };
                }
            }

            return GetEmptyResult();
        }

        private SortableReportResultModel<CashOutWinLoss> GetData(CashOutWinLossMetaModel metaModel)
        {
            return metaModel.DrillDownLevel == 0 ?
                _cashOutWinLossService.GetSummary(metaModel) :
                _cashOutWinLossService.GetDetail(metaModel);
        }
    }
}