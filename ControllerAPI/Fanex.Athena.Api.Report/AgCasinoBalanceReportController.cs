// --------------------------------------------------------------------------
// <copyright file="ReportController.AgCasinoBalance.cs" company="">
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

    [ApiFunctionAuthorization(PermissionKeys.ReportAGCasinoBalance)]
    public class AgCasinoBalanceReportController : ApiReportBaseController
    {
        private readonly IAgCasinoBalanceDataAccess _agcasinoBalanceDataAccess;

        public AgCasinoBalanceReportController(
            IAgCasinoBalanceDataAccess agcasinoBalanceDataAccess,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _agcasinoBalanceDataAccess = agcasinoBalanceDataAccess;
        }

        [HttpPost]
        public Task<dynamic> Index(ThirdPartyWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        protected internal virtual AgCasinoBalanceExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ThirdPartyWinLossMetaModel metaModel,
            AgCasinoBalanceViewModel viewModel)
        {
            return new AgCasinoBalanceExcel(templatePath, convertingOption, metaModel, viewModel);
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

        private static IList<AgCasinoBalance> CalculateTotal(IList<AgCasinoBalance> dataList)
        {
            return dataList.GroupBy(d => d.CurrencyName).Select(g => new AgCasinoBalance
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                OpeningBalance = g.Sum(p => p.OpeningBalance),
                TransferIn = g.Sum(p => p.TransferIn),
                TransferOut = g.Sum(p => p.TransferOut),
                ClosingBalance = g.Sum(p => p.ClosingBalance),
            }).ToList();
        }

        private static void AssignRowSpan(IEnumerable<AgCasinoBalance> dataList)
        {
            var date = new DateTime();
            var index = 1;
            foreach (var item in dataList)
            {
                if (date != item.Date)
                {
                    date = item.Date ?? new DateTime();
                    item.RowSpan = dataList.Count(x => x.Date == item.Date);
                    item.Index = index++;
                }
                else
                {
                    item.RowSpan = -1;
                }
            }
        }

        private HttpResponseMessage ExportExcel(
            ThirdPartyWinLossMetaModel metaModel,
            IList<AgCasinoBalance> dataList,
            IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            const string FileName = "AgCasinoBalance.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/AgCasinoBalance.xls");

            var currencyList = userSession.CurrencyList;

            var currencyName = currencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Value
            };

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
                metaModel.CurrencyId = metaModel.CurrencyId > 0 ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            }

            var viewModel = new AgCasinoBalanceViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = CreateExcel(templatePath, convertingOption, metaModel, viewModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private async Task<SortableReportResultModel<AgCasinoBalance>> GetData(ThirdPartyWinLossMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(
                metaModel.DrillDownCurrencyId == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId,
                metaModel.IsExcludedTestData);
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            if (metaModel.DrillDownLevel == 0)
            {
                var result = await _agcasinoBalanceDataAccess.GetSummaryBalance(metaModel, userCurrencyList).ConfigureAwait(false);
                AssignRowSpan(result.DataList);
                return result;
            }
            else
            {
                return metaModel.DrillDownLevel == 1 ?
                    await _agcasinoBalanceDataAccess.GetAgentBalanceDetail(metaModel, userCurrencyList).ConfigureAwait(false) :
                    await _agcasinoBalanceDataAccess.GetCustomerBalanceDetail(metaModel, userCurrencyList).ConfigureAwait(false);
            }
        }

        private async Task<dynamic> ProcessRequest(ThirdPartyWinLossMetaModel metaModel)
        {
            if (IsValidRequest(metaModel.CurrencyId, metaModel.DrillDownCurrencyId))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                var result = await GetData(metaModel).ConfigureAwait(false);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result.DataList, result.SummaryTotal);
                }

                if (result.DataList.Any())
                {
                    return new
                    {
                        TotalRows = result.TotalRows,
                        Data = result.DataList,
                        Total = CalculateTotal(result.DataList),
                        SummaryTotal = result.SummaryTotal
                    };
                }
            }

            return GetAgCasinoBalanceEmptyResult();
        }
    }
}