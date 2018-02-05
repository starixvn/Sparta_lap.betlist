// -----------------------------------------------------------------------
// <copyright file="ReportController.MicroGamingWinLoss.cs" company="Microsoft">
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
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportMicrogamingWinloss)]
    public class MicrogamingWinlossReportController : ApiReportBaseController
    {
        private readonly IMicroGamingWinLossDataAccess _microGamingWinLossDataAccess;

        public MicrogamingWinlossReportController()
        {
            _microGamingWinLossDataAccess = new MicroGamingWinLossDataAccess();
        }

        public MicrogamingWinlossReportController(
            ISessionService sessionService,
            IProductService productService,
            IMicroGamingWinLossDataAccess microGamingWinLossDataAccess)
            : base(sessionService, productService)
        {
            _microGamingWinLossDataAccess = microGamingWinLossDataAccess;
        }

        [HttpPost]
        public dynamic Index(ThirdPartyWinLossMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludedTestData);

            var result = _microGamingWinLossDataAccess.GetSummaryWinloss(metaModel);

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel, result.DataList, result.SummaryTotal);
            }

            if (result.DataList.Any())
            {
                return new
                {
                    Data = DoRestructureData(result.DataList.GroupBy(i => new { i.TypeID, i.Date })),
                    Total = CalculateTotal(result.DataList),
                    SummaryTotal = result.SummaryTotal
                };
            }

            return GetEmptyMicroGamingWinLossResult();
        }

        private static IList<MicroGamingWinLoss> DoRestructureData(dynamic groupData)
        {
            var newDataList = new List<MicroGamingWinLoss>();

            var index = 0;
            foreach (var item in groupData)
            {
                var rowSpanFlag = true;
                var temps = (IList<MicroGamingWinLoss>)item;
                /*processing index value*/
                var numOfItem = temps.Count();

                foreach (var col in temps)
                {
                    if (rowSpanFlag && numOfItem > 1)
                    {
                        index++;
                        col.RowSpan = numOfItem;
                        rowSpanFlag = false;
                    }
                    else if (numOfItem > 1)
                    {
                        col.RowSpan = -1;
                    }
                    else
                    {
                        index++;
                        col.RowSpan = 0;
                    }

                    col.Index = index;
                    newDataList.Add(col);
                }
            }

            return newDataList;
        }

        private static dynamic GetEmptyMicroGamingWinLossResult()
        {
            return new
            {
                Data = new List<ThirdPartyWinLoss>(),
                Total = new List<ThirdPartyWinLoss>(),
                SummaryTotal = new Dictionary<string, ThirdPartyWinLoss>()
            };
        }

        private static IEnumerable<MicroGamingWinLoss> CalculateTotal(IEnumerable<MicroGamingWinLoss> dataList)
        {
            return dataList.GroupBy(d => d.CurrencyName).Select(g => new MicroGamingWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                ProgressiveContribution = g.Sum(p => p.ProgressiveContribution),
                ProgressivePayout = g.Sum(p => p.ProgressivePayout),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                LicenseeNetWin = g.Sum(p => p.LicenseeNetWin),
                AmountDueToMGC = g.Sum(p => p.AmountDueToMGC),
                ProgressiveFee = g.Sum(p => p.ProgressiveFee)
            }).ToList();
        }

        [CLSCompliant(false)]
        protected internal virtual MicroGamingWinLossExcel CreateExcel(
            ThirdPartyWinLossMetaModel metaModel,
            string templatePath,
            ConvertingOption convertingOption,
            MicroGamingWinLossViewModel excelModel)
        {
            return new MicroGamingWinLossExcel(templatePath, convertingOption, metaModel, excelModel);
        }

        private HttpResponseMessage ExportExcel(
            ThirdPartyWinLossMetaModel metaModel,
            IList<MicroGamingWinLoss> dataList,
            IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/MicroGamingWinLoss.xls");
            var currency = userSession.CurrencyList.SingleOrDefault(i => i.Key == metaModel.CurrencyId);

            var exportedFileName = string.Format(
                "MGCWinLoss_{0:ddMMMyyyy}_{1}.xls",
                metaModel.FromDate,
                currency.Key == 0 ? "All" : currency.Value);
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currency.Value
            };

            var excelModel = new MicroGamingWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = CreateExcel(metaModel, templatePath, convertingOption, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }
    }
}