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
        public dynamic LicenseeWl(
            LicenseeWinLossMetaModel metaModel,
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
        public dynamic LicenseeWl([FromBody] LicenseeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        #endregion Action

        private static dynamic GetEmptyResult(LicenseeWinLossMetaModel metaModel)
        {
            return new
            {
                Data = new List<LicenseeWinLossGridViewModel>(),
                Total = new List<LicenseeWinLossGridViewModel>(),
                SummaryTotal = new Dictionary<string, LicenseeWinLossGridViewModel>(),
                ProductOptions = metaModel.ProductOptions.ToJson()
            };
        }

        #region ProcessRequest

        /// <summary>
        /// Licensee win loss - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Data list.</param>
        /// <param name="summaryTotal">Summary data.</param>
        /// <returns>Http Response Message.</returns>
        private HttpResponseMessage ExportExcel(LicenseeWinLossMetaModel metaModel, IList<LicenseeWinLoss> dataList, IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            var licenseeSession = SessionService.LicenseeSession;
            const string FileName = "LicenseeWinLoss.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/LicenseeWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);

            var currencyList = SessionService.UserSession.CurrencyList;
            var showingOption = new ShowingOption
            {
                BaVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.Ba) && productAccess.IsAllowBa,
                RbVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.Rb) && productAccess.IsAllowRb,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
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

            var exccelModel = new LicenseeWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var licenseeWinLoss = new LicenseeWinLossExcel(templatePath, FileName, convertingOption, showingOption, metaModel, exccelModel)
            {
                Level = metaModel.DrillDownLevel,
                EnableReportByMonth = metaModel.ReportBy
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
                LicenseeWinLossMetaModel metaModel,
                out SortableList<LicenseeWinLoss> dataList,
                out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            if (metaModel.DrillDownLevel == 0)
            {
                dataList = _licenseeWinLossService.GetSummaryWinloss(metaModel, out summaryTotal);
            }
            else
            {
                dataList = metaModel.LicenseeId > 1 || metaModel.DrillDownLevel == 2
                               ? _licenseeWinLossService.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                               : _licenseeWinLossService.GetAgentWinlossDetail(metaModel, out summaryTotal);
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
            LicenseeWinLossMetaModel metaModel,
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

                IDictionary<string, LicenseeWinLoss> summaryTotal;
                SortableList<LicenseeWinLoss> dataList;
                GetData(metaModel, out dataList, out summaryTotal);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, summaryTotal);
                }

                if (dataList != null && dataList.Count > 0)
                {
                    dataList = SortData(dataList, metaModel.SortBy, metaModel.SortOrder);

                    var summaryTotalView = summaryTotal.ToDictionary(d => d.Key, d => MapToGridView(d.Value));
                    return new
                    {
                        Data = dataList.Select(MapToGridView).ToList(),
                        Total = _licenseeWinLossService.TotalCalc(dataList).Select(MapToGridView).ToList(),
                        SummaryTotal = summaryTotalView,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
                }
            }

            return GetEmptyResult(metaModel);
        }

        /// <summary>
        /// Sort data list by property (column name).
        /// </summary>
        /// <param name="dataList">Data list to be sorted.</param>
        /// <param name="sortBy">Sort column name.</param>
        /// <param name="sortOrder">ASC or DESC.</param>
        /// <returns>Data list after being sorted.</returns>
        private SortableList<LicenseeWinLoss> SortData(SortableList<LicenseeWinLoss> dataList, string sortBy = null, string sortOrder = "asc")
        {
            if (sortBy != null)
            {
                // Sort for grouping columns, get the real column name
                if (sortBy.EndsWith("Single", StringComparison.OrdinalIgnoreCase))
                {
                    sortBy = sortBy.Remove(sortBy.LastIndexOf("Single"));
                }

                var property = typeof(LicenseeWinLoss).GetProperty(sortBy);

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
        /// Convert to view model of grid.
        /// </summary>
        /// <param name="item">The data object.</param>
        /// <param name="index">Row index.</param>
        /// <returns>Licensee WinLoss Grid View Model.</returns>
        private LicenseeWinLossGridViewModel MapToGridView(LicenseeWinLoss item, int index = 0)
        {
            return new LicenseeWinLossGridViewModel
            {
                Index = index + 1,
                AmtDueToBA = item.AmtDueToBA,
                AmtDueToRacing = item.AmtDueToRacing,
                BACommission = item.BACommission,
                CurrencyId = item.CurrencyId,
                SystemId = item.SystemId,
                CurrencyName = item.CurrencyName,
                CustomerComm = item.CustomerComm,
                CustomerWinLoss = item.CustomerWinLoss,
                TotalTransaction = item.TotalTransaction,
                RacingRevenue = item.RacingRevenue,
                CustomerId = item.CustomerId,
                Date = item.Date,
                ExternalId = item.ExternalId,
                LicenseeWLBA = item.LicenseeWL.Keys.Contains(Product.Ba) ? item.LicenseeWL[Product.Ba] : 0,
                LicenseeWLSB = item.LicenseeWL.Keys.Contains(Product.Sb) ? item.LicenseeWL[Product.Sb] : 0,
                LicenseeWLRB = item.LicenseeWL.Keys.Contains(Product.Rb) ? item.LicenseeWL[Product.Rb] : 0,
                LicenseeWLNG = item.LicenseeWL.Keys.Contains(Product.Ng) ? item.LicenseeWL[Product.Ng] : 0,
                LicenseeWLLC = item.LicenseeWL.Keys.Contains(Product.Lc) ? item.LicenseeWL[Product.Lc] : 0,
                LicenseeWLVS = item.LicenseeWL.Keys.Contains(Product.Vs) ? item.LicenseeWL[Product.Vs] : 0,
                LicenseeWLBG = item.LicenseeWL.Keys.Contains(Product.Bg) ? item.LicenseeWL[Product.Bg] : 0,
                LicenseeWLCS = item.LicenseeWL.Keys.Contains(Product.Cs) ? item.LicenseeWL[Product.Cs] : 0,
                LicenseeWLKN = item.LicenseeWL.Keys.Contains(Product.Kn) ? item.LicenseeWL[Product.Kn] : 0,
                TurnOverBA = item.TurnOver.Keys.Contains(Product.Ba) ? item.TurnOver[Product.Ba] : 0,
                TurnOverSB = item.TurnOver.Keys.Contains(Product.Sb) ? item.TurnOver[Product.Sb] : 0,
                TurnOverRB = item.TurnOver.Keys.Contains(Product.Rb) ? item.TurnOver[Product.Rb] : 0,
                TurnOverNG = item.TurnOver.Keys.Contains(Product.Ng) ? item.TurnOver[Product.Ng] : 0,
                TurnOverLC = item.TurnOver.Keys.Contains(Product.Lc) ? item.TurnOver[Product.Lc] : 0,
                TurnOverVS = item.TurnOver.Keys.Contains(Product.Vs) ? item.TurnOver[Product.Vs] : 0,
                TurnOverCS = item.TurnOver.Keys.Contains(Product.Cs) ? item.TurnOver[Product.Cs] : 0,
                TurnOverBG = item.TurnOver.Keys.Contains(Product.Bg) ? item.TurnOver[Product.Bg] : 0,
                TurnOverKN = item.TurnOver.Keys.Contains(Product.Kn) ? item.TurnOver[Product.Kn] : 0,
            };
        }

        #endregion Private Utility
    }
}