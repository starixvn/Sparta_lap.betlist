// -----------------------------------------------------------------------
// <copyright file="CustomerApiController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer
{
    #region Using

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.ViewModel;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.Customer.Helpers;

    #endregion Using

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class CustomerApiController : ContentBaseController
    {
        private const string CustomerAPIRemoveFromGroupComName = "CustomerAPI_RemoveFromGroupCom";

        private readonly ICoreLogger _logger;
        private readonly ICustomerCommService _customerCommService;
        private readonly ICustomerListService _customerListService;
        private readonly IGroupComService _groupComService;
        private readonly ILicenseeDataAccess _licenseeDataAccess;
        private readonly IProductService _productService;

        #region Constructor

#pragma warning disable S107 // Methods should not have too many parameters

        public CustomerApiController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICoreLogger logger,
            ICustomerCommService customerCommService,
            ICustomerListService customerListService,
            IGroupComService groupComService,
            ILicenseeDataAccess licenseeDataAccess,
            IProductService productService)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(languageService, sessionService, appSetting)
        {
            _logger = logger;
            _customerCommService = customerCommService;
            _licenseeDataAccess = licenseeDataAccess;
            _customerListService = customerListService;
            _groupComService = groupComService;
            _productService = productService;
        }

        #endregion Constructor

        #region Level & Status API

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public async Task<ActionResult> UpdateCustStatus(int custId, bool isClosed, bool isSuspended)
        {
            try
            {
                var licenseeSession = SessionService.LicenseeSession;
                if (AppSettings.LicenseeOfferStatusFunction.Contains(licenseeSession.LicenseeId))
                {
                    var userSession = SessionService.UserSession;
                    var customerStatusMetaModel = new CustomerStatusMetaModel
                    {
                        LicenseeId = licenseeSession.LicenseeId,
                        LicenseeName = licenseeSession.LicenseeName,
                        CustId = custId,
                        IsOn = isClosed,
                        ModifyById = userSession.UserId,
                        ModifyByUsername = userSession.UserName,
                        Ip = Request.UserHostAddress
                    };

                    var result = await _customerListService.UpdateCustClosed(customerStatusMetaModel, userSession.Language).ConfigureAwait(false);
                    if (result.Code == 0)
                    {
                        customerStatusMetaModel.IsOn = isSuspended;
                        result = await _customerListService.UpdateCustSuspend(customerStatusMetaModel, userSession.Language).ConfigureAwait(false);
                    }

                    return Json(result);
                }
                else
                {
                    return Json(new ResultInfo { Code = 1, Message = ViewResources.FunctionNoAvailableMsg });
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            return Json(new ResultInfo { Code = 1, Message = ViewResources.HttpError_Default });
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public async Task<ActionResult> UpdateSpecialLevel(int custId, int level)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var isFakeLicensee = IsFakeLicensee(licenseeId);

            if (!isFakeLicensee)
            {
                var userSession = SessionService.UserSession;

                try
                {
                    var dataResult = await _customerListService.UpdateSpecialLevel(
                        licenseeId,
                        userSession.UserId,
                        custId,
                        level,
                        Request.UserHostAddress,
                        userSession.Language).ConfigureAwait(false);

                    return Json(new ResultViewModel
                    {
                        Success = dataResult.IsSuccessful,
                        Message = dataResult.Message,
                        ErrorCode = dataResult.Code,
                        ErrorMessage = dataResult.Message
                    });
                }
                catch (Exception exception)
                {
                    _logger.Error(exception);
                }
            }

            var errorMessage = isFakeLicensee ? ViewResources.FunctionDeniedMsg : ViewResources.HttpError_Default;

            return Json(new ResultViewModel
            {
                Success = false,
                Message = errorMessage,
                ErrorCode = 1,
                ErrorMessage = errorMessage
            });
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public async Task<ActionResult> UpdateOddsGroup(int custId, string oddsGroup)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var isFakeLicensee = IsFakeLicensee(licenseeId);

            if (!isFakeLicensee)
            {
                var userSession = SessionService.UserSession;

                try
                {
                    var metaModel = new CustomerOddsGroupMetaModel
                    {
                        LicenseeId = licenseeId,
                        CustId = custId,
                        Ip = Request.UserHostAddress,
                        Language = userSession.Language,
                        OddsGroup = oddsGroup,
                        UserId = userSession.UserId
                    };
                    var dataResult = await _customerListService.UpdateOddsGroup(metaModel).ConfigureAwait(false);

                    var message = !dataResult.IsSuccessful && string.IsNullOrWhiteSpace(dataResult.Message) ?
                                   ViewResources.HttpError_Default : dataResult.Message;
                    return Json(new ResultViewModel
                    {
                        Success = dataResult.IsSuccessful,
                        Message = message,
                        ErrorCode = dataResult.Code,
                        ErrorMessage = message
                    });
                }
                catch (Exception exception)
                {
                    _logger.Error(exception);
                }
            }

            var errorMessage = isFakeLicensee ? ViewResources.FunctionDeniedMsg : ViewResources.HttpError_Default;
            return Json(new ResultViewModel
            {
                Success = false,
                Message = errorMessage,
                ErrorCode = 1,
                ErrorMessage = errorMessage
            });
        }

        #endregion Level & Status API

        #region BA API

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> UpdateBAStatus(int custId, bool isAllowBA)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = await _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ConfigureAwait(false);
            if (!productAccess.IsAllowBA)
            {
                return CreateUpdatingBaResult(isAllowBA, false, ViewResources.NotAllowBA);
            }

            var request = new Request
            {
                Language = userSession.Language,
                Ip = Request.UserHostAddress
            };

            var specialBaPermission = SessionService.IsAllowedSpecialBA(userSession);
            if (!specialBaPermission)
            {
                var isAllowUpdateBaResult = _licenseeDataAccess.IsAllowUpdateMemberBAStatus(request, licenseeSession.LicenseeId);
                bool isAllowUpdateBa = isAllowUpdateBaResult.ExtendedData;
                if (!isAllowUpdateBa)
                {
                    return CreateUpdatingBaResult(isAllowBA, false, isAllowUpdateBaResult.Message);
                }
            }

            var updateResult = await _customerListService.UpdateCustomerBAStatus(
                licenseeSession.LicenseeId,
                custId,
                userSession.UserId,
                isAllowBA,
                request).ConfigureAwait(false);
            if (updateResult.IsSuccessful)
            {
                return CreateUpdatingBaResult(isAllowBA, true, updateResult.Message, updateResult.ExtendedData);
            }

            return CreateUpdatingBaResult(isAllowBA, false, updateResult.Message);
        }

        #endregion BA API

        #region Customer Comm API

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
        [SuppressMessage(
         "StyleCop.CSharp.StylecopPlus.Rules",
         "SP2101:MethodMustNotContainMoreLinesThan",
         Justification = "Reviewed. Suppression is OK here.")]
        public async Task<ActionResult> UpdateCustomerCom(UpdatedCustomerComMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            if (!licenseeSession.IsAllowedCustomerComm)
            {
                return AccessDenied();
            }

            var result = new ResultViewModel
            {
                Success = false,
                ErrorMessage = ViewResources.HttpError_Default
            };
            if (metaModel.Customers != null && metaModel.Customers.Count > 0)
            {
                var isvalid = GroupCommHelper.IsAllValid(
                                metaModel.SBComm,
                                metaModel.ParlayComm,
                                metaModel.NGComm,
                                metaModel.CSComm);
                var userSession = SessionService.UserSession;

                var productAccess = await _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ConfigureAwait(false);
                if (isvalid)
                {
                    metaModel.LicenseeId = licenseeSession.LicenseeId;
                    metaModel.UserId = userSession.UserId;
                    metaModel.Ip = this.Request.UserHostAddress;
                    metaModel.IsAllowedNG = productAccess.IsAllowNG;
                    metaModel.IsAllowedCS = productAccess.IsAllowCS;
                    metaModel.Language = userSession.Language;
                    /* Get oldGroup Commission to compare with new group commission.
                     * Check data for Update group commission
                     * exclude groupId = -1, 0 (get all group or get none group)*/
                    if (metaModel.GroupId > 0 && IsSameOldComm(metaModel))
                    {
                        result.Success = false;
                        result.ErrorMessage = ViewResources.MessageCommNoChange;
                    }
                    else
                    {
                        this.ProcessUpdatingCustomerCom(metaModel, result);
                    }
                }
            }

            return Json(result);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
        public async Task<ActionResult> RemoveFromGroupCom(string customerIds, bool isRemovePlayercomm)
        {
            var licenseeSession = SessionService.LicenseeSession;
            if (!licenseeSession.IsAllowedCustomerComm)
            {
                return AccessDenied();
            }

            var userSession = SessionService.UserSession;

            var productAccess = await _productService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                CustomerAPIRemoveFromGroupComName).ConfigureAwait(false);
            var metaModel = new RemoveFromGroupComMetaModel
            {
                CustomerIds = customerIds,
                IsRemovePlayercomm = isRemovePlayercomm,
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId,
                Ip = Request.UserHostAddress,
                Language = userSession.Language
            };
            var removalResult = _customerCommService.RemoveCustomersFromGroup(metaModel, productAccess.IsAllowCS);
            var result = new ResultViewModel
            {
                Success = removalResult.IsSuccessful,
                Message = ViewResources.MessageSuccess,
                ErrorMessage = removalResult.Message,
                ErrorCode = removalResult.Code,
                ExtendedData = new
                {
                    SBComm = 0,
                    RCComm = 0,
                    NGComm = 0,
                    RNGComm = 0,
                    GroupName = ViewResources.None,
                    GroupColor = "#ffffff",
                }
            };

            return Json(result);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
        public async Task<ActionResult> MoveToGroupCom(string customerIds, int groupComId)
        {
            var licenseeSession = SessionService.LicenseeSession;
            if (!licenseeSession.IsAllowedCustomerComm)
            {
                return AccessDenied();
            }

            var userSession = SessionService.UserSession;
            var productAccess = await _productService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                CustomerAPIRemoveFromGroupComName).ConfigureAwait(false);

            var metaModel = new MoveToGroupComMetaModel(productAccess)
            {
                CustomerIds = customerIds,
                GroupComId = groupComId,
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId,
                Ip = Request.UserHostAddress,
                Language = userSession.Language
            };

            var temp = _customerCommService.MoveCustomersToGroup(metaModel);
            var result = new ResultViewModel
            {
                Success = temp.IsSuccessful,
                Message = ViewResources.MessageSuccess,
                ErrorMessage = temp.Message,
                ErrorCode = temp.Code
            };

            if (result.Success)
            {
                var newGroup = _groupComService.GetById(metaModel.GroupComId);
                if (newGroup != null)
                {
                    result.ExtendedData = CreateExtendedData(newGroup);
                }
            }

            return Json(result);
        }

        #endregion Customer Comm API

        private static object CreateExtendedData(GroupCommMetaModel newGroup)
        {
            return new
            {
                SBComm = NumberHelper.DecFormat(newGroup.SBComm * 100, 2),
                NGComm = NumberHelper.DecFormat(newGroup.NGComm * 100, 2),
                RNGComm = NumberHelper.DecFormat(newGroup.CSComm * 100, 2),
                newGroup.GroupName,
                newGroup.GroupColor,
            };
        }

        private JsonResult AccessDenied()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult
            {
                Data = new { ErrCode = 403, ErrMsg = "Access Denied." },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private bool IsSameOldComm(UpdatedCustomerComMetaModel metaModel)
        {
            var oldGroup = this._groupComService.GetById(metaModel.GroupId);
#pragma warning disable S1067 // Expressions should not be too complex
            return oldGroup.NGComm == metaModel.NGComm && oldGroup.SBComm == metaModel.SBComm &&
                               oldGroup.CSComm == metaModel.CSComm;
#pragma warning restore S1067 // Expressions should not be too complex
        }

        private void ProcessUpdatingCustomerCom(UpdatedCustomerComMetaModel metaModel, ResultViewModel result)
        {
            metaModel.Language = SessionService.UserSession.Language;
            var temp = _customerCommService.UpdateCustomersCommission(metaModel, metaModel.IsAllowedCS);
            result.Success = temp.IsSuccessful;
            result.Message = ViewResources.MessageSuccess;
            result.ErrorMessage = temp.Message;
            result.ErrorCode = temp.Code;
            result.ExtendedData = new
            {
                SBComm = NumberHelper.DecFormat(metaModel.SBComm * 100, 2),
                NGComm = NumberHelper.DecFormat(metaModel.NGComm * 100, 2),
                RNGComm = NumberHelper.DecFormat(metaModel.CSComm * 100, 2),
                GroupName = ViewResources.None,
                GroupColor = "#ffffff",
            };
        }

        protected internal virtual ActionResult CreateUpdatingBaResult(bool isAllowBA, bool isSuccessful, string message, string oddsGroup = null)
        {
            return Json(new
            {
                IsSuccessful = isSuccessful,
                Message = message,
                IsBA = isAllowBA,
                BAStatus = isAllowBA ? "On" : "Off",
                ConfirmMessage = isAllowBA ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                Tooltip = isAllowBA ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip,
                OddsGroup = oddsGroup
            });
        }
    }
}