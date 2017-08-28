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
                Data = new List<ActiveCustomer>(),
                GrantTotalCustomer = 0,
                ProductOptions = metaModel.ProductOptions.ToJson()
            };
        }

        #region ProcessRequest

        private static SortableList<ActiveCustomer> SortData(SortableList<ActiveCustomer> dataList, string sortBy = null, string sortOrder = "asc")
        {
            if (sortBy != null)
            {
                var property = typeof(ActiveCustomer).GetProperty(sortBy);

                if (property == null)
                {
                    // The two last characters is product code.
                    var propertyName = nameof(ActiveCustomer.CustomerCount);
                    var productCode = sortBy.Substring(propertyName.Length);

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

            var currencyName = currencyList.SingleOrDefault(i => i.Key == metaModel.CurrencyId);
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Value
            };

            var excelModel = new ActiveCustomerViewModel
            {
                MainResult = dataList,
                TotalCustomer = totalCustomer,
            };

            var excel = new ActiveCustomerSummaryExcel(templatePath, convertingOption, showingOption, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private dynamic ProcessRequest(ActiveCustomerMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);

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
                        Data = dataList,
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