namespace Fanex.Athena.Controllers.Customer
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
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
    using Helpers;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
    public class CustomerComGroupController : ContentBaseController
    {
        private readonly IGroupComService _groupComService;
        private readonly IProductService _productService;

        public CustomerComGroupController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IGroupComService groupComService,
            IProductService productService)
        : base(languageService, sessionService, appSetting)
        {
            _groupComService = groupComService;
            _productService = productService;
        }

        #region Get data

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = await _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language).ConfigureAwait(false);
            var model = _groupComService.GetAll(licenseeId);

            ViewBag.IsAllowNG = productAccess.IsAllowNG;
            ViewBag.IsAllowCS = productAccess.IsAllowCS;

            return View("~/Views/Desktop/Customer/CustomerComm/GroupCommList.cshtml", model);
        }

        #endregion Get data

        #region Create group

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new GroupCommMetaModel();

            await CreateMetaModel(model).ConfigureAwait(false);

            return View("~/Views/Desktop/Customer/CustomerComm/CreateNewGroup.cshtml", model);
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
                else if (CustomerComGroupHelper.IsValidCommissions(metaModel))
                {
                    var request = new Request
                    {
                        Language = userSession.Language,
                        Ip = Request.UserHostAddress
                    };
                    result = _groupComService.InsertGroup(metaModel, request, userSession.UserId);

                    result.Message = CustomerComGroupHelper.GetCreateGroupErrorMessage(result.Code);

                    if (result.IsSuccessful)
                    {
                        result.Message = ViewResources.MessageSuccess;
                        CustomerComGroupHelper.UpdateMetaModelAfterProcessSuccessfully(metaModel);
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
            return View("~/Views/Desktop/Customer/CustomerComm/CreateNewGroup.cshtml", metaModel);
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
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            CreateMetaModel(model);

            CustomerComGroupHelper.SetDropDownListComm(model);

            return View("~/Views/Desktop/Customer/CustomerComm/UpdateGroup.cshtml", model);
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
                else if (CustomerComGroupHelper.IsValidCommissions(metaModel))
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

            return View("~/Views/Desktop/Customer/CustomerComm/UpdateGroup.cshtml", metaModel);
        }

        #endregion Update group

        #region Delete group

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<JsonResult> Delete(int groupId, bool isRemovePlayerComm, bool isResetDefault)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            var productAccess = await _productService.GetProductAccess(licenseeId, userSession.Language).ConfigureAwait(false);

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
        public async Task<ActionResult> ExportExcel()
        {
            const string FileName = "CustomerCommGroup";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/CustomerGroup/CustomerCommGroup.xls");

            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = await _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language).ConfigureAwait(false);
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

        private async Task CreateMetaModel(GroupCommMetaModel metaModel)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var productAccess = await _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language).ConfigureAwait(false);
            var groupList = _groupComService.GetAll(licenseeId);
            var defaultGroupId = groupList.FirstOrDefault(x => x.IsDefaultGroup)?.GroupCommId ?? 0;

            metaModel.LicenseeId = licenseeId;
            metaModel.IsUpdateNG = productAccess.IsAllowNG;
            metaModel.IsUpdateCS = productAccess.IsAllowCS;
            metaModel.DefaultGroupId = defaultGroupId;
            metaModel.OriginalGroupName = metaModel.GroupName;
        }

        private IResultInfo UpdateGroupComm(GroupCommMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currentGroup = _groupComService.GetById(metaModel.GroupCommId);
            if (currentGroup != null && !CustomerComGroupHelper.IsSameGroup(currentGroup, metaModel))
            {
                var request = new Request
                {
                    Language = userSession.Language,
                    Ip = Request.UserHostAddress
                };
                var result = _groupComService.UpdateGroup(metaModel, request, userSession);
                if (string.IsNullOrWhiteSpace(result.Message))
                {
                    result.Message = CustomerComGroupHelper.GetUpdateGroupErrorMessage(result.Code);
                }

                if (result.IsSuccessful)
                {
                    result.Message = ViewResources.MessageSuccess;
                    CustomerComGroupHelper.UpdateMetaModelAfterProcessSuccessfully(metaModel);
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