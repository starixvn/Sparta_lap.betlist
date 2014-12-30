// --------------------------------------------------------------------------
// <copyright file="ReportController.WinLostByProduct.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;

    /// <summary>
    /// WinLossByProductController class.
    /// </summary>
    public partial class ReportController
    {
        #region Action

        /// <summary>
        /// Win Loss By Product - Drill down.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="fdate">From Date value.</param>
        /// <param name="tdate">To Date value.</param>
        /// <param name="level">The Level value.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Call method ProcessRequest.</returns>
        [HttpPost]
        public dynamic WinLostByProduct(WinLossByProductMetaModel metaModel, DateTime? fdate, DateTime? tdate, int level = 0, int currencyId = 0, int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        /// <summary>
        /// Win Loss By Product - Level 0.
        /// </summary>
        /// <param name="metaModel">The level 0 MetaModel.</param>
        /// <returns>Process Request.</returns>
        [HttpPost]
        public dynamic WinLostByProduct([FromBody] WinLossByProductMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        #endregion Action

        #region Private Utility

        /// <summary>
        /// Restructure data.
        /// </summary>
        /// <param name="dataList">The data collection.</param>
        /// <param name="level">The Level value.</param>
        /// <returns>Return the grid view data collection.</returns>
        private static IList<WinLossByProduct> WinLossByProductRestructureData(IList<WinLossByProduct> dataList, int level)
        {
            // Calculate rowspan
            return level == 0
                ? DoRestructureData(dataList.GroupBy(i => new { i.Date, i.CurrencyName }))
                : DoRestructureData(dataList.GroupBy(i => new { i.SystemId, i.CurrencyName }));
        }

        #endregion Private Utility

        private static dynamic GetEmptyResult(WinLossByProductMetaModel metaModel)
        {
            return new
            {
                Data = new List<WinLossByProduct>(),
                Total = new List<WinLossByProduct>(),
                ProductOptions = metaModel.ProductOptions.ToJson()
            };
        }

        /// <summary>
        /// Restructure data.
        /// </summary>
        /// <param name="groupData">The grouping data.</param>
        /// <returns>Restructure data list.</returns>
        private static IList<WinLossByProduct> DoRestructureData(dynamic groupData)
        {
            var newDataList = new List<WinLossByProduct>();

            var index = 0;
            foreach (var item in groupData)
            {
                var rowSpanFlag = true;
                var temps = (IList<WinLossByProduct>)item;
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

        #region ProcessRequest

        /// <summary>
        /// Win Loss By Product - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter - report data list.</param>
        /// <param name="summaryTotal">Output parameter - summary data.</param>
        /// <returns>Http Response Message.</returns>
        private HttpResponseMessage ExportExcel(
            WinLossByProductMetaModel metaModel,
            IList<WinLossByProduct> dataList,
            IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            const string FileName = "WinLossByProduct.xls";
            var templatePath = HttpContext.Current.Server.MapPath("~/App_Data/Template/Reports/WinLossByProducts.xls");
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            metaModel.FromDate = metaModel.Level == 0 ? metaModel.FromDate : metaModel.DrillDownFromDate;
            metaModel.ToDate = metaModel.Level == 0 ? metaModel.ToDate : metaModel.DrillDownToDate;

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                LCVisible = productAccess.IsAllowLC,
                SBVisible = productAccess.IsAllowSB,
                VSVisible = productAccess.IsAllowVS,
                KNVisible = productAccess.IsAllowKN,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var currencyList = userSession.CurrencyList;
            var currencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.Level == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));

            var convertingOption = new ConvertingOption
            {
                IsConvert = false,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Key
            };

            var viewModel = new WinLossByProductViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal
            };

            var winlossByProduct = new WinlossByProductExcel(templatePath, FileName, convertingOption, showingOption, metaModel, viewModel)
            {
                Level = metaModel.Level,
                EnableReportByMonth = metaModel.ReportBy,
                ProductOptions = metaModel.ProductOptions
            };
            winlossByProduct.Export();

            return ExcelStream(winlossByProduct.Workbook, FileName);
        }

        /// <summary>
        /// Win Loss By Product - Get report data.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter - report data list.</param>
        /// <param name="summaryTotal">Output parameter - summary data.</param>
        private void GetData(WinLossByProductMetaModel metaModel, out IList<WinLossByProduct> dataList, out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var licenseeId = licenseeSession.LicenseeId;
            metaModel.LicenseeId = licenseeId;

            if (metaModel.Level == 0)
            {
                dataList = _winlossByProductService.GetWinlossByProducts(metaModel, out summaryTotal);
            }
            else
            {
                dataList = licenseeId > 1 || metaModel.Level == 2
                               ? _winlossByProductService.GetWinlossByProductCustomers(metaModel, out summaryTotal)
                               : _winlossByProductService.GetWinlossByProductAgents(metaModel, out summaryTotal);
            }
        }

        /// <summary>
        /// Check if the request is valid or not.
        /// Check if the session is valid.
        /// Check if currency and drill down currency is valid.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <returns>Request is valid or not.</returns>
        private bool IsValidRequest(WinLossByProductMetaModel metaModel, int currencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            var validModel = (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= -2)
                             || currencyList.Values.Contains(metaModel.CurrencyId);
            var validCurrency = (currencyId <= 0 && currencyId >= -2)
                                 || currencyList.Values.Contains(currencyId);

            return validModel && validCurrency;
        }

        /// <summary>
        ///  Process request.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="fdate">From Date value.</param>
        /// <param name="tdate">To Date value.</param>
        /// <param name="level">The Level value.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Dynamic type.</returns>
        private dynamic ProcessRequest(
            WinLossByProductMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {            
            if (!IsValidRequest(metaModel, currencyId))
            {
                RedirectToErrorPage();
            }

            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.Username = !string.IsNullOrWhiteSpace(metaModel.Username) 
                                && metaModel.Username.Equals(ViewResources.ExternalIdOrSystemId, StringComparison.OrdinalIgnoreCase) 
                                ? string.Empty : metaModel.Username;

            metaModel.DrillDownCurrencyId = currencyId;
            metaModel.Level = level;
            metaModel.CustomerId = custId;
            if (fdate.HasValue)
            {
                metaModel.DrillDownFromDate = fdate.Value;
            }

            if (tdate.HasValue)
            {
                metaModel.DrillDownToDate = tdate.Value;
            }

            IDictionary<string, IList<WinLossByProduct>> summaryTotal;
            IList<WinLossByProduct> dataList;

            GetData(metaModel, out dataList, out summaryTotal);
            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel, dataList, summaryTotal);
            }

            return dataList.Count > 0
                ? (dynamic)new
                {
                    Data = WinLossByProductRestructureData(dataList, level),
                    Total = _winlossByProductService.TotalCalc(dataList),
                    SummaryTotal = summaryTotal,
                    ProductOptions = metaModel.ProductOptions.ToJson()
                }
                : GetEmptyResult(metaModel);
        }

        #endregion ProcessRequest
    }
}