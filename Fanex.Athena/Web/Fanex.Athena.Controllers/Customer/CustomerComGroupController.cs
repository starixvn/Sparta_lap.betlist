// -----------------------------------------------------------------------
// <copyright file="CustomerComGroupController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

/*
 * @Alex20120724
 * Customer-group-list report,include these function are CreateGroup,ListGroup (Index) and CustomerList
 */

namespace Fanex.Athena.Controllers.Customer
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Customer;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Customer;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Customer;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    /// Customer group commission.
    /// </summary>
    public class CustomerComGroupController : ReadonlyBaseController
    {
        #region Fields

        private readonly IGroupComService _customerComService;
        private readonly IProductService _productService;

        #endregion Fields

        #region Constructor

        public CustomerComGroupController()
        {
            this._customerComService = new GroupComService();
            this._productService = new ProductService();
        }

        #endregion Constructor

        #region Action Methods

        #region Get data

        /// <summary>
        ///     Get all groups.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language, false);
            var model = _customerComService.GetAll(licenseeId);

            ViewBag.IsAllowNG = productAccess.IsAllowNg;
            ViewBag.IsAllowRC = productAccess.IsAllowRb;
            ViewBag.IsAllowLC = productAccess.IsAllowLc;
            ViewBag.IsAllowCS = productAccess.IsAllowCs;

            return View("~/Views/Customer/CustomerComm/GroupCommList.cshtml", model);
        }

        [HttpPost]
        public ActionResult Index(bool isExportExcel)
        {
            return isExportExcel ? this.ExportExcel() : this.Index();
        }

        #endregion Get data

        #region Create group

        /// <summary>
        ///     Create new group.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, false);
            /*Gathering group list*/
            var groupList = _customerComService.GetAll(licenseeSession.LicenseeId);

            /*Find group which is set to default*/
            var defaultGroupId = _customerComService.GetDefaultGroup(groupList);
            var model = new GroupCommMetaModel
                {
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                    IsUpdateNG = productAccess.IsAllowNg,
                    IsUpdateRacing = productAccess.IsAllowRb,
                    IsUpdateLC = productAccess.IsAllowLc,
                    IsUpdateCS = productAccess.IsAllowCs,
                    DefaultGroupId = defaultGroupId
                };

            return View("~/Views/Customer/CustomerComm/CreateNewGroup.cshtml", model);
        }

        /// <summary>
        /// Create new group commission.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult Create(GroupCommMetaModel metaModel)
        {
            var result = new ResultViewModel { Success = false };
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            if (ModelState.IsValid)
            {
                /*Check valid group name (exclude 'None')*/
                if (metaModel.GroupName.Trim().Equals("none", StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = ViewResources.ChooseGroupNameMsg;
                    ViewBag.Result = result;
                    return View("~/Views/Customer/CustomerComm/CreateNewGroup.cshtml", metaModel);
                }

                var validComms = GroupCommSetting.GetValidComm();
                var valid = validComms.Any(i => i.Value == metaModel.SBComm) &&
                            validComms.Any(i => i.Value == metaModel.RCComm) &&
                            validComms.Any(i => i.Value == metaModel.NGComm) &&
                            validComms.Any(i => i.Value == metaModel.LCComm) &&
                            validComms.Any(i => i.Value == metaModel.CSComm);
                if (valid)
                {
                    var request = new Request
                        {
                            Language = SessionService.UserSession.Language,
                            Ip = Request.UserHostAddress
                        };
                    _customerComService.InsertGroup(metaModel, request, SessionService.LicenseeSession.UserId, out result);
                    GetCreateErrorMessage(result);
                }
                else
                {
                    result.ErrorMessage = ViewResources.MessageInvalidCommValue;
                    result.Success = false;
                }
            }

            ViewBag.Result = result;
            return View("~/Views/Customer/CustomerComm/CreateNewGroup.cshtml", metaModel);
        }

        [HttpGet]
        public string Validate(string groupName)
        {
            if (!string.IsNullOrEmpty(groupName))
            {
                var result = _customerComService.CheckByName(SessionService.LicenseeSession.LicenseeId, groupName);
                return result == false ? "OK" : "NO";
            }

            return "NO";
        }

        #endregion Create group

        #region Update group

        /// <summary>
        /// Update group commission.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult Update(int groupId)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, false);

            /*Gathering group list*/
            var groupList = _customerComService.GetAll(licenseeSession.LicenseeId);

            /* Find group which is set to default*/
            var defaultGroupId = _customerComService.GetDefaultGroup(groupList);

            var model = _customerComService.GetById(groupId);
            if (model != null)
            {
                model.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                model.IsUpdateNG = productAccess.IsAllowNg;
                model.IsUpdateRacing = productAccess.IsAllowRb;
                model.IsUpdateLC = productAccess.IsAllowLc;
                model.IsUpdateCS = productAccess.IsAllowCs;
                model.DefaultGroupId = defaultGroupId;
            }

            /*Convert decimal of each comm to int value to display in web.*/
            SetDropDownListComm(ref model);

            return View("~/Views/Customer/CustomerComm/UpdateGroup.cshtml", model);
        }

        /// <summary>
        /// Update group commission.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [SuppressMessage(
                  "StyleCop.CSharp.Nexcel.NexcelCustomRules",
                  "SP2101:MethodMustNotContainMoreLinesThan",
                  Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        public ActionResult Update(GroupCommMetaModel metaModel)
        {
            var result = new ResultViewModel();
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            
            if (ModelState.IsValid)
            {
                if (metaModel.GroupName.Trim().ToLower().Equals("none"))
                {
                    result.ErrorMessage = ViewResources.ChooseGroupNameMsg;
                    ViewBag.Result = result;

                    return View("~/Views/Customer/CustomerComm/UpdateGroup.cshtml", metaModel);
                }

                var validComms = GroupCommSetting.GetValidComm();
                var valid = validComms.Any(i => i.Value == metaModel.SBComm) &&
                            validComms.Any(i => i.Value == metaModel.RCComm) &&
                            validComms.Any(i => i.Value == metaModel.NGComm) &&
                            validComms.Any(i => i.Value == metaModel.LCComm) &&
                            validComms.Any(i => i.Value == metaModel.CSComm);

                if (valid)
                {
                    var request = new Request
                    {
                        Language = SessionService.UserSession.Language,
                        Ip = Request.UserHostAddress
                    };
                    var userId = SessionService.LicenseeSession.UserId;
                    var currentGroup = _customerComService.GetById(metaModel.GroupCommId);
                    if (currentGroup != null && !currentGroup.Equals(metaModel))
                    {                        
                        _customerComService.UpdateGroup(metaModel, request, SessionService.UserSession, out result);
                        
                        if (string.IsNullOrWhiteSpace(result.ErrorMessage))
                        {
                            GetUpdateGroupErrorMessage(result);
                        }
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = ViewResources.MessageSuccess;
                    }
                }
                else
                {
                    result.ErrorMessage = ViewResources.MessageInvalidCommValue;
                    result.Success = false;
                }
            }

            ViewBag.Result = result;

            return View("~/Views/Customer/CustomerComm/UpdateGroup.cshtml", metaModel);
        }
        
        #endregion Update group

        #region Delete group

        /// <summary>
        /// Delete a group commission.
        /// </summary>
        /// <param name="groupId">The group Id.</param>
        /// <param name="isRemovePlayerComm">If this value equals true, removing all commission of customers belong to this group.</param>
        /// <param name="isResetDefault">Reset default commission for customers  belong to this group.</param>
        /// <returns>The JSON result.</returns>
        [HttpGet]
        public JsonResult Delete(int groupId, bool isRemovePlayerComm, bool isResetDefault)
        {
            var result = new ResultViewModel();

            if (ModelState.IsValid)
            {
                var licenseeSession = SessionService.LicenseeSession;
                var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, false);

                _customerComService.DeleteGroup(
                    new DeleteGroupMetaModel()
                    {
                        GroupId = groupId,
                        LicenseeId = licenseeSession.LicenseeId,
                        UserId = SessionService.UserSession.UserId,
                        UserName = SessionService.UserSession.UserName,
                        Ip = Request.UserHostAddress,
                        Language = SessionService.UserSession.Language,
                        IsRemoveCommSetting = isRemovePlayerComm,
                        IsAllowedCSComm = productAccess.IsAllowCs,
                        IsResetDefault = isResetDefault
                    },
                    out result);     

                if (result.ErrorCode == 0)
                {
                    result.Message = isRemovePlayerComm
                                                ? ViewResources.CommIsClear
                                                : ViewResources.CommIsUnChange;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Delete group

        #region Group Comm API

        /// <summary>
        /// Get default group commission.
        /// </summary>
        /// <returns>The JSON result.</returns>
        public JsonResult GetDefaultGroup()
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            /* Gathering group list*/
            var groupList = _customerComService.GetAll(licenseeId);
            var defaultGroupId = _customerComService.GetDefaultGroup(groupList);
            return Json(new { id = defaultGroupId }, JsonRequestBehavior.AllowGet);
        }

        #endregion Group Comm API

        #region Export Excel

        /// <summary>
        /// Export excel file.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ExportExcel()
        {
            const string FileName = "CustomerCommGroup";
            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerGroup/CustomerCommGroup.xls");
            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException(templatePath);
            }

            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, false);
            var showingOption = new ShowingOption
                {
                    RbVisible = productAccess.IsAllowRb,
                    NgVisible = productAccess.IsAllowNg,
                    CsVisible = productAccess.IsAllowCs
                };

            var mainTable = _customerComService.GetDataForExcel(SessionService.LicenseeSession.LicenseeId);

            var custmerCommGroupExcel = new CustomerCommGroup(templatePath, FileName, mainTable, showingOption);

            custmerCommGroupExcel.Export();

            return new ExcelResult(custmerCommGroupExcel.Workbook, FileName);
        }

        #endregion Export Excel

        #region Utility

        /// <summary>
        /// Get error message for updating.
        /// </summary>
        /// <param name="result">Result View Model.</param>   
        private static void GetUpdateGroupErrorMessage(ResultViewModel result)
        {
            switch (result.ErrorCode)
            {
                case 100:
                    result.ErrorMessage = ViewResources.MessageCommCannotGreaterThan;
                    break;

                case 101:
                    result.ErrorMessage = ViewResources.MessageGroupDoesNotExistMsg;
                    break;

                case 102:
                    result.ErrorMessage = ViewResources.MessageGroupHasAlreadyChoosen;
                    break;

                case 103:
                    result.ErrorMessage = ViewResources.GroupSetAsDefaultMsg;
                    break;

                default:
                    result.Message = ViewResources.MessageSuccess;
                    break;
            }
        }

        /// <summary>
        /// Get error message for creating.
        /// </summary>
        /// <param name="result">Result View Model.</param>        
        private static void GetCreateErrorMessage(ResultViewModel result)
        {
            switch (result.ErrorCode)
            {
                case 100:
                    result.ErrorMessage = ViewResources.MessageCommCannotGreaterThan;
                    break;
                case 101:
                    result.ErrorMessage = ViewResources.MessageGroupCreationFail;
                    break;
                case 102:
                    result.ErrorMessage = ViewResources.GroupNameExistedMsg;
                    break;
                default:
                    result.Message = ViewResources.MessageSuccess;
                    break;
            }
        }

        /// <summary>
        /// Set commission.
        /// </summary>
        /// <param name="comm">Commission after * 10000 (for easy calculating  value for each drop-down-list).</param>
        /// <param name="position">A position of number (First, Second and Last).</param>
        /// <returns>The commission.</returns>
        private static int SetComm(decimal comm, int position)
        {
            var intComm = 0;
            var actualComm = comm * 10000;
            switch (position)
            {
                case 1:
                    intComm = (int)(actualComm / 100);
                    break;

                case 2:
                    intComm = (int)(actualComm % 100) / 10;
                    break;

                case 3:
                    intComm = (int)actualComm % 10;
                    break;
            }

            return intComm;
        }

        /// <summary>
        ///  Repair commission values in order to binding them to drop down list control.
        /// </summary>
        /// <param name="model">The group commission metal model.</param>
        private void SetDropDownListComm(ref GroupCommMetaModel model)
        {
            var decSbComm = model.SBComm;
            var decRcComm = model.RCComm;
            var decNgComm = model.NGComm;
            var decLcComm = model.LCComm;
            var decCsComm = model.CSComm;
            model.SbComm1 = SetComm(decSbComm, 1);
            model.SbComm2 = SetComm(decSbComm, 2);
            model.SbComm3 = SetComm(decSbComm, 3);

            model.RcComm1 = SetComm(decRcComm, 1);
            model.RcComm2 = SetComm(decRcComm, 2);
            model.RcComm3 = SetComm(decRcComm, 3);

            model.NgComm1 = SetComm(decNgComm, 1);
            model.NgComm2 = SetComm(decNgComm, 2);
            model.NgComm3 = SetComm(decNgComm, 3);

            model.LcComm1 = SetComm(decLcComm, 1);
            model.LcComm2 = SetComm(decLcComm, 2);
            model.LcComm3 = SetComm(decLcComm, 3);

            model.CsComm1 = SetComm(decCsComm, 1);
            model.CsComm2 = SetComm(decCsComm, 2);
            model.CsComm3 = SetComm(decCsComm, 3);
        }

        #endregion Utility

        #endregion Action Methods
    }
}