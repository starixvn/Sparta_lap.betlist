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
    using Constant.DataDomain;
    using DataAccess.Core.Users.Interfaces;
    using Excel.Base.Helpers;
    using Extension.Utility;
    using Fanex.Athena.Excel.Customer;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Model.Customer.ViewModel;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Model.Customer.DTO;
    using Newtonsoft.Json;
    using Resources;
    using Service.Core.Users.Interfaces;
    using Service.Report.Interfaces;

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
        private readonly ISubsidiaryService _subsidiaryService;

#pragma warning disable S107 // Methods should not have too many parameters
        public CustomerController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICustomerListService customerListService,
            IGroupComService groupComService,
            ILicenseeDataAccess licenseeDataAccess,
            IProductService productService,
            ISubsidiaryService subsidiaryService)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(languageService, sessionService, appSetting)
        {
            _customerListService = customerListService;
            _groupComService = groupComService;
            _licenseeDataAccess = licenseeDataAccess;
            _productService = productService;
            _subsidiaryService = subsidiaryService;
        }

        #region Action

        [HttpGet]
        public ActionResult CustomerList(string userName = "", int groupId = -1)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            var productAccess = this._productService.GetProductAccess(licenseeId, userSession.Language);
            var allowLicensees = AppSettings.LicenseeOfferStatusFunction;
            var placeholderUsername = IsFakeLicensee() ? ViewResources.ExternalId : ViewResources.ExternalIdOrSystemId;
            var metaModel = new CustomerListMetaModel
            {
                Username = userName == placeholderUsername ? string.Empty : userName,
                GroupCommId = groupId,
                LicenseeId = licenseeId,
                IsShowStatusDropdown = allowLicensees.Contains(licenseeId),
                GridColumns = InitGridColumns(productAccess.IsAllow(Product.BA), allowLicensees),
                OddsGroups = GetOddsGroups()
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = CreateCustomerViewModel(metaModel);

            if (!model.IsSubsidiary)
            {
                this.SetValueForBaStatus(model);
            }

            ViewBag.ProductList = productAccess.ProductList.Select(x => x.ProductCode);
            return View("~/Views/Desktop/Customer/Customers.cshtml", model);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public dynamic CustomerList(CustomerListMetaModel metaModel)
        {
            AssignMetaModelData(metaModel);

            var result = _customerListService.GetActiveCustomers(metaModel);
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
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public dynamic DormantCustomerList(CustomerListMetaModel metaModel)
        {
            AssignMetaModelData(metaModel);

            var result = _customerListService.GetDormantCustomers(metaModel);
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

        private void AssignMetaModelData(CustomerListMetaModel metaModel)
        {
            var placeholderUsername = IsFakeLicensee() ?
                            ViewResources.ExternalId :
                            ViewResources.ExternalIdOrSystemId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            metaModel.IsShowStatusDropdown = AppSettings.LicenseeOfferStatusFunction.Contains(metaModel.LicenseeId);
            metaModel.Username = metaModel.Username == placeholderUsername ? string.Empty : metaModel.Username;
            metaModel.AgentListXml = GetAgentListXml(metaModel.SubsidiaryList, metaModel.CurrencyId);
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
                metaModel.IsShowStatusDropdown = AppSettings.LicenseeOfferStatusFunction.Contains(licenseeId);
                metaModel.LicenseeId = licenseeId;
                metaModel.AgentListXml = GetAgentListXml(metaModel.SubsidiaryList, metaModel.CurrencyId);

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

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult ExportExcelDormantCustomer(string excelParam)
        {
            if (string.IsNullOrEmpty(excelParam))
            {
                return null;
            }

            var metaModel = JsonConvert.DeserializeObject<CustomerListMetaModel>(excelParam);
            if (metaModel != null)
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.AgentListXml = GetAgentListXml(metaModel.SubsidiaryList, metaModel.CurrencyId);

                var templatePath = Server.MapPath(@"~/App_Data/Template/Customers/DormantCustomerList.xls");

                var resultModel = _customerListService.GetDormantCustomers(metaModel);
                if (resultModel.DataList.Any())
                {
                    var fileName = "DormantCustomerList_" + DateTime.Now.ToString("MM-dd-yyyy");
                    var reportTitle = ViewResources.DormantCustomers + " - "
                        + string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper(CultureInfo.CurrentCulture) : metaModel.CurrencyName);

                    var excel = CreateDormantCustomerExcel(templatePath, reportTitle, resultModel.DataList);

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

            var logInfo = _customerListService.GetCustomerLoginInfo(custId, licenseeSession.LicenseeId, licenseeSession.LicenseeName);

            var model = new LogInfoViewModel
            {
                LogInfo = logInfo,
                ProductAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language)
            };

            return View("~/Views/Desktop/Customer/CustomerDialog/LogInfo.cshtml", model);
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

        protected internal virtual DormantCustomerListExcel CreateDormantCustomerExcel(
            string templatePath,
            string reportTitle,
            IEnumerable<DormantCustomer> dataList)
        {
            return new DormantCustomerListExcel(templatePath, reportTitle, dataList);
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
            var isSubsidiary = userSession.UserLevel == UserLevelType.Subsidiary;
            IEnumerable<string> subsidiaryNames = userSession.IsManageableSubsidiary ? _subsidiaryService.GetSubsidiaryNames() : new string[] { };

            var model = new CustomerListViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
                IsAllowedUserEditComm = SessionService.IsAllowedEditCommission(userSession),
                IsAllowedUserChangeBA = SessionService.IsAllowedSpecialBA(userSession),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                IsFakeLicensee = IsFakeLicensee(),
                IsSubsidiary = isSubsidiary,
                HasSubsidiaryList = userSession.IsManageableSubsidiary,
                SubsidiaryList = subsidiaryNames.Select(x => new CheckedItem
                {
                    Name = x,
                    Checked = true
                }).ToList()
            };
            if (!isSubsidiary && licenseeSession.IsAllowedCustomerComm)
            {
                model.GroupCommDataList = _groupComService.GetAll(licenseeSession.LicenseeId);
                model.CurrentGroupComm = _groupComService.GetById(metaModel.GroupCommId);
            }

            return model;
        }

        private CustomerListViewModel GetCustomers(CustomerListMetaModel metaModel)
        {
            var model = CreateCustomerViewModel(metaModel);

            var result = _customerListService.GetActiveCustomers(model.Param);
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
                var canUpdateBaResult = _licenseeDataAccess.IsAllowUpdateMemberBAStatus(request, licenseeSession.LicenseeId);
                canUpdateBa = canUpdateBaResult.ExtendedData;
                canUpdateBaMessage = canUpdateBaResult.Message;
            }

            model.BATimeCheck = (!canUpdateBa && productAccess.IsAllowBA)
                                    ? canUpdateBaMessage
                                    : string.Empty;
            model.CanUpdateBA = canUpdateBa;
            model.CanUpdateBAMessage = canUpdateBaMessage;
        }

        private IEnumerable<CheckedItem> InitGridColumns(bool isAllowBA, IEnumerable<int> allowLicensees)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var isSubsidiary = SessionService.UserSession.UserLevel == UserLevelType.Subsidiary;
            var isFakeLicensee = IsFakeLicensee();
            var gridColumns = isFakeLicensee ? GridColumnsForFakeLincesee : GridColumns;
            var items = new List<CheckedItem>();
            foreach (var name in gridColumns)
            {
                items.Add(new CheckedItem { Name = name, Checked = true });
            }

            if (isAllowBA && !isSubsidiary)
            {
                items.Add(new CheckedItem { Name = BASTATUS, Checked = true });
            }

            if (licenseeSession.IsAllowedCustomerComm && !isFakeLicensee && !isSubsidiary)
            {
                items.Insert(0, new CheckedItem { Name = GROUP, Checked = true });
                items.Add(new CheckedItem { Name = CUSTOMERCOMM, Checked = true });
            }

            if (allowLicensees.Contains(licenseeSession.LicenseeId))
            {
                items.Add(new CheckedItem { Name = STATUS, Checked = true });
            }

            return items;
        }

        #endregion Ultilities

        private string GetAgentListXml(string subsidiaryList, int currencyId)
        {
            var userSession = SessionService.UserSession;
            if (userSession.IsManageableSubsidiary)
            {
                var currencyAgentList = _subsidiaryService.GetCurrencyAgentList(subsidiaryList, currencyId: currencyId);
                return SessionService.GenerateAgentListXml(currencyAgentList);
            }

            return SessionService.GenerateAgentListXml(currencyId);
        }
    }
}