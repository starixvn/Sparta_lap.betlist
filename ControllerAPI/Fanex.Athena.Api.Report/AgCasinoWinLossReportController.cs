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
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report.Interfaces;
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

    [ApiFunctionAuthorization(PermissionKeys.ReportAGCasinoWinLoss)]
    public class AgCasinoWinLossReportController : ApiReportBaseController
    {
        private readonly IThirdPartyWinLossDataAccess<ThirdPartyWinLoss> _thirdPartyWinLossDataAccess;

        public AgCasinoWinLossReportController(
            IThirdPartyWinLossDataAccess<ThirdPartyWinLoss> thirdPartyWinLossDataAccess,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _thirdPartyWinLossDataAccess = thirdPartyWinLossDataAccess;
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

        protected internal virtual ThirdPartyWinLossExcel<ThirdPartyWinLoss> CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ThirdPartyWinLossMetaModel metaModel,
            ReportResultModel<ThirdPartyWinLoss> resultData)
        {
            return new ThirdPartyWinLossExcel<ThirdPartyWinLoss>(templatePath, convertingOption, metaModel, resultData);
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
            ReportResultModel<ThirdPartyWinLoss> resultData)
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

            var excel = CreateExcel(templatePath, convertingOption, metaModel, resultData);

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
            if (IsValidRequest(metaModel.CurrencyId, currencyId))
            {
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level > 0 ? currencyId : metaModel.CurrencyId, metaModel.IsExcludedTestData);

                var data = await GetData(metaModel);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, data);
                }

                if (data.DataList != null && data.DataList.Any())
                {
                    return new
                    {
                        Data = data.DataList,
                        Total = data.TotalDataList,
                        SummaryTotal = data.SummaryTotal
                    };
                }
            }

            return GetEmptyResult();
        }

        private async Task<ReportResultModel<ThirdPartyWinLoss>> GetData(ThirdPartyWinLossMetaModel metaModel)
        {
            var userCurrencyList = SessionService.UserSession.CurrencyList;

            switch ((ReportLevel)metaModel.DrillDownLevel)
            {
                case ReportLevel.Main:
                    return await _thirdPartyWinLossDataAccess.GetSummaryWinloss(metaModel, userCurrencyList, "Lrf_Rpt_AGCasinoWinloss").ConfigureAwait(false);

                case ReportLevel.Licensee:
                    return await _thirdPartyWinLossDataAccess.GetAgentWinlossDetail(metaModel, userCurrencyList, "LRF_Rpt_AGCasinoWinloss_Ag").ConfigureAwait(false);

                case ReportLevel.Customer:
                    return await _thirdPartyWinLossDataAccess.GetCustomerWinlossDetail(metaModel, userCurrencyList, "LRF_Rpt_AGCasinoWinloss_Mem").ConfigureAwait(false);

                default:
                    throw new ArgumentException("Invalid level: " + metaModel.DrillDownLevel);
            }
        }
    }
}