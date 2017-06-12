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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
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
    using ONEbook.UM.Filters;
    using Resources;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
    public class CustomerComGroupController : BaseController
    {
        private readonly IGroupComService _customerComService;
        private readonly IProductService _productService;

        public CustomerComGroupController()
        {
            _customerComService = new GroupComService();
            _productService = new ProductService();
        }

        #region Get data

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

        #endregion Get data

        #region Create group

        [HttpGet]
        public ActionResult Create()
        {
            var model = new GroupCommMetaModel();

            CreateMetaModel(model);

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
                }
                else if (ValidateCommissions(metaModel))
                {
                    var request = new Request
                    {
                        Language = userSession.Language,
                        Ip = Request.UserHostAddress
                    };
                    _customerComService.InsertGroup(metaModel, request, userSession.UserId, out result);

                    result.ErrorMessage = GetCreateGroupErrorMessage(result.ErrorCode);

                    if (result.Success)
                    {
                        result.Message = ViewResources.MessageSuccess;
                        UpdateMetaModelAfterProcessSuccessfully(metaModel);
                    }
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
            return string.IsNullOrEmpty(groupName) || _customerComService.CheckExistingGroupByName(SessionService.LicenseeSession.LicenseeId, groupName)
                ? "NO" : "OK";
        }

        #endregion Create group

        #region Update group

        [HttpGet]
        public ActionResult Update(int groupId)
        {
            var model = _customerComService.GetById(groupId);

            if (model != null)
            {
                CreateMetaModel(model);
            }

            /*Convert decimal of each comm to int value to display in web.*/
            SetDropDownListComm(model);

            return View("~/Views/Customer/CustomerComm/UpdateGroup.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Update(GroupCommMetaModel metaModel)
        {
            var result = new ResultViewModel();
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            if (ModelState.IsValid)
            {
                if (metaModel.GroupName.Trim().ToLower(CultureInfo.CurrentCulture).Equals("none"))
                {
                    result.ErrorMessage = ViewResources.ChooseGroupNameMsg;
                }
                else if (ValidateCommissions(metaModel))
                {
                    result = UpdateGroupComm(metaModel);
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

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult Delete(int groupId, bool isRemovePlayerComm, bool isResetDefault)
        {
            var result = new ResultViewModel();

            if (!ModelState.IsValid)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            var productAccess = _productService.GetProductAccess(licenseeId, userSession.Language);

            _customerComService.DeleteGroup(
                new DeleteGroupMetaModel
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
                result.Message = isRemovePlayerComm ? ViewResources.CommIsClear : ViewResources.CommIsUnChange;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Delete group

        #region Group Comm API

        [HttpGet]
        public JsonResult GetDefaultGroup()
        {
            /* Gathering group list*/
            var groupList = _customerComService.GetAll(SessionService.LicenseeSession.LicenseeId);
            var defaultGroupId = groupList.FirstOrDefault(x => x.IsDefaultGroup)?.GroupCommId ?? 0;

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

            var custmerCommGroupExcel = new CustomerCommGroup(templatePath, mainTable, showingOption);

            custmerCommGroupExcel.Export();

            return new ExcelResult(custmerCommGroupExcel.Workbook, FileName);
        }

        #endregion Export Excel

        #region Utility

        private static string GetUpdateGroupErrorMessage(int errorCode)
        {
            switch (errorCode)
            {
                case 100:
                    return ViewResources.MessageCommCannotGreaterThan;

                case 101:
                    return ViewResources.MessageGroupDoesNotExistMsg;

                case 102:
                    return ViewResources.MessageGroupHasAlreadyChoosen;

                case 103:
                    return ViewResources.GroupSetAsDefaultMsg;

                default:
                    return string.Empty;
            }
        }

        private static string GetCreateGroupErrorMessage(int errorCode)
        {
            switch (errorCode)
            {
                case 100:
                    return ViewResources.MessageCommCannotGreaterThan;

                case 101:
                    return ViewResources.MessageGroupCreationFail;

                case 102:
                    return ViewResources.GroupNameExistedMsg;

                default:
                    return string.Empty;
            }
        }

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

        private static bool ValidateCommissions(GroupCommMetaModel metaModel)
        {
            var validComms = GroupCommSetting.ValidCommissions;
            var valid = validComms.Any(i => i.Value == metaModel.SBComm) &&
                        validComms.Any(i => i.Value == metaModel.ParlayComm) &&
                        validComms.Any(i => i.Value == metaModel.RCComm) &&
                        validComms.Any(i => i.Value == metaModel.NGComm) &&
                        validComms.Any(i => i.Value == metaModel.LCComm) &&
                        validComms.Any(i => i.Value == metaModel.CSComm);

            return valid;
        }

        private static void UpdateMetaModelAfterProcessSuccessfully(GroupCommMetaModel metaModel)
        {
            metaModel.IsResetDefault = false;
            metaModel.DefaultGroupId = metaModel.IsDefaultGroup ? metaModel.GroupCommId : metaModel.DefaultGroupId;
        }

        private void CreateMetaModel(GroupCommMetaModel metaModel)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var groupList = _customerComService.GetAll(licenseeId);
            var defaultGroupId = groupList.FirstOrDefault(x => x.IsDefaultGroup)?.GroupCommId ?? 0;

            metaModel.LicenseeId = licenseeId;
            metaModel.IsUpdateNG = productAccess.IsAllowNG;
            metaModel.IsUpdateRacing = productAccess.IsAllowRB;
            metaModel.IsUpdateLC = productAccess.IsAllowLC;
            metaModel.IsUpdateCS = productAccess.IsAllowCS;
            metaModel.DefaultGroupId = defaultGroupId;
        }

        private ResultViewModel UpdateGroupComm(GroupCommMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var request = new Request
            {
                Language = userSession.Language,
                Ip = Request.UserHostAddress
            };
            var currentGroup = _customerComService.GetById(metaModel.GroupCommId);
            var result = new ResultViewModel();

            if (currentGroup != null && !currentGroup.Equals(metaModel))
            {
                _customerComService.UpdateGroup(metaModel, request, userSession, out result);

                if (string.IsNullOrWhiteSpace(result.ErrorMessage))
                {
                    result.ErrorMessage = GetUpdateGroupErrorMessage(result.ErrorCode);
                }

                if (result.Success)
                {
                    result.Message = ViewResources.MessageSuccess;
                    UpdateMetaModelAfterProcessSuccessfully(metaModel);
                }
            }
            else
            {
                result.Success = true;
                result.Message = ViewResources.MessageSuccess;
            }

            return result;
        }

        #endregion Utility
    }
}