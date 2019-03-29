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
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCashOutWinLoss)]
    public class CashOutWinLossReportController : ApiReportBaseController
    {
        private readonly ICashOutWinLossDataAccess _cashOutWinLossDataAccess;

        public CashOutWinLossReportController(
            ICashOutWinLossDataAccess cashOutWinLossDataAccess,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _cashOutWinLossDataAccess = cashOutWinLossDataAccess;
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

        protected internal virtual CashOutWinLossExcel CreateExcel(
            string templatePath,
            string exportingFileName,
            ConvertingOption convertingOption,
            ThirdPartyWinLossMetaModel metaModel,
            CashOutWinLossViewModel viewModel)
        {
            return new CashOutWinLossExcel(templatePath, convertingOption, metaModel, viewModel);
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

        private HttpResponseMessage ExportExcel(
            ThirdPartyWinLossMetaModel metaModel,
            IList<CashOutWinLoss> dataList,
            IEnumerable<CashOutWinLoss> totalData,
            IDictionary<string, CashOutWinLoss> summaryTotal)
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
                TotalData = totalData,
                SummaryTotal = summaryTotal
            };

            var excel = CreateExcel(templatePath, exportedFileName, convertingOption, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        private async Task<dynamic> ProcessRequest(
            ThirdPartyWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            if (!IsValidRequest(metaModel.CurrencyId, currencyId))
            {
                return GetEmptyResult();
            }

            UpdateMetaModel(metaModel, fdate, tdate, level, currencyId, custId);
            var result = await GetData(metaModel).ConfigureAwait(false);

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel, result.DataList, result.TotalDataList, result.SummaryTotal);
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

            return GetEmptyResult();
        }

        private void UpdateMetaModel(
            ThirdPartyWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level,
            int currencyId,
            int custId)
        {
            metaModel.DrillDownCurrencyId = currencyId;
            metaModel.DrillDownLevel = level;
            metaModel.DrillDownCustomerId = custId;
            metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
            metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(level > 0 ? currencyId : metaModel.CurrencyId, metaModel.IsExcludedTestData);
        }

        private async Task<SortableReportResultModel<CashOutWinLoss>> GetData(ThirdPartyWinLossMetaModel metaModel)
        {
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            return metaModel.DrillDownLevel == 0 ?
                await _cashOutWinLossDataAccess.GetSummary(metaModel, userCurrencyList).ConfigureAwait(false) :
                await _cashOutWinLossDataAccess.GetDetail(metaModel, userCurrencyList).ConfigureAwait(false);
        }
    }
}