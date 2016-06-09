// --------------------------------------------------------------------------
// <copyright file="ReportController.AgCasinoBalance.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Service.Implement.Report;
    using Service.Interface.Report;
    using Web.ActionFilter;

    public class AgCasinoBalanceReportController : ApiReportBaseController
    {
        private readonly IAgCasinoBalanceService _agcasinoBalanceService;

        public AgCasinoBalanceReportController()
        {
            _agcasinoBalanceService = new AgCasinoBalanceService();
        }

        [HttpPost]
        [ApiFunctionAuthorization]
        public dynamic AgCasinoBalance(AgCasinoBalanceMetaModel metaModel)
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

        #region Private Utility

        /// <summary>
        /// AGCasino win loss - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Data list.</param>
        /// <param name="summaryTotal">Summary data.</param>
        /// <returns>Http Response Message.</returns>
        private HttpResponseMessage ExportExcel(AgCasinoBalanceMetaModel metaModel, IList<AgCasinoBalance> dataList, IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            const string FileName = "AgCasinoBalance.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/AgCasinoBalance.xls");

            var currencyList = userSession.CurrencyList;

            var currencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Key
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

            var excel = new AgCasinoBalanceExcel(templatePath, FileName, convertingOption, metaModel, excelModel)
            {
                Level = metaModel.DrillDownLevel,
            };

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        /// <summary>
        /// Get data for Licensee Win Loss report.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter: Data List.</param>
        /// <param name="summaryTotal">Output parameter summary data.</param>
        /// <param name="totalRows">Total rows.</param>
        private void GetData(
                AgCasinoBalanceMetaModel metaModel,
                out SortableList<AgCasinoBalance> dataList,
                out IDictionary<string, AgCasinoBalance> summaryTotal,
                out int totalRows)
        {
            totalRows = 0;
            if (metaModel.DrillDownLevel == 0)
            {
                dataList = _agcasinoBalanceService.GetSummaryBalance(metaModel, out summaryTotal);
            }
            else
            {
                dataList = metaModel.DrillDownLevel == 2
                               ? _agcasinoBalanceService.GetCustomerBalanceDetail(metaModel, out summaryTotal, out totalRows)
                               : _agcasinoBalanceService.GetAgentBalanceDetail(metaModel, out summaryTotal);
            }
        }

        /// <summary>
        ///  Process request.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Dynamic object.</returns>
        private dynamic ProcessRequest(AgCasinoBalanceMetaModel metaModel)
        {
            if (IsValidRequest(metaModel, metaModel.DrillDownCurrencyId))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;

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

        #endregion Private Utility
    }
}