// -----------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Customer;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Models.MetaModel.Customer;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Customer;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Customer;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Customer;
    using Fanex.Athena.Service.Interface.Users;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Newtonsoft.Json;

    /// <summary>
    /// The customer controller.
    /// </summary>
    public class CustomerController : BaseController
    {
        private const string GROUP = "Group";
        private const string BASTATUS = "BaStatus";
        private const string CUSTOMERCOMM = "Commission";
        private const string STATUS = "Status";

        private readonly string[] _gridColumNames =
        {
                                                    "ExternalId",
                                                    "SystemId",
                                                    "AvailableFunds",
                                                    "PendingFunds",
                                                    "SpecialLevel",
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

        /// <summary>
        /// The customer list report.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="groupId">The group id.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CustomerList(string userName = "", int groupId = -1)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            var productAccess = this._productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var allowLicensees = GetAllowLicenseesForStatusFunction();
            var metaModel = new CustomerListMetaModel
                        {
                            Username = userName == ViewResources.ExternalIdOrSystemId ? string.Empty : userName,
                            GroupCommId = groupId,
                            LicenseeId = licenseeSession.LicenseeId,
                            UserId = SessionService.UserSession.UserId,
                            AllowLicensees = allowLicensees,
                            GridColumns = this.IniGridColumns(productAccess, allowLicensees)
                        };
            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";

            var model = GetCustomers(metaModel, licenseeSession, userSession);

            this.SetValueForBaStatus(model);

            return View("~/Views/Customer/Customers.cshtml", model);
        }

        /// <summary>
        /// The customer list report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public dynamic CustomerList(CustomerListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.AllowLicensees = GetAllowLicenseesForStatusFunction();
            metaModel.Username = metaModel.Username == ViewResources.ExternalIdOrSystemId ? string.Empty : metaModel.Username;

            var totalRows = 0;
            var dataList = _customerService.Search(metaModel, ref totalRows);

            return Json( new
            {
                Data = dataList,
                TotalRows = totalRows
            });
        }

        /// <summary>
        /// Export excel.
        /// </summary>
        /// <param name="excelParam">The excel parameter.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportExcel(string excelParam)
        {
            if (string.IsNullOrEmpty(excelParam))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(excelParam))
            {
                CustomerListMetaModel metaModel = JsonConvert.DeserializeObject<CustomerListMetaModel>(excelParam);
                if (metaModel != null)
                {
                    metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                    metaModel.UserId = SessionService.UserSession.UserId;
                    metaModel.AllowLicensees = GetAllowLicenseesForStatusFunction();

                    if (metaModel.GridColumns.Where(i => i.Checked).Count() > 0)
                    {
                        var model = GetCustomers(metaModel, SessionService.LicenseeSession, SessionService.UserSession);
                        var templatePath = Server.MapPath(@"~/App_Data/Template/Customers/CustomerList.xls");
                        var fileName = "CustomerList_" + DateTime.Now.ToString("MM-dd-yyyy");
                        if (!System.IO.File.Exists(templatePath))
                        {
                            throw new FileNotFoundException(templatePath);
                        }

                        var reportTitle = ViewResources.CustomerList + " - " + ViewResources.Viewin + " "
                            + (metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

                        var excel = new CustomerListExcel(templatePath, reportTitle, model);

                        excel.Export();

                        return new ExcelResult(excel.Workbook, fileName);
                    }
                }
            }

            return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        /// <summary>
        /// The log info report.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="externalId">The external id.</param>
        /// <param name="systemId">The system id.</param>
        /// <returns>The action result.</returns>
        public ActionResult LogInfo(int custId, string externalId, string systemId)
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

        private CustomerListViewModel CreateCustomerViewModel(CustomerListMetaModel metaModel, LicenseeSession licenseeSession, UserSession userSession)
        {
            var model = new CustomerListViewModel { Param = metaModel };
            model.CurrencyList = SessionService.UserSession.CurrencyList;
            model.LicenseeTypeList = licenseeSession.LicenseeType;
            model.ProductAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            model.IsAllowedUserEditComm = userSession.IsAllowedEditCommission;
            model.IsAllowedUserChangeBA = userSession.IsAllowedSpecialBA;
            model.IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm;
            if (licenseeSession.IsAllowedCustomerComm)
            {
                model.GroupCommDataList = _groupComService.GetAll(licenseeSession.LicenseeId);
                model.CurrentGroupComm = _groupComService.GetById(metaModel.GroupCommId);
            }

            return model;
        }

        /// <summary>
        /// Get customers.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <param name="licenseeSession">The licensee session.</param>
        /// <param name="userSession">The user session.</param>
        /// <returns>The view model.</returns>
        private CustomerListViewModel GetCustomers(CustomerListMetaModel metaModel, LicenseeSession licenseeSession, UserSession userSession)
        {
            var model = CreateCustomerViewModel(metaModel, licenseeSession, userSession);

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
            var specialBaPermission = SessionService.UserSession.IsAllowedSpecialBA;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var request = new Request
                {
                    Language = SessionService.UserSession.Language,
                    Ip = Request.UserHostAddress
                };
            var canUpdateBa = specialBaPermission;
            var canUpdateBaMessage = string.Empty;
            if (!specialBaPermission)
            {
                var canUpdateBaResult = _licenseeService.IsAllowUpdateMemberBaStatus(request, licenseeSession.LicenseeId, out canUpdateBa);
                canUpdateBaMessage = canUpdateBaResult.Message;
            }

            model.BaTimeCheck = (!canUpdateBa && productAccess.IsAllowBa)
                                    ? canUpdateBaMessage
                                    : string.Empty;
            model.CanUpdateBA = canUpdateBa;
            model.CanUpdateBAMessage = canUpdateBaMessage;
        }

        /// <summary>
        /// Initialize grid columns.
        /// </summary>
        /// <param name="productAccess">The product access.</param>
        /// <param name="allowLicensees">The allowed licensees.</param>
        /// <returns>The columns.</returns>
        private IList<CheckedItem> IniGridColumns(ProductAccess productAccess, IList<string> allowLicensees)
        {
            var items = new List<CheckedItem>();
            foreach (var name in _gridColumNames)
            {
                items.Add(new CheckedItem
                {
                    Name = name,
                    Checked = true
                });
            }

            if (productAccess.IsAllowBa)
            {
                items.Add(new CheckedItem
                {
                    Name = BASTATUS,
                    Checked = true
                });
            }

            var licenseeSession = SessionService.LicenseeSession;
            if (licenseeSession.IsAllowedCustomerComm)
            {
                items.Insert(
                 0,
                 new CheckedItem
                 {
                     Name = GROUP,
                     Checked = true
                 });
                items.Add(new CheckedItem
                {
                    Name = CUSTOMERCOMM,
                    Checked = true
                });
            }

            if (true || allowLicensees.Contains(licenseeSession.LicenseeId.ToString()))
            {
                items.Add(new CheckedItem
                {
                    Name = STATUS,
                    Checked = true
                });
            }

            return items;
        }

        #endregion Ultilities
    }
}