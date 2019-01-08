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
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Report.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportActiveCustomerSummary)]
    public class ActiveCustomerSummaryReportController : ApiReportBaseController
    {
        private readonly IActiveCustomerSummaryDataAccess _activeCustomerSummaryDataAccess;

        public ActiveCustomerSummaryReportController(
            IActiveCustomerSummaryDataAccess activeCustomerSummaryDataAccess,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _activeCustomerSummaryDataAccess = activeCustomerSummaryDataAccess;
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

        private static void SortData(SortableList<ActiveCustomer> dataList, string sortBy, string sortOrder = "asc")
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return;
            }

            var property = typeof(ActiveCustomer).GetProperty(sortBy);

            if (property == null)
            {
                // The two last characters is product code.
                var propertyName = nameof(ActiveCustomer.CustomerCount);
                var productCode = sortBy.Substring(propertyName.Length);

                dataList.SortByDictionary(productCode, propertyName, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                dataList.Sort(sortBy, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
            }
        }

        protected internal virtual ActiveCustomerSummaryExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ShowingOption showingOption,
            ActiveCustomerMetaModel metaModel,
            ActiveCustomerViewModel excelModel)
        {
            return new ActiveCustomerSummaryExcel(templatePath, convertingOption, showingOption, metaModel, excelModel);
        }

        private HttpResponseMessage ExportExcel(ActiveCustomerMetaModel metaModel, ActiveCustomerSummaryResultModel resultModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "ActiveCustomerSummary.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/ActiveCustomerSummary.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var currencyList = userSession.CurrencyList;
            var showingOption = new ShowingOption
            {
                BAVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.BA) && productAccess.IsAllowBA,
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
                MainResult = resultModel.DataList,
                TotalCustomer = resultModel.TotalCustomer,
            };

            var excel = CreateExcel(templatePath, convertingOption, showingOption, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private dynamic ProcessRequest(ActiveCustomerMetaModel metaModel)
        {
            if (IsValidRequest(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);

                var result = metaModel.IsValid ?
                    _activeCustomerSummaryDataAccess.GetActiveCustomers(metaModel) :
                    new ActiveCustomerSummaryResultModel
                    {
                        DataList = new SortableList<ActiveCustomer>(),
                        TotalCustomer = 0
                    };

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result);
                }

                if (result.DataList.Any())
                {
                    SortData(result.DataList, metaModel.SortBy, metaModel.SortOrder);

                    return new
                    {
                        Data = result.DataList,
                        GrantTotalCustomer = result.TotalCustomer,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
                }
            }

            return GetEmptyResult(metaModel);
        }
    }
}