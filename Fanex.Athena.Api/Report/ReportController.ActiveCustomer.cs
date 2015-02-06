// --------------------------------------------------------------------------
// <copyright file="ReportController.ActiveCustomer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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
        /// Active customer.
        /// </summary>
        /// <param name="metaModel">Meta model data.</param>
        /// <returns>Process Request.</returns>
        [HttpPost]
        public dynamic ActiveCustomer(ActiveCustomerMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        #endregion Action

        private static dynamic GetEmptyResult(ActiveCustomerMetaModel metaModel)
        {
            return new
            {
                Data = new List<ActiveCustomerGridViewModel>(),
                GrantTotalCustomer = 0,
                ProductOptions = metaModel.ProductOptions.ToJson()                
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
        private static SortableList<ActiveCustomer> SortData(SortableList<ActiveCustomer> dataList, string sortBy = null, string sortOrder = "asc")
        {
            if (sortBy != null)
            {
                // Sort for grouping columns, get the real column name
                if (sortBy.EndsWith("Single", StringComparison.OrdinalIgnoreCase))
                {
                    sortBy = sortBy.Remove(sortBy.LastIndexOf("Single", StringComparison.OrdinalIgnoreCase));
                }

                var property = typeof(ActiveCustomer).GetProperty(sortBy);

                if (property == null)
                {
                    // The two last characters is product code.
                    var indexProductCode = sortBy.Length - 2;
                    var propertyName = sortBy.Substring(0, indexProductCode);
                    var productCode = sortBy.Substring(indexProductCode);

                    dataList.SortByDictionary<string, int>(productCode, propertyName, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
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
        /// <returns>Licensee WinLoss Grid View Model.</returns>
        private static ActiveCustomerGridViewModel MapToGridView(ActiveCustomer item)
        {
            return new ActiveCustomerGridViewModel
            {
                TotalCustomer = item.TotalCustomer,
                TransactionDate = item.TransactionDate,
                CustomerCountBA = item.CustomerCount.Keys.Contains(Product.BA) ? item.CustomerCount[Product.BA] : 0,
                CustomerCountSB = item.CustomerCount.Keys.Contains(Product.SB) ? item.CustomerCount[Product.SB] : 0,
                CustomerCountRB = item.CustomerCount.Keys.Contains(Product.RB) ? item.CustomerCount[Product.RB] : 0,
                CustomerCountNG = item.CustomerCount.Keys.Contains(Product.NG) ? item.CustomerCount[Product.NG] : 0,
                CustomerCountLC = item.CustomerCount.Keys.Contains(Product.LC) ? item.CustomerCount[Product.LC] : 0,
                CustomerCountVS = item.CustomerCount.Keys.Contains(Product.VS) ? item.CustomerCount[Product.VS] : 0,
                CustomerCountCS = item.CustomerCount.Keys.Contains(Product.CS) ? item.CustomerCount[Product.CS] : 0,
                CustomerCountBG = item.CustomerCount.Keys.Contains(Product.BG) ? item.CustomerCount[Product.BG] : 0,
                CustomerCountKN = item.CustomerCount.Keys.Contains(Product.KN) ? item.CustomerCount[Product.KN] : 0,
            };
        }

        /// <summary>
        /// Active customer - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Data list.</param>
        /// <param name="totalCustomer">Summary data.</param>
        /// <returns>Http Response Message.</returns>
        private HttpResponseMessage ExportExcel(ActiveCustomerMetaModel metaModel, IList<ActiveCustomer> dataList, int totalCustomer)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "ActiveCustomer.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/ActiveCustomer.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var currencyList = userSession.CurrencyList;
            var showingOption = new ShowingOption
            {
                BAVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.BA) && productAccess.IsAllowBA,
                RBVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.RB) && productAccess.IsAllowRB,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var currencyName = currencyList.SingleOrDefault(i => i.Value == metaModel.CurrencyId);
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Key
            };

            var exccelModel = new ActiveCustomerViewModel
            {
                MainResult = dataList,
                TotalCustomer = totalCustomer,
            };

            var excel = new ActiveCustomerExcel(templatePath, FileName, convertingOption, showingOption, metaModel, exccelModel);
            
            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        /// <summary>
        /// Get data for Active Customer report.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter: Data List.</param>
        /// <param name="totalCustomer">Output parameter total customer.</param>
        private void GetData(
                ActiveCustomerMetaModel metaModel,
                out SortableList<ActiveCustomer> dataList,
                out int totalCustomer)
        {            
             dataList = _activeCustomerService.GetActiveCustomers(metaModel, out totalCustomer);            
        }

        /// <summary>
        ///  Process request.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Dynamic object.</returns>
        private dynamic ProcessRequest(
            ActiveCustomerMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId; // focus here
                
                int totalCustomer;
                SortableList<ActiveCustomer> dataList;
                GetData(metaModel, out dataList, out totalCustomer);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, totalCustomer);
                }

                if (dataList != null && dataList.Count > 0)
                {
                    dataList = SortData(dataList, metaModel.SortBy, metaModel.SortOrder);

                    return new
                    {
                        Data = dataList.Select(MapToGridView).ToList(),
                        GrantTotalCustomer = totalCustomer,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
                }
            }

            return GetEmptyResult(metaModel);
        }

        #endregion Private Utility
    }
}