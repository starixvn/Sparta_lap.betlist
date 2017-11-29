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
    using System.Net;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Users;
    using DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Athena.Service.Customer;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.ViewModel;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Customer.Helpers;

    #endregion Using

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class CustomerApiController : BaseController
    {
        private const string CustomerAPIRemoveFromGroupComName = "CustomerAPI_RemoveFromGroupCom";

        private readonly ICustomerCommService _customerCommService;
        private readonly ICustomerListService _customerListService;
        private readonly IGroupComService _groupComService;
        private readonly ILicenseeDataAccess _licenseeDataAccess;
        private readonly IProductService _productService;

        #region Constructor

        public CustomerApiController()
        {
            _customerCommService = new CustomerCommService();
            _licenseeDataAccess = new LicenseeDataAccess();
            _customerListService = new CustomerListService();
            _groupComService = new GroupComService();
            _productService = new ProductService();
        }

        #endregion Constructor

        #region Level & Status API

        /// <summary>
        /// Update customer status.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="isClosed">Close customer if ISCLOSED equal true.</param>
        /// <param name="isSuspended">TClose customer if ISSUSPENDEND equal true.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public ActionResult UpdateCustStatus(int custId, bool isClosed, bool isSuspended)
        {
            IResultInfo result = new ResultInfo { Code = 1, Message = ViewResources.HttpError_Default };
            try
            {
                var licenseeSession = SessionService.LicenseeSession;
                var userSession = SessionService.UserSession;
                var allowLicensees = GetAllowLicenseesForStatusFunction();
                if (allowLicensees.Contains(licenseeSession.LicenseeId.ToString()))
                {
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

                    result = _customerListService.UpdateCustClosed(customerStatusMetaModel, userSession.Language);
                    if (result.Code == 0)
                    {
                        customerStatusMetaModel.IsOn = isSuspended;
                        result = _customerListService.UpdateCustSuspend(customerStatusMetaModel, userSession.Language);
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
                LogHelper.WriteError(exception);
            }

            return Json(result);
        }

        /// <summary>
        /// Update customer level.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="level">The customer level.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public ActionResult UpdateSpecialLevel(int custId, int level)
        {
            var isFakeLicensee = IsFakeLicensee();

            if (!isFakeLicensee)
            {
                var userSession = SessionService.UserSession;

                try
                {
                    var dataResult = _customerListService.UpdateSpecialLevel(
                        SessionService.LicenseeSession.LicenseeId,
                        userSession.UserId,
                        custId,
                        level,
                        Request.UserHostAddress,
                        userSession.Language);

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
                    LogHelper.WriteError(exception);
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

        /// <summary>
        /// Update customer group.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="oddsGroup">The customer group.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public ActionResult UpdateOddsGroup(int custId, string oddsGroup)
        {
            var isFakeLicensee = IsFakeLicensee();

            if (!isFakeLicensee)
            {
                var userSession = SessionService.UserSession;

                try
                {
                    var metaModel = new CustomerOddsGroupMetaModel
                    {
                        LicenseeId = SessionService.LicenseeSession.LicenseeId,
                        CustId = custId,
                        Ip = Request.UserHostAddress,
                        Language = userSession.Language,
                        OddsGroup = oddsGroup,
                        UserId = userSession.UserId
                    };
                    var dataResult = _customerListService.UpdateOddsGroup(metaModel);

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
                    LogHelper.WriteError(exception);
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
        public ActionResult UpdateBAStatus(int custId, bool isAllowBA)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
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
                bool isAllowUpdateBa;
                var isAllowUpdateBaResult = _licenseeDataAccess.IsAllowUpdateMemberBAStatus(request, licenseeSession.LicenseeId, out isAllowUpdateBa);
                if (!isAllowUpdateBa)
                {
                    return CreateUpdatingBaResult(isAllowBA, false, isAllowUpdateBaResult.Message);
                }
            }

            var updateResult = _customerListService.UpdateCustomerBAStatus(
                licenseeSession.LicenseeId,
                custId,
                userSession.UserId,
                isAllowBA,
                request);
            if (updateResult.IsSuccessful)
            {
                return CreateUpdatingBaResult(isAllowBA, true, updateResult.Message, updateResult.ExtendedData);
            }

            return CreateUpdatingBaResult(isAllowBA, false, updateResult.Message);
        }

        #endregion BA API

        #region Customer Comm API

        /// <summary>
        /// Update customer commission.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
        [SuppressMessage(
         "StyleCop.CSharp.StylecopPlus.Rules",
         "SP2101:MethodMustNotContainMoreLinesThan",
         Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult UpdateCustomerCom(UpdatedCustomerComMetaModel metaModel)
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
                                metaModel.RCComm,
                                metaModel.NGComm,
                                metaModel.LCComm,
                                metaModel.CSComm,
                                metaModel.ABComm);
                var userSession = SessionService.UserSession;

                var productAccess =
                    _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
                if (isvalid)
                {
                    metaModel.LicenseeId = licenseeSession.LicenseeId;
                    metaModel.UserId = userSession.UserId;
                    metaModel.Ip = this.Request.UserHostAddress;
                    metaModel.IsAllowedNG = productAccess.IsAllowNG;
                    metaModel.IsAllowedRC = productAccess.IsAllowRB;
                    metaModel.IsAllowedLC = productAccess.IsAllowLC;
                    metaModel.IsAllowedCS = productAccess.IsAllowCS;
                    metaModel.IsAllowedAB = productAccess.IsAllowAB;
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

        /// <summary>
        /// Remove customer from group commission.
        /// </summary>
        /// <param name="customerIds">Customer id list.</param>
        /// <param name="isRemovePlayercomm">Remove Player commission or not.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
        public ActionResult RemoveFromGroupCom(string customerIds, bool isRemovePlayercomm)
        {
            var licenseeSession = SessionService.LicenseeSession;
            if (!licenseeSession.IsAllowedCustomerComm)
            {
                return AccessDenied();
            }

            var result = new ResultViewModel
            {
                Success = false,
                ErrorMessage = ViewResources.Error
            };

            var metaModel = new RemoveFromGroupComMetaModel
            {
                CustomerIds = customerIds,
                IsRemovePlayercomm = isRemovePlayercomm
            };
            var userSession = SessionService.UserSession;

            var productAccess = _productService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                CustomerAPIRemoveFromGroupComName);
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            metaModel.Ip = Request.UserHostAddress;
            metaModel.Language = userSession.Language;
            var temp = _customerCommService.RemoveCustomersFromGroup(metaModel, productAccess.IsAllowCS);
            result.Success = temp.IsSuccessful;
            result.Message = ViewResources.MessageSuccess;
            result.ErrorMessage = temp.Message;
            result.ErrorCode = temp.Code;
            result.ExtendedData = new
            {
                SBComm = 0,
                RCComm = 0,
                NGComm = 0,
                LCComm = 0,
                RNGComm = 0,
                GroupName = ViewResources.None,
                GroupColor = "#ffffff",
            };

            return Json(result);
        }

        /// <summary>
        /// Move customer in a group commission.
        /// </summary>
        /// <param name="customerIds">Customer id list.</param>
        /// <param name="groupComId">Group commission id.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        [FunctionAuthorization(PermissionKeys.CustomerInfoCommissionGroupList)]
        public ActionResult MoveToGroupCom(string customerIds, int groupComId)
        {
            var licenseeSession = SessionService.LicenseeSession;
            if (!licenseeSession.IsAllowedCustomerComm)
            {
                return AccessDenied();
            }

            var userSession = SessionService.UserSession;
            var productAccess = _productService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                CustomerAPIRemoveFromGroupComName);

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

        [AjaxRequest]
        [HttpGet]
        public JsonResult SearchCustomer(string query)
        {
            var userName = query.Trim();
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userId = SessionService.UserSession.UserId;
            var customerNames = _customerListService.SearchCustomers(licenseeId, userId, query, SessionService.GenerateAgentListXml());

            var searchResult = new
            {
                data = new[] { string.Empty },
                query = userName,
                suggestions = customerNames
            };

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        #endregion Customer Comm API

        private static object CreateExtendedData(GroupCommMetaModel newGroup)
        {
            return new
            {
                SBComm = NumberHelper.DecFormat(newGroup.SBComm * 100, 2),
                RCComm = NumberHelper.DecFormat(newGroup.RCComm * 100, 2),
                NGComm = NumberHelper.DecFormat(newGroup.NGComm * 100, 2),
                LCComm = NumberHelper.DecFormat(newGroup.LCComm * 100, 2),
                RNGComm = NumberHelper.DecFormat(newGroup.CSComm * 100, 2),
                ABComm = NumberHelper.DecFormat(newGroup.ABComm * 100, 2),
                newGroup.GroupName,
                newGroup.GroupColor,
            };
        }

        private ActionResult AccessDenied()
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
            return oldGroup.NGComm == metaModel.NGComm && oldGroup.SBComm == metaModel.SBComm &&
                               oldGroup.RCComm == metaModel.RCComm && oldGroup.LCComm == metaModel.LCComm &&
                               oldGroup.CSComm == metaModel.CSComm && oldGroup.ABComm == metaModel.ABComm;
        }

        /// <summary>
        /// Process updating customer commission.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="result">The result view model.</param>
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
                RCComm = NumberHelper.DecFormat(metaModel.RCComm * 100, 2),
                NGComm = NumberHelper.DecFormat(metaModel.NGComm * 100, 2),
                LCComm = NumberHelper.DecFormat(metaModel.LCComm * 100, 2),
                RNGComm = NumberHelper.DecFormat(metaModel.CSComm * 100, 2),
                ABComm = NumberHelper.DecFormat(metaModel.ABComm * 100, 2),
                GroupName = ViewResources.None,
                GroupColor = "#ffffff",
            };
        }

        private ActionResult CreateUpdatingBaResult(bool isAllowBA, bool isSuccessful, string message, string oddsGroup = null)
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