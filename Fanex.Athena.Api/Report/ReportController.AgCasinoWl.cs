// --------------------------------------------------------------------------
// <copyright file="ReportController.LicenseeWl.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;

    /// <summary>
    /// ReportController class.
    /// </summary>
    public partial class ReportController
    {
        #region Action

        /// <summary>
        /// Licensee win loss report for down line level.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="fdate">From Date value.</param>
        /// <param name="tdate">To Date value.</param>
        /// <param name="level">Level value.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Call method ProcessRequest.</returns>
        [HttpPost]
        public dynamic AgCasinoWL(
            AgCasinoWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        /// <summary>
        /// Licensee Win Loss - Level 0.
        /// </summary>
        /// <param name="metaModel">The level 0 filter.</param>
        /// <returns>Process Request.</returns>
        [HttpPost]
        public dynamic AgCasinoWL([FromBody] AgCasinoWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        #endregion Action

        private static dynamic GetEmptyResult()
        {
            return new
            {
                Data = new List<AgCasinoWinLoss>(),
                Total = new List<AgCasinoWinLoss>(),
                SummaryTotal = new Dictionary<string, AgCasinoWinLoss>()
            };
        }

        #region ProcessRequest

        /// <summary>
        /// Sort data list by property (column name).
        /// </summary>
        /// <param name="dataList">Data list to be sorted.</param>
        /// <param name="sortBy">Sort column name.</param>
        /// <param name="sortOrder">ASC or DESC.</param>
        /// <returns>Data list after being sorted.</returns>
        private static SortableList<AgCasinoWinLoss> SortData(SortableList<AgCasinoWinLoss> dataList, string sortBy = null, string sortOrder = "asc")
        {
            if (sortBy != null)
            {
                // Sort for grouping columns, get the real column name
                if (sortBy.EndsWith("Single", StringComparison.OrdinalIgnoreCase))
                {
                    sortBy = sortBy.Remove(sortBy.LastIndexOf("Single", StringComparison.OrdinalIgnoreCase));
                }

                var property = typeof(AgCasinoWinLoss).GetProperty(sortBy);

                if (property == null)
                {
                    // The two last characters is product code.
                    var indexProductCode = sortBy.Length - 2;
                    var propertyName = sortBy.Substring(0, indexProductCode);
                    var productCode = sortBy.Substring(indexProductCode);

                    dataList.SortByDictionary<string, decimal>(productCode, propertyName, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    dataList.Sort(sortBy, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
                }
            }

            return dataList;
        }

        #endregion ProcessRequest

        #region Private Utility

        /// <summary>
        /// Licensee win loss - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Data list.</param>
        /// <param name="summaryTotal">Summary data.</param>
        /// <returns>Http Response Message.</returns>
        private HttpResponseMessage ExportExcel(AgCasinoWinLossMetaModel metaModel, IList<AgCasinoWinLoss> dataList, IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "AgCasinoWinLoss.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/AgCasinoWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var currencyList = userSession.CurrencyList;
            var showingOption = new ShowingOption
            {
                BAVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.BA) && productAccess.IsAllowBA,
                RBVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.RB) && productAccess.IsAllowRB,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

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

            var exccelModel = new AgCasinoWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var licenseeWinLoss = new AgCasinoWinLossExcel(templatePath, FileName, convertingOption, showingOption, metaModel, exccelModel)
            {
                Level = metaModel.DrillDownLevel,
            };

            licenseeWinLoss.Export();

            return ExcelStream(licenseeWinLoss.Workbook, FileName);
        }

        /// <summary>
        /// Get data for Licensee Win Loss report.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter: Data List.</param>
        /// <param name="summaryTotal">Output parameter summary data.</param>
        private void GetData(
                AgCasinoWinLossMetaModel metaModel,
                out SortableList<AgCasinoWinLoss> dataList,
                out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            if (metaModel.DrillDownLevel == 0)
            {
                dataList = _agcasinoWinLossService.GetSummaryWinloss(metaModel, out summaryTotal);
            }
            else
            {
                dataList = metaModel.LicenseeId > 1 || metaModel.DrillDownLevel == 2
                               ? _agcasinoWinLossService.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                               : _agcasinoWinLossService.GetAgentWinlossDetail(metaModel, out summaryTotal);
            }
        }

        /// <summary>
        ///  Process request.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="fdate">From Date value.</param>
        /// <param name="tdate">To Date value.</param>
        /// <param name="level">The Level value.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Dynamic object.</returns>
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
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId; // focus here
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;

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
                        SummaryTotal = summaryTotal,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
                }
            }

            return GetEmptyResult();
        }

        #endregion Private Utility
    }
}