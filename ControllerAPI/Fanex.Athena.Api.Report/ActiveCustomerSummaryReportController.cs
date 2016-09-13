// --------------------------------------------------------------------------
// <copyright file="ActiveCustomerSummaryReportController.cs" company="">
// Copyright (c) . All rights reserved.
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
    using Base.Attributes;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportActiveCustomerSummary)]
    public class ActiveCustomerSummaryReportController : ApiReportBaseController
    {
        private readonly IActiveCustomerService _activeCustomerService;

        public ActiveCustomerSummaryReportController()
        {
            _activeCustomerService = new ActiveCustomerService();
        }

        [HttpPost]
        public dynamic Index(ActiveCustomerMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

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
                CustomerCountPTCS = item.CustomerCount.Keys.Contains(Product.PTCS) ? item.CustomerCount[Product.PTCS] : 0,
                CustomerCountAGC = item.CustomerCount.Keys.Contains(Product.AGC) ? item.CustomerCount[Product.AGC] : 0,
                CustomerCountMGCLC = item.CustomerCount.Keys.Contains(Product.MGCLC) ? item.CustomerCount[Product.MGCLC] : 0,
                CustomerCountMGCRNG = item.CustomerCount.Keys.Contains(Product.MGCRNG) ? item.CustomerCount[Product.MGCRNG] : 0,
                CustomerCountBT = item.CustomerCount.Keys.Contains(Product.BT) ? item.CustomerCount[Product.BT] : 0,
                CustomerCountVB = item.CustomerCount.Keys.Contains(Product.VB) ? item.CustomerCount[Product.VB] : 0,
                CustomerCountAB = item.CustomerCount.Keys.Contains(Product.AB) ? item.CustomerCount[Product.AB] : 0
            };
        }

        private HttpResponseMessage ExportExcel(ActiveCustomerMetaModel metaModel, IList<ActiveCustomer> dataList, int totalCustomer)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "ActiveCustomerSummary.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/ActiveCustomerSummary.xls");
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

            var excel = new ActiveCustomerSummaryExcel(templatePath, FileName, convertingOption, showingOption, metaModel, exccelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private dynamic ProcessRequest(
            ActiveCustomerMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;

                int totalCustomer;
                var dataList = _activeCustomerService.GetActiveCustomers(metaModel, out totalCustomer);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, totalCustomer);
                }

                if (dataList.Any())
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