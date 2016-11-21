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
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Customer;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Model.Core.ViewModel;
    using Resources;

    [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
    public class CustomerComGroupController : ReadonlyBaseController
    {
        private readonly IGroupComService _customerComService;
        private readonly IProductService _productService;

        public CustomerComGroupController()
        {
            this._customerComService = new GroupComService();
            this._productService = new ProductService();
        }

        #region Get data

        /// <summary>
        ///     Get all groups.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var model = _customerComService.GetAll(licenseeId);

            ViewBag.IsAllowNG = productAccess.IsAllowNG;
            ViewBag.IsAllowRC = productAccess.IsAllowRB;
            ViewBag.IsAllowLC = productAccess.IsAllowLC;
            ViewBag.IsAllowCS = productAccess.IsAllowCS;

            return View("~/Views/Customer/CustomerComm/GroupCommList.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(bool isExportExcel)
        {
            return isExportExcel ? this.ExportExcel() : this.Index();
        }

        #endregion Get data

        #region Create group

        [HttpGet]
        public ActionResult Create()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var groupList = _customerComService.GetAll(licenseeSession.LicenseeId);

            var defaultGroupId = _customerComService.GetDefaultGroup(groupList);
            var model = new GroupCommMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                IsUpdateNG = productAccess.IsAllowNG,
                IsUpdateRacing = productAccess.IsAllowRB,
                IsUpdateLC = productAccess.IsAllowLC,
                IsUpdateCS = productAccess.IsAllowCS,
                DefaultGroupId = defaultGroupId
            };

            return View("~/Views/Customer/CustomerComm/CreateNewGroup.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Create(GroupCommMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var result = new ResultViewModel { Success = false };
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            if (ModelState.IsValid)
            {
                if (metaModel.GroupName.Trim().Equals("none", StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = ViewResources.ChooseGroupNameMsg;
                    ViewBag.Result = result;
                    return View("~/Views/Customer/CustomerComm/CreateNewGroup.cshtml", metaModel);
                }

                var validComms = GroupCommSetting.ValidCommissions;
                var valid = validComms.Any(i => i.Value == metaModel.SBComm) &&
                            validComms.Any(i => i.Value == metaModel.ParlayComm) &&
                            validComms.Any(i => i.Value == metaModel.RCComm) &&
                            validComms.Any(i => i.Value == metaModel.NGComm) &&
                            validComms.Any(i => i.Value == metaModel.LCComm) &&
                            validComms.Any(i => i.Value == metaModel.CSComm);
                if (valid)
                {
                    var request = new Request
                    {
                        Language = userSession.Language,
                        Ip = Request.UserHostAddress
                    };
                    _customerComService.InsertGroup(metaModel, request, userSession.UserId, out result);
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

        [HttpGet]
        public ActionResult Update(int groupId)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);

            var groupList = _customerComService.GetAll(licenseeSession.LicenseeId);

            var defaultGroupId = _customerComService.GetDefaultGroup(groupList);

            var model = _customerComService.GetById(groupId);
            if (model != null)
            {
                model.LicenseeId = licenseeSession.LicenseeId;
                model.IsUpdateNG = productAccess.IsAllowNG;
                model.IsUpdateRacing = productAccess.IsAllowRB;
                model.IsUpdateLC = productAccess.IsAllowLC;
                model.IsUpdateCS = productAccess.IsAllowCS;
                model.DefaultGroupId = defaultGroupId;
            }

            /*Convert decimal of each comm to int value to display in web.*/
            SetDropDownListComm(model);

            return View("~/Views/Customer/CustomerComm/UpdateGroup.cshtml", model);
        }

        [SuppressMessage(
                  "StyleCop.CSharp.StylecopPlus.Rules",
                  "SP2101:MethodMustNotContainMoreLinesThan",
                  Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Update(GroupCommMetaModel metaModel)
        {
            var result = new ResultViewModel();
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;

            if (ModelState.IsValid)
            {
                if (metaModel.GroupName.Trim().ToLower().Equals("none"))
                {
                    result.ErrorMessage = ViewResources.ChooseGroupNameMsg;
                    ViewBag.Result = result;

                    return View("~/Views/Customer/CustomerComm/UpdateGroup.cshtml", metaModel);
                }

                var validComms = GroupCommSetting.ValidCommissions;
                var valid = validComms.Any(i => i.Value == metaModel.SBComm) &&
                            validComms.Any(i => i.Value == metaModel.ParlayComm) &&
                            validComms.Any(i => i.Value == metaModel.RCComm) &&
                            validComms.Any(i => i.Value == metaModel.NGComm) &&
                            validComms.Any(i => i.Value == metaModel.LCComm) &&
                            validComms.Any(i => i.Value == metaModel.CSComm);

                if (valid)
                {
                    var request = new Request
                    {
                        Language = userSession.Language,
                        Ip = Request.UserHostAddress
                    };
                    var currentGroup = _customerComService.GetById(metaModel.GroupCommId);
                    if (currentGroup != null && !currentGroup.Equals(metaModel))
                    {
                        _customerComService.UpdateGroup(metaModel, request, userSession, out result);

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
        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult Delete(int groupId, bool isRemovePlayerComm, bool isResetDefault)
        {
            var result = new ResultViewModel();

            if (ModelState.IsValid)
            {
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                var userSession = SessionService.UserSession;
                var productAccess = _productService.GetProductAccess(licenseeId, userSession.Language);

                _customerComService.DeleteGroup(
                    new DeleteGroupMetaModel()
                    {
                        GroupId = groupId,
                        LicenseeId = licenseeId,
                        UserId = userSession.UserId,
                        UserName = userSession.UserName,
                        Ip = Request.UserHostAddress,
                        Language = userSession.Language,
                        IsRemoveCommSetting = isRemovePlayerComm,
                        IsAllowedCSComm = productAccess.IsAllowCS,
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
        [HttpGet]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
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

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult ExportExcel()
        {
            const string FileName = "CustomerCommGroup";
            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerGroup/CustomerCommGroup.xls");
            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException(templatePath);
            }

            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var showingOption = new ShowingOption
            {
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                CSVisible = productAccess.IsAllowCS
            };

            var mainTable = _customerComService.GetDataForExcel(licenseeId);

            var custmerCommGroupExcel = new CustomerCommGroup(templatePath, FileName, mainTable, showingOption);

            custmerCommGroupExcel.Export();

            return new ExcelResult(custmerCommGroupExcel.Workbook, FileName);
        }

        #endregion Export Excel

        #region Utility

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
        private static void SetDropDownListComm(GroupCommMetaModel model)
        {
            var decSbComm = model.SBComm;
            var decParlayComm = model.ParlayComm;
            var decRcComm = model.RCComm;
            var decNgComm = model.NGComm;
            var decLcComm = model.LCComm;
            var decCsComm = model.CSComm;

            model.SBComm1 = SetComm(decSbComm, 1);
            model.SBComm2 = SetComm(decSbComm, 2);
            model.SBComm3 = SetComm(decSbComm, 3);

            model.ParlayComm1 = SetComm(decParlayComm, 1);
            model.ParlayComm2 = SetComm(decParlayComm, 2);
            model.ParlayComm3 = SetComm(decParlayComm, 3);

            model.RCComm1 = SetComm(decRcComm, 1);
            model.RCComm2 = SetComm(decRcComm, 2);
            model.RCComm3 = SetComm(decRcComm, 3);

            model.NGComm1 = SetComm(decNgComm, 1);
            model.NGComm2 = SetComm(decNgComm, 2);
            model.NGComm3 = SetComm(decNgComm, 3);

            model.LCComm1 = SetComm(decLcComm, 1);
            model.LCComm2 = SetComm(decLcComm, 2);
            model.LCComm3 = SetComm(decLcComm, 3);

            model.CSComm1 = SetComm(decCsComm, 1);
            model.CSComm2 = SetComm(decCsComm, 2);
            model.CSComm3 = SetComm(decCsComm, 3);
        }

        #endregion Utility
    }
}