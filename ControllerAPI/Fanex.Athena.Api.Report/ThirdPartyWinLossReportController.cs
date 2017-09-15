// -----------------------------------------------------------------------
// <copyright file="ThirdPartyWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Users;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    public class ThirdPartyWinLossReportController : ApiReportBaseController
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly IThirdPartyWinLossService _thirdPartyWinLossService;

        private readonly IDictionary<int, string> _productPermissionMap =
            new Dictionary<int, string>
            {
                { ProductTypes.GoldDeluxe, PermissionKeys.ReportGoldDeluxeWinloss }
            };

        public ThirdPartyWinLossReportController()
        {
            _thirdPartyWinLossService = new ThirdPartyWinLossService();
            _authorizationService = new AuthorizationService();
        }

        public ThirdPartyWinLossReportController(
            IThirdPartyWinLossService thirdPartyWinLossService,
            IAuthorizationService authorizationService,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _thirdPartyWinLossService = thirdPartyWinLossService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public dynamic Index(
            ThirdPartyWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        [HttpPost]
        public dynamic Index([FromBody] ThirdPartyWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        [CLSCompliant(false)]
        protected internal virtual ThirdPartyWinLossExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            ThirdPartyWinLossMetaModel metaModel,
            ThirdPartyWinLossViewModel excelModel)
        {
            return new ThirdPartyWinLossExcel(templatePath, convertingOption, metaModel, excelModel)
            {
                Level = metaModel.DrillDownLevel,
            };
        }

        private static dynamic GetEmptyThirdPartyWinLossResult()
        {
            return new
            {
                Data = new List<ThirdPartyWinLoss>(),
                Total = new List<ThirdPartyWinLoss>(),
                SummaryTotal = new Dictionary<string, ThirdPartyWinLoss>()
            };
        }

        private static string GetExcelFileName(ThirdPartyWinLossMetaModel metaModel, ref KeyValuePair<int, string> currency, string productCodeExport)
        {
            var fromDate = $"{(metaModel.DrillDownLevel > 0 ? metaModel.DrillDownFromDate : metaModel.FromDate):ddMMMyyyy}";
            var currencyName = currency.Key == 0 ? "All" : currency.Value;

            var exportedFileName = $"{productCodeExport}WinLoss_{fromDate}_{currencyName}.xls";
            return exportedFileName;
        }

        private HttpResponseMessage ExportExcel(
            ThirdPartyWinLossMetaModel metaModel,
            IList<ThirdPartyWinLoss> dataList,
            IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/ThirdPartyWinLoss.xls");
            var currency = userSession.CurrencyList.SingleOrDefault(i => i.Key == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));

            var exportedFileName = GetExcelFileName(metaModel, ref currency, metaModel.ProductCode);

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currency.Value
            };

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
                metaModel.CurrencyId = metaModel.CurrencyId > 0 ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            }

            var excelModel = new ThirdPartyWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = CreateExcel(templatePath, convertingOption, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, exportedFileName);
        }

        private void GetData(
                ThirdPartyWinLossMetaModel metaModel,
                out SortableList<ThirdPartyWinLoss> dataList,
                out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            dataList = metaModel.DrillDownLevel == 0 ? _thirdPartyWinLossService.GetSummaryWinloss(metaModel, out summaryTotal) : metaModel.DrillDownLevel == 2
                               ? _thirdPartyWinLossService.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                               : _thirdPartyWinLossService.GetAgentWinlossDetail(metaModel, out summaryTotal);
        }

        private dynamic ProcessRequest(
            ThirdPartyWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            var userSession = SessionService.UserSession;
            if (!HasPermission(metaModel.ProductId, userSession))
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Content = new StringContent(ViewResources.AccessDeniedMsg)
                };
            }

            if (IsValidRequest(metaModel, currencyId))
            {
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level == 0 ? metaModel.CurrencyId : currencyId, metaModel.IsExcludedTestData);

                IDictionary<string, ThirdPartyWinLoss> summaryTotal;
                SortableList<ThirdPartyWinLoss> dataList;
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
                        Total = _thirdPartyWinLossService.TotalCalc(dataList),
                        SummaryTotal = summaryTotal
                    };
                }
            }

            return GetEmptyThirdPartyWinLossResult();
        }

        private bool HasPermission(int productId, UserSession user)
        {
            var permissionKey = new string[] { _productPermissionMap[productId] };

            return user.ApplicationMode == AppMode.Parasitic ?
                _authorizationService.HasPermissions(user.PortalFunctionList, permissionKey) :
                _authorizationService.HasPermissions(user.FunctionIdList, permissionKey);
        }
    }
}