namespace Fanex.Athena.Controllers.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Customer;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Customer.Helpers;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
    public class CustomerComGroupController : BaseController
    {
        private static readonly IDictionary<int, string> UpdatingErrorMap = new Dictionary<int, string>
        {
            { 100, nameof(ViewResources.MessageCommCannotGreaterThan) },
            { 101, nameof(ViewResources.MessageGroupDoesNotExistMsg) },
            { 102, nameof(ViewResources.MessageGroupHasAlreadyChoosen) },
            { 103, nameof(ViewResources.GroupSetAsDefaultMsg) },
        };

        private static readonly IDictionary<int, string> CreatingErrorMap = new Dictionary<int, string>
        {
            { 100, nameof(ViewResources.MessageCommCannotGreaterThan) },
            { 101, nameof(ViewResources.MessageGroupCreationFail) },
            { 102, nameof(ViewResources.GroupNameExistedMsg) }
        };

        private readonly IGroupComService _groupComService;
        private readonly IProductService _productService;

        public CustomerComGroupController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IGroupComService groupComService,
            IProductService productService)
        : base(languageService, sessionService, appSetting, clientViewHelper)
        {
            _groupComService = groupComService;
            _productService = productService;
        }

        #region Get data

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var model = _groupComService.GetAll(licenseeId);

            ViewBag.IsAllowNG = productAccess.IsAllowNG;
            ViewBag.IsAllowCS = productAccess.IsAllowCS;
            ViewBag.IsAllowAB = productAccess.IsAllowAB;

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
            IResultInfo result = new ResultInfo();
            metaModel.LicenseeId = licenseeSession.LicenseeId;

            if (ModelState.IsValid)
            {
                if (metaModel.GroupName.Trim().Equals("none", StringComparison.OrdinalIgnoreCase))
                {
                    result.Code = 1;
                    result.Message = ViewResources.ChooseGroupNameMsg;
                }
                else if (IsValidCommissions(metaModel))
                {
                    var request = new Request
                    {
                        Language = userSession.Language,
                        Ip = Request.UserHostAddress
                    };
                    result = _groupComService.InsertGroup(metaModel, request, userSession.UserId);

                    result.Message = GetCreateGroupErrorMessage(result.Code);

                    if (result.IsSuccessful)
                    {
                        result.Message = ViewResources.MessageSuccess;
                        UpdateMetaModelAfterProcessSuccessfully(metaModel);
                    }
                }
                else
                {
                    result.Code = 2;
                    result.Message = ViewResources.MessageInvalidCommValue;
                }
            }
            else
            {
                result.Code = 3;
                result.Message = ViewResources.InvalidParams;
            }

            ViewBag.Result = result;
            return View("~/Views/Customer/CustomerComm/CreateNewGroup.cshtml", metaModel);
        }

        [HttpGet]
        public string Validate(string groupName)
        {
            return string.IsNullOrEmpty(groupName) || _groupComService.CheckExistingGroupByName(SessionService.LicenseeSession.LicenseeId, groupName)
                ? "NO" : "OK";
        }

        #endregion Create group

        #region Update group

        [HttpGet]
        public ActionResult Update(int groupId)
        {
            var model = _groupComService.GetById(groupId);

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
            IResultInfo result = new ResultInfo();
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            if (ModelState.IsValid)
            {
                if (metaModel.GroupName.Trim().ToLower(CultureInfo.CurrentCulture).Equals("none"))
                {
                    result.Code = 1;
                    result.Message = ViewResources.ChooseGroupNameMsg;
                }
                else if (IsValidCommissions(metaModel))
                {
                    result = UpdateGroupComm(metaModel);
                }
                else
                {
                    result.Code = 2;
                    result.Message = ViewResources.MessageInvalidCommValue;
                }
            }
            else
            {
                result.Code = 3;
                result.Message = ViewResources.InvalidParams;
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
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            var productAccess = _productService.GetProductAccess(licenseeId, userSession.Language);

            var result = _groupComService.DeleteGroup(
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
                });

            if (result.Code == 0)
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
            var groupList = _groupComService.GetAll(SessionService.LicenseeSession.LicenseeId);
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
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/CustomerGroup/CustomerCommGroup.xls");

            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var showingOption = new ShowingOption
            {
                SBVisible = productAccess.IsAllowSB,
                NGVisible = productAccess.IsAllowNG,
                CSVisible = productAccess.IsAllowCS,
                ABVisible = productAccess.IsAllowAB
            };

            var mainTable = _groupComService.GetDataForExcel(licenseeId);

            var excel = CreateExcel(templatePath, mainTable, showingOption);

            excel.Export();

            return new ExcelResult(excel.Workbook, FileName);
        }

        protected internal virtual CustomerCommGroupExcel CreateExcel(string templatePath, DataTable mainTable, ShowingOption showingOption)
        {
            return new CustomerCommGroupExcel(templatePath, mainTable, showingOption);
        }

        #endregion Export Excel

        #region Utility

        private static bool IsValidCommissions(GroupCommMetaModel metaModel)
        {
            return GroupCommHelper.IsAllValid(
                                metaModel.SBComm,
                                metaModel.ParlayComm,
                                metaModel.NGComm,
                                metaModel.CSComm,
                                metaModel.ABComm);
        }

        private static string GetUpdateGroupErrorMessage(int errorCode)
        {
            if (!UpdatingErrorMap.ContainsKey(errorCode))
            {
                return string.Empty;
            }

            return ViewResources.ResourceManager.GetString(UpdatingErrorMap[errorCode]);
        }

        private static string GetCreateGroupErrorMessage(int errorCode)
        {
            if (!CreatingErrorMap.ContainsKey(errorCode))
            {
                return string.Empty;
            }

            return ViewResources.ResourceManager.GetString(CreatingErrorMap[errorCode]);
        }

        private static int SetComm(decimal comm, int position)
        {
            var actualComm = comm * 10000;
            switch (position)
            {
                case 1:
                    return (int)(actualComm / 100);

                case 2:
                    return (int)(actualComm % 100) / 10;

                case 3:
                    return (int)actualComm % 10;

                default:
                    return 0;
            }
        }

        /// <summary>
        ///  Repair commission values in order to binding them to drop down list control.
        /// </summary>
        /// <param name="model">The group commission metal model.</param>
        private static void SetDropDownListComm(GroupCommMetaModel model)
        {
            var decSbComm = model.SBComm;
            var decParlayComm = model.ParlayComm;
            var decNgComm = model.NGComm;
            var decCsComm = model.CSComm;
            var decAbComm = model.ABComm;

            model.SBComm1 = SetComm(decSbComm, 1);
            model.SBComm2 = SetComm(decSbComm, 2);
            model.SBComm3 = SetComm(decSbComm, 3);

            model.ParlayComm1 = SetComm(decParlayComm, 1);
            model.ParlayComm2 = SetComm(decParlayComm, 2);
            model.ParlayComm3 = SetComm(decParlayComm, 3);

            model.NGComm1 = SetComm(decNgComm, 1);
            model.NGComm2 = SetComm(decNgComm, 2);
            model.NGComm3 = SetComm(decNgComm, 3);

            model.CSComm1 = SetComm(decCsComm, 1);
            model.CSComm2 = SetComm(decCsComm, 2);
            model.CSComm3 = SetComm(decCsComm, 3);

            model.ABComm1 = SetComm(decAbComm, 1);
            model.ABComm2 = SetComm(decAbComm, 2);
            model.ABComm3 = SetComm(decAbComm, 3);
        }

        private static void UpdateMetaModelAfterProcessSuccessfully(GroupCommMetaModel metaModel)
        {
            metaModel.IsResetDefault = false;
            metaModel.DefaultGroupId = metaModel.IsDefaultGroup ? metaModel.GroupCommId : metaModel.DefaultGroupId;
            metaModel.OriginalGroupName = metaModel.GroupName;
        }

        private static decimal[] BuildCommissionList(GroupCommMetaModel groupComm)
        {
            return new[]
           {
                groupComm.SBComm,
                groupComm.ParlayComm,
                groupComm.NGComm,
                groupComm.CSComm,
                groupComm.ABComm
            };
        }

        private static bool IsSameGroup(GroupCommMetaModel currentGroup, GroupCommMetaModel editedGroup)
        {
            var currentComm = BuildCommissionList(currentGroup);

            var editedComm = BuildCommissionList(editedGroup);

            for (var i = 0; i < currentComm.Count(); i++)
            {
                if (currentComm[i] != editedComm[i])
                {
                    return false;
                }
            }

            return currentGroup.GroupColor.Equals(editedGroup.GroupColor, StringComparison.OrdinalIgnoreCase)
                && currentGroup.GroupCommId == editedGroup.GroupCommId
                && currentGroup.GroupName.Equals(editedGroup.GroupName)
                && currentGroup.IsDefaultGroup == editedGroup.IsDefaultGroup;
        }

        private void CreateMetaModel(GroupCommMetaModel metaModel)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var groupList = _groupComService.GetAll(licenseeId);
            var defaultGroupId = groupList.FirstOrDefault(x => x.IsDefaultGroup)?.GroupCommId ?? 0;

            metaModel.LicenseeId = licenseeId;
            metaModel.IsUpdateNG = productAccess.IsAllowNG;
            metaModel.IsUpdateCS = productAccess.IsAllowCS;
            metaModel.IsUpdateAB = productAccess.IsAllowAB;
            metaModel.DefaultGroupId = defaultGroupId;
            metaModel.OriginalGroupName = metaModel.GroupName;
        }

        private IResultInfo UpdateGroupComm(GroupCommMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currentGroup = _groupComService.GetById(metaModel.GroupCommId);
            if (currentGroup != null && !IsSameGroup(currentGroup, metaModel))
            {
                var request = new Request
                {
                    Language = userSession.Language,
                    Ip = Request.UserHostAddress
                };
                var result = _groupComService.UpdateGroup(metaModel, request, userSession);
                if (string.IsNullOrWhiteSpace(result.Message))
                {
                    result.Message = GetUpdateGroupErrorMessage(result.Code);
                }

                if (result.IsSuccessful)
                {
                    result.Message = ViewResources.MessageSuccess;
                    UpdateMetaModelAfterProcessSuccessfully(metaModel);
                }

                return result;
            }
            else
            {
                return new ResultInfo
                {
                    Message = ViewResources.MessageSuccess
                };
            }
        }

        #endregion Utility
    }
}