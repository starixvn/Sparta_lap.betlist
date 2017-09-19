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
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Fanex.Athena.Excel.Customer;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Model.Customer.ViewModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Customer;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Newtonsoft.Json;
    using Resources;

    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class CustomerController : BaseController
    {
        private const string GROUP = "Group";
        private const string BASTATUS = "BAStatus";
        private const string CUSTOMERCOMM = "Commission";
        private const string STATUS = "Status";

        private static readonly IEnumerable<string> GridColumns = new string[]
        {
            "ExternalId",
            "SystemId",
            "AvailableFunds",
            "PendingFunds",
            "SpecialLevel",
            "OddsGroup",
            "CustomerOption"
        };

        private readonly ICustomerService _customerService;
        private readonly IGroupComService _groupComService;
        private readonly ILicenseeService _licenseeService;
        private readonly IProductService _productService;

        public CustomerController()
        {
            _licenseeService = new LicenseeService();
            _customerService = new CustomerService();
            _groupComService = new GroupComService();
            _productService = new ProductService();
        }

        #region Action

        [HttpGet]
        public ActionResult CustomerList(string userName = "", int groupId = -1)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            var productAccess = this._productService.GetProductAccess(licenseeId, userSession.Language);
            var allowLicensees = GetAllowLicenseesForStatusFunction();
            var metaModel = new CustomerListMetaModel
            {
                Username = userName == ViewResources.ExternalIdOrSystemId ? string.Empty : userName,
                GroupCommId = groupId,
                LicenseeId = licenseeId,
                IsShowStatusDropdown = allowLicensees.Contains(licenseeId.ToString()),
                GridColumns = this.InitGridColumns(productAccess.IsAllowBA, allowLicensees),
                OddsGroups = GetOddsGroups()
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = CreateCustomerViewModel(metaModel);

            this.SetValueForBaStatus(model);

            return View("~/Views/Customer/Customers.cshtml", model);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public dynamic CustomerList(CustomerListMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            metaModel.IsShowStatusDropdown = GetAllowLicenseesForStatusFunction().Contains(metaModel.LicenseeId.ToString());
            metaModel.Username = metaModel.Username == ViewResources.ExternalIdOrSystemId ? string.Empty : metaModel.Username;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var totalRows = 0;
            var dataList = _customerService.Search(metaModel, ref totalRows);
            return new JsonResult
            {
                Data = new
                {
                    Data = dataList,
                    TotalRows = totalRows
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
                    var model = GetCustomers(metaModel);
                    var templatePath = Server.MapPath(@"~/App_Data/Template/Customers/CustomerList.xls");
                    var fileName = "CustomerList_" + DateTime.Now.ToString("MM-dd-yyyy");
                    if (!System.IO.File.Exists(templatePath))
                    {
                        throw new FileNotFoundException(templatePath);
                    }

                    var reportTitle = ViewResources.CustomerList + " - "
                        + string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper(CultureInfo.CurrentCulture) : metaModel.CurrencyName);

                    var excel = new CustomerListExcel(templatePath, reportTitle, model);

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

            var logInfo = _customerService.GetCustomerLoginInfo(custId, licenseeSession.LicenseeId);

            var model = new LogInfoViewModel
            {
                LogInfo = logInfo,
                ProductAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language)
            };

            return View("~/Views/Customer/CustomerDialog/LogInfo.cshtml", model);
        }

        #endregion Action

        #region Ultilities

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
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
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

            var totalRows = 0;
            model.DataList = _customerService.Search(model.Param, ref totalRows);
            model.TotalRows = totalRows;

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
                var canUpdateBaResult = _licenseeService.IsAllowUpdateMemberBAStatus(request, licenseeSession.LicenseeId, out canUpdateBa);
                canUpdateBaMessage = canUpdateBaResult.Message;
            }

            model.BATimeCheck = (!canUpdateBa && productAccess.IsAllowBA)
                                    ? canUpdateBaMessage
                                    : string.Empty;
            model.CanUpdateBA = canUpdateBa;
            model.CanUpdateBAMessage = canUpdateBaMessage;
        }

        private IList<CheckedItem> InitGridColumns(bool isAllowBA, IList<string> allowLicensees)
        {
            var items = new List<CheckedItem>();
            foreach (var name in GridColumns)
            {
                items.Add(new CheckedItem { Name = name, Checked = true });
            }

            if (isAllowBA)
            {
                items.Add(new CheckedItem { Name = BASTATUS, Checked = true });
            }

            var licenseeSession = SessionService.LicenseeSession;
            if (licenseeSession.IsAllowedCustomerComm)
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