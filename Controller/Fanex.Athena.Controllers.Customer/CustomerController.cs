// -----------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.Core.Users;
    using DataAccess.Core.Users.Interfaces;
    using Excel.Base.Helpers;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Excel.Customer;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Model.Customer.ViewModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Newtonsoft.Json;
    using Resources;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class CustomerController : BaseController
    {
        private const string GROUP = "Group";
        private const string BASTATUS = "BAStatus";
        private const string CUSTOMERCOMM = "Commission";
        private const string STATUS = "Status";

        private static readonly IEnumerable<string> GridColumns = new[]
        {
            "ExternalId",
            "SystemId",
            "AvailableFunds",
            "PendingFunds",
            "SpecialLevel",
            "OddsGroup",
            "CustomerOption"
        };

        private static readonly IEnumerable<string> GridColumnsForFakeLincesee = new[]
        {
            "ExternalId",
            "AvailableFunds",
            "PendingFunds",
            "CustomerOption"
        };

        private readonly ICustomerListService _customerListService;
        private readonly IGroupComService _groupComService;
        private readonly ILicenseeDataAccess _licenseeDataAccess;
        private readonly IProductService _productService;

        public CustomerController()
        {
            _licenseeDataAccess = new LicenseeDataAccess();
            _customerListService = new CustomerListService();
            _groupComService = new GroupComService();
            _productService = new ProductService();
        }

#pragma warning disable S107 // Methods should not have too many parameters

        public CustomerController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICustomerListService customerListService,
            IGroupComService groupComService,
            ILicenseeDataAccess licenseeDataAccess,
            IProductService productService)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(languageService, sessionService, appSetting)
        {
            _customerListService = customerListService;
            _groupComService = groupComService;
            _licenseeDataAccess = licenseeDataAccess;
            _productService = productService;
        }

        #region Action

        [HttpGet]
        public ActionResult CustomerList(string userName = "", int groupId = -1)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            var productAccess = this._productService.GetProductAccess(licenseeId, userSession.Language);
            var allowLicensees = GetAllowLicenseesForStatusFunction();
            var placeholderUsername = IsFakeLicensee() ? ViewResources.ExternalId : ViewResources.ExternalIdOrSystemId;
            var metaModel = new CustomerListMetaModel
            {
                Username = userName == placeholderUsername ? string.Empty : userName,
                GroupCommId = groupId,
                LicenseeId = licenseeId,
                IsShowStatusDropdown = allowLicensees.Contains(licenseeId.ToString()),
                GridColumns = InitGridColumns(productAccess.IsAllow(Product.BA), allowLicensees),
                OddsGroups = GetOddsGroups()
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = CreateCustomerViewModel(metaModel);

            this.SetValueForBaStatus(model);
            ViewBag.ProductList = productAccess.ProductList.Select(x => x.ProductCode);
            return View("~/Views/Customer/Customers.cshtml", model);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public dynamic CustomerList(CustomerListMetaModel metaModel)
        {
            var placeholderUsername = IsFakeLicensee() ?
                ViewResources.ExternalId :
                ViewResources.ExternalIdOrSystemId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            metaModel.IsShowStatusDropdown = GetAllowLicenseesForStatusFunction().Contains(metaModel.LicenseeId.ToString());
            metaModel.Username = metaModel.Username == placeholderUsername ? string.Empty : metaModel.Username;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var result = _customerListService.Search(metaModel);
            return new JsonResult
            {
                Data = new
                {
                    Data = result.DataList,
                    TotalRows = result.TotalRows
                },
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult ExportExcel(string excelParam)
        {
            if (string.IsNullOrEmpty(excelParam))
            {
                return null;
            }

            var metaModel = JsonConvert.DeserializeObject<CustomerListMetaModel>(excelParam);
            if (metaModel != null)
            {
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.IsShowStatusDropdown = GetAllowLicenseesForStatusFunction().Contains(licenseeId.ToString());
                metaModel.LicenseeId = licenseeId;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

                if (metaModel.GridColumns.Any(i => i.Checked))
                {
                    var templatePath = Server.MapPath(@"~/App_Data/Template/Customers/CustomerList.xls");

                    var model = GetCustomers(metaModel);
                    var fileName = "CustomerList_" + DateTime.Now.ToString("MM-dd-yyyy");
                    var reportTitle = ViewResources.CustomerList + " - "
                        + string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper(CultureInfo.CurrentCulture) : metaModel.CurrencyName);

                    var excel = CreateExcel(templatePath, reportTitle, model);

                    excel.Export();

                    return new ExcelResult(excel.Workbook, fileName);
                }
            }

            return File(ExcelHelper.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        [CustomValidateAntiForgery]
        public ActionResult LogInfo(int custId)
        {
            var licenseeSession = SessionService.LicenseeSession;

            var logInfo = _customerListService.GetCustomerLoginInfo(custId, licenseeSession.LicenseeId);

            var model = new LogInfoViewModel
            {
                LogInfo = logInfo,
                ProductAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language)
            };

            return View("~/Views/Customer/CustomerDialog/LogInfo.cshtml", model);
        }

        #endregion Action

        #region Ultilities

        protected internal virtual CustomerListExcel CreateExcel(
        string templatePath,
        string reportTitle,
        CustomerListViewModel model)
        {
            return new CustomerListExcel(templatePath, reportTitle, model);
        }

        private static IEnumerable<CheckedItem> GetOddsGroups()
        {
            return new List<CheckedItem>
            {
                new CheckedItem { Name = "A", Checked = true },
                new CheckedItem { Name = "B", Checked = true },
                new CheckedItem { Name = "C", Checked = true },
                new CheckedItem { Name = "D", Checked = true },
            };
        }

        private CustomerListViewModel CreateCustomerViewModel(CustomerListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var model = new CustomerListViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
                IsAllowedUserEditComm = SessionService.IsAllowedEditCommission(userSession),
                IsAllowedUserChangeBA = SessionService.IsAllowedSpecialBA(userSession),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                IsFakeLicensee = IsFakeLicensee()
            };
            if (licenseeSession.IsAllowedCustomerComm)
            {
                model.GroupCommDataList = _groupComService.GetAll(licenseeSession.LicenseeId);
                model.CurrentGroupComm = _groupComService.GetById(metaModel.GroupCommId);
            }

            return model;
        }

        private CustomerListViewModel GetCustomers(CustomerListMetaModel metaModel)
        {
            var model = CreateCustomerViewModel(metaModel);

            var result = _customerListService.Search(model.Param);
            model.DataList = result.DataList;
            model.TotalRows = result.TotalRows;

            return model;
        }

        /// <summary>
        ///     This function relates to special BA permission of Justin - Martin.
        ///     Martin - Justin can update BA anytime.
        ///     Other users can update BA on a special time only, and they can not turn on BA
        ///     for the user who Martin-Justin has turned off.
        /// </summary>
        /// <param name="model">The view model.</param>
        private void SetValueForBaStatus(CustomerListViewModel model)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var specialBaPermission = SessionService.IsAllowedSpecialBA(userSession);
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            var request = new Request
            {
                Language = userSession.Language,
                Ip = Request.UserHostAddress
            };
            var canUpdateBa = specialBaPermission;
            var canUpdateBaMessage = string.Empty;
            if (!specialBaPermission)
            {
                var canUpdateBaResult = _licenseeDataAccess.IsAllowUpdateMemberBAStatus(request, licenseeSession.LicenseeId, out canUpdateBa);
                canUpdateBaMessage = canUpdateBaResult.Message;
            }

            model.BATimeCheck = (!canUpdateBa && productAccess.IsAllowBA)
                                    ? canUpdateBaMessage
                                    : string.Empty;
            model.CanUpdateBA = canUpdateBa;
            model.CanUpdateBAMessage = canUpdateBaMessage;
        }

        private IEnumerable<CheckedItem> InitGridColumns(bool isAllowBA, IEnumerable<string> allowLicensees)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var isFakeLicensee = IsFakeLicensee();
            var gridColumns = isFakeLicensee ? GridColumnsForFakeLincesee : GridColumns;
            var items = new List<CheckedItem>();
            foreach (var name in gridColumns)
            {
                items.Add(new CheckedItem { Name = name, Checked = true });
            }

            if (isAllowBA)
            {
                items.Add(new CheckedItem { Name = BASTATUS, Checked = true });
            }

            if (licenseeSession.IsAllowedCustomerComm && !isFakeLicensee)
            {
                items.Insert(0, new CheckedItem { Name = GROUP, Checked = true });
                items.Add(new CheckedItem { Name = CUSTOMERCOMM, Checked = true });
            }

            if (allowLicensees.Contains(licenseeSession.LicenseeId.ToString()))
            {
                items.Add(new CheckedItem { Name = STATUS, Checked = true });
            }

            return items;
        }

        #endregion Ultilities
    }
}