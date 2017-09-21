// --------------------------------------------------------------------------
// <copyright file="WinLossByProductReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Excel.Report.Models;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.MetaModel.Product;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByProduct)]
    public class WinLossByProductReportController : ApiReportBaseController
    {
        private readonly IAppSettings _appSettings;

        private readonly IWinLossByProductDataAccess _winlossByProductDataAccess;

        public WinLossByProductReportController()
        {
            _appSettings = new AppSettings();
            _winlossByProductDataAccess = new WinLossByProductDataAccess();
        }

        public WinLossByProductReportController(
            ISessionService sessionService,
            IProductService productService,
            IAppSettings appSettings,
            IWinLossByProductDataAccess winlossByProductDataAccess)
            : base(sessionService, productService)
        {
            _appSettings = appSettings;
            _winlossByProductDataAccess = winlossByProductDataAccess;
        }

        [HttpPost]
        public dynamic Index(
           WinLossByProductMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0,
            int memberId = 0,
            string systemId = null,
            string currencyName = null)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId, memberId, systemId, currencyName);
        }

        [HttpPost]
        public dynamic Index([FromBody] WinLossByProductMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        [CLSCompliant(false)]
        protected internal virtual WinlossByProductExcel CreateExcel(string templatePath, WinLossByProductExcelModel excelModel)
        {
            return new WinlossByProductExcel(templatePath, excelModel);
        }

        #region Private Utility

        /// <summary>
        /// Restructure data.
        /// </summary>
        /// <param name="dataList">The data collection.</param>
        /// <param name="level">The Level value.</param>
        /// <returns>Return the grid view data collection.</returns>
        private static IEnumerable<WinLossByProduct> WinLossByProductRestructureData(IEnumerable<WinLossByProduct> dataList, int level)
        {
            // Calculate rowspan
            return level == 0
                ? DoRestructureData(dataList.GroupBy(i => new { i.Date, i.CurrencyName }))
                : DoRestructureData(dataList.GroupBy(i => new { i.SystemId, i.CurrencyName }));
        }

        #endregion Private Utility

        private static dynamic GetEmptyResult(IEnumerable<ProductOption> productOptions, int memberId, string systemId, string errorMessage)
        {
            return new
            {
                Data = new List<WinLossByProduct>(),
                Total = new List<WinLossByProduct>(),
                ProductOptions = productOptions.ToJson(),
                MemberId = memberId,
                SystemId = systemId,
                ErrorMessage = errorMessage
            };
        }

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

        private static string GetErrorMessage(int code)
        {
            switch (code)
            {
                case 100:
                    return ViewResources.NoCustomerFoundMsg;

                case 101:
                    return ViewResources.MoreThanOneCustomerFoundMsg;

                default:
                    return string.Empty;
            }
        }

        #region ProcessRequest

        private HttpResponseMessage ExportExcel(
            WinLossByProductMetaModel metaModel,
            IEnumerable<WinLossByProduct> dataList,
            IEnumerable<WinLossByProduct> totalData,
            IDictionary<string, IEnumerable<WinLossByProduct>> summaryTotal,
            string systemId,
            string errorMessage)
        {
            const string FileName = "WinLossByProduct.xls";
            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/WinLossByProducts.xls");
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            metaModel.FromDate = metaModel.Level == 0 ? metaModel.FromDate : metaModel.DrillDownFromDate;
            metaModel.ToDate = metaModel.Level == 0 ? metaModel.ToDate : metaModel.DrillDownToDate;

            var convertingOption = new ConvertingOption
            {
                IsConvert = false,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.Level == 0 ? metaModel.CurrencyName : metaModel.DrillDownCurrencyName
            };

            var excelModel = new WinLossByProductExcelModel
            {
                ConvertingOption = convertingOption,
                DataList = dataList,
                ErrorMessage = errorMessage,
                MetaModel = metaModel,
                SummaryTotal = summaryTotal,
                SystemId = systemId,
                TotalData = totalData,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                SystemIdVisible = !_appSettings.FakeLicenseeList.Contains(licenseeSession.LicenseeId)
            };

            var winlossByProduct = CreateExcel(templatePath, excelModel);
            winlossByProduct.Export();

            return ExcelStream(winlossByProduct.Workbook, FileName);
        }

        private void GetData(
            WinLossByProductMetaModel metaModel,
            out IEnumerable<WinLossByProduct> dataList,
            out IEnumerable<WinLossByProduct> totalData,
            out IDictionary<string, IEnumerable<WinLossByProduct>> summaryTotal,
            out IResultInfo resultInfo)
        {
            if (!metaModel.IsValid)
            {
                dataList = new List<WinLossByProduct>();
                totalData = new List<WinLossByProduct>();
                summaryTotal = new Dictionary<string, IEnumerable<WinLossByProduct>>();
                resultInfo = new ResultInfo();
                return;
            }

            if (metaModel.Level == 0)
            {
                dataList = _winlossByProductDataAccess.GetWinlossByProducts(metaModel, out totalData, out summaryTotal, out resultInfo);
            }
            else
            {
                resultInfo = new ResultInfo();
                dataList = SessionService.LicenseeSession.LicenseeId > 1 || metaModel.Level == 2
                               ? _winlossByProductDataAccess.GetWinlossByProductCustomers(metaModel, out totalData, out summaryTotal)
                               : _winlossByProductDataAccess.GetWinlossByProductAgents(metaModel, out totalData, out summaryTotal);
            }
        }

        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private dynamic ProcessRequest(
            WinLossByProductMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0,
            int memberId = 0,
            string systemId = null,
            string currencyName = null)
        {
            if (!IsValidRequest(metaModel, currencyId))
            {
                return BadRequest(ViewResources.InvalidParams);
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(level == 0 ? metaModel.CurrencyId : currencyId, metaModel.IsExcludedTestData);
            metaModel.Username = !string.IsNullOrWhiteSpace(metaModel.Username)
                                && metaModel.Username.Equals(ViewResources.ExternalIdOrSystemId, StringComparison.OrdinalIgnoreCase)
                                ? string.Empty : metaModel.Username;

            metaModel.DrillDownCurrencyId = currencyId;
            metaModel.DrillDownCurrencyName = currencyName;
            metaModel.Level = level;
            metaModel.CustomerId = custId;
            if (metaModel.MemberId == 0)
            {
                metaModel.MemberId = memberId;
            }

            if (fdate.HasValue)
            {
                metaModel.DrillDownFromDate = fdate.Value;
            }

            if (tdate.HasValue)
            {
                metaModel.DrillDownToDate = tdate.Value;
            }

            IDictionary<string, IEnumerable<WinLossByProduct>> summaryTotal;
            IEnumerable<WinLossByProduct> dataList;
            IEnumerable<WinLossByProduct> totalData;
            IResultInfo resultInfo;

            GetData(metaModel, out dataList, out totalData, out summaryTotal, out resultInfo);

            resultInfo.Message = GetErrorMessage(resultInfo.Code);

            if (memberId == 0 && !string.IsNullOrWhiteSpace(resultInfo.ExtendedData))
            {
                var extendedItems = resultInfo.ExtendedData.Split(';');
                memberId = string.IsNullOrWhiteSpace(extendedItems[0]) ? 0 : Convert.ToInt32(extendedItems[0]);
                systemId = extendedItems[1];
                metaModel.MemberId = memberId;
            }

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel, dataList, totalData, summaryTotal, systemId, resultInfo.Message);
            }

            return dataList.Any()
                ? new
                {
                    Data = WinLossByProductRestructureData(dataList, level),
                    Total = totalData,
                    SummaryTotal = summaryTotal,
                    ProductOptions = metaModel.ProductOptions.ToJson(),
                    MemberId = memberId,
                    SystemId = systemId,
                    ErrorMessage = resultInfo.Message
                }
                : GetEmptyResult(metaModel.ProductOptions, memberId, systemId, resultInfo.Message);
        }

        #endregion ProcessRequest
    }
}