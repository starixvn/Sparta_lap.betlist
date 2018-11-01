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
    using Model.Report.ResultModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByProduct)]
    public class WinLossByProductReportController : ApiReportBaseController
    {
        private readonly IAppSettings _appSettings;

        private readonly IWinLossByProductDataAccess _winlossByProductDataAccess;

        public WinLossByProductReportController(
            ISessionService sessionService,
            IProductService productService,
            IAppSettings appSettings,
            ILanguageService langService,
            IWinLossByProductDataAccess winlossByProductDataAccess)
            : base(sessionService, productService, langService)
        {
            _appSettings = appSettings;
            _winlossByProductDataAccess = winlossByProductDataAccess;
        }

        [HttpPost]
#pragma warning disable S107 // Methods should not have too many parameters
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
#pragma warning restore S107 // Methods should not have too many parameters
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId, memberId, systemId, currencyName);
        }

        [HttpPost]
        public dynamic Index([FromBody] WinLossByProductMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        protected internal virtual WinlossByProductExcel CreateExcel(string templatePath, WinLossByProductExcelModel excelModel)
        {
            return new WinlossByProductExcel(templatePath, excelModel);
        }

        private static IEnumerable<WinLossByProduct> RestructureData(IEnumerable<WinLossByProduct> dataList, int level)
        {
            // Calculate rowspan
            return level == 0
                ? DoRestructureData(dataList.GroupBy(i => new { i.Date, i.CurrencyName }))
                : DoRestructureData(dataList.GroupBy(i => new { i.SystemId, i.CurrencyName }));
        }

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

        private static void AssignDrilldownDates(WinLossByProductMetaModel metaModel, DateTime? fdate, DateTime? tdate)
        {
            if (fdate.HasValue)
            {
                metaModel.DrillDownFromDate = fdate.Value;
            }

            if (tdate.HasValue)
            {
                metaModel.DrillDownToDate = tdate.Value;
            }
        }

        #region ProcessRequest

        private HttpResponseMessage ExportExcel(
            WinLossByProductMetaModel metaModel,
            WinLossByProductResultModel resultModel,
            string systemId)
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
                DataList = resultModel.DataList,
                ErrorMessage = resultModel.ResultInfo.Message,
                MetaModel = metaModel,
                SummaryTotal = resultModel.SummaryTotal,
                SystemId = systemId,
                TotalData = resultModel.TotalData,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                BAVisible = productAccess.IsAllowBA,
                SystemIdVisible = !_appSettings.FakeLicenseeList.Contains(licenseeSession.LicenseeId)
            };

            var winlossByProduct = CreateExcel(templatePath, excelModel);
            winlossByProduct.Export();

            return ExcelStream(winlossByProduct.Workbook, FileName);
        }

        private WinLossByProductResultModel GetData(WinLossByProductMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new WinLossByProductResultModel
                {
                    DataList = new List<WinLossByProduct>(),
                    TotalData = new List<WinLossByProduct>(),
                    SummaryTotal = new Dictionary<string, IEnumerable<WinLossByProduct>>(),
                    ResultInfo = new ResultInfo()
                };
            }

            var userCurrencyList = SessionService.UserSession.CurrencyList;
            if (metaModel.Level == 0)
            {
                return _winlossByProductDataAccess.GetWinlossByProducts(metaModel, userCurrencyList);
            }
            else if (SessionService.LicenseeSession.LicenseeId != LicenseeIdentity.OneTwoBet || metaModel.Level == 2)
            {
                return _winlossByProductDataAccess.GetWinlossByProductCustomers(metaModel, userCurrencyList);
            }
            else
            {
                return _winlossByProductDataAccess.GetWinlossByProductAgents(metaModel, userCurrencyList);
            }
        }

        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
#pragma warning disable S107 // Methods should not have too many parameters
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
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (!IsValidRequest(metaModel.CurrencyId, currencyId))
            {
                return BadRequest(ViewResources.InvalidParams);
            }

            metaModel.Username = ValidateUserName(metaModel.Username);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(level == 0 ? metaModel.CurrencyId : currencyId, metaModel.IsExcludedTestData);
            metaModel.DrillDownCurrencyId = currencyId;
            metaModel.DrillDownCurrencyName = currencyName;
            metaModel.Level = level;
            metaModel.CustomerId = custId;
            var drillDownMemberId = memberId;
            if (metaModel.MemberId == 0)
            {
                metaModel.MemberId = drillDownMemberId;
            }

            AssignDrilldownDates(metaModel, fdate, tdate);

            var result = GetData(metaModel);

            result.ResultInfo.Message = GetErrorMessage(result.ResultInfo.Code);

            var drillDownSystemId = systemId;
            if (drillDownMemberId == 0 && !string.IsNullOrWhiteSpace(result.ResultInfo.ExtendedData))
            {
                var extendedItems = result.ResultInfo.ExtendedData.Split(';');
                drillDownMemberId = string.IsNullOrWhiteSpace(extendedItems[0]) ? 0 : Convert.ToInt32(extendedItems[0]);
                drillDownSystemId = extendedItems[1];
                metaModel.MemberId = drillDownMemberId;
            }

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel, result, drillDownSystemId);
            }

            return result.DataList.Any()
                ? new
                {
                    Data = RestructureData(result.DataList, level),
                    Total = result.TotalData,
                    SummaryTotal = result.SummaryTotal,
                    ProductOptions = metaModel.ProductOptions.ToJson(),
                    MemberId = drillDownMemberId,
                    SystemId = drillDownSystemId,
                    ErrorMessage = result.ResultInfo.Message
                }
                : GetEmptyResult(metaModel.ProductOptions, memberId, drillDownSystemId, result.ResultInfo.Message);
        }

        private string ValidateUserName(string userName)
        {
            var placeholderUsername = _appSettings.FakeLicenseeList.Contains(SessionService.LicenseeSession.LicenseeId) ?
                            ViewResources.ExternalId :
                            ViewResources.ExternalIdOrSystemId;
            return !string.IsNullOrWhiteSpace(userName)
                                && userName.Equals(placeholderUsername, StringComparison.OrdinalIgnoreCase)
                                ? string.Empty : userName;
        }

        #endregion ProcessRequest
    }
}