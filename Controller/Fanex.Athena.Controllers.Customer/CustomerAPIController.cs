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
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Customer;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.ViewModel;
    using ONEbook.UM.Filters;
    using Resources;

    #endregion Using

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class CustomerApiController : BaseController
    {
        private const string CustomerAPIRemoveFromGroupComName = "CustomerAPI_RemoveFromGroupCom";

        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IGroupComService _groupComService;
        private readonly ILicenseeService _licenseeService;
        private readonly IProductService _productService;

        #endregion Fields

        #region Constructor

        public CustomerApiController()
        {
            _licenseeService = new LicenseeService();
            _customerService = new CustomerService();
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

                    result = _customerService.UpdateCustClosed(customerStatusMetaModel, userSession.Language);
                    if (result.Code == 0)
                    {
                        customerStatusMetaModel.IsOn = isSuspended;
                        result = _customerService.UpdateCustSuspend(customerStatusMetaModel, userSession.Language);
                    }

                    if (result.Code > 0)
                    {
                        result.Message = ViewResources.HttpError_Default;
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
            var userSession = SessionService.UserSession;

            try
            {
                var dataResult = _customerService.UpdateSpecialLevel(
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

            return Json(new ResultViewModel
            {
                Success = false,
                Message = ViewResources.HttpError_Default,
                ErrorCode = 1,
                ErrorMessage = ViewResources.HttpError_Default
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
                var dataResult = _customerService.UpdateOddsGroup(metaModel);

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

            return Json(new ResultViewModel
            {
                Success = false,
                Message = ViewResources.HttpError_Default,
                ErrorCode = 1,
                ErrorMessage = ViewResources.HttpError_Default
            });
        }

        #endregion Level & Status API

        #region BA API

        /// <summary>
        /// Update customer BA status.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="isAllowBA">Allowed BA if ISALLOWBA equals true.</param>
        /// <returns>The action result.</returns>
        [SuppressMessage(
          "StyleCop.CSharp.StylecopPlus.Rules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
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
                return Json(new
                {
                    IsSuccessful = false,
                    Message = ViewResources.NotAllowBA,
                    IsBA = isAllowBA,
                    BAStatus = isAllowBA ? "On" : "Off",
                    ConfirmMessage = isAllowBA ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                    Tooltip = isAllowBA ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip
                });
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
                var isAllowUpdateBaResult = _licenseeService.IsAllowUpdateMemberBAStatus(request, licenseeSession.LicenseeId, out isAllowUpdateBa);
                if (!isAllowUpdateBa)
                {
                    return Json(new
                    {
                        IsSuccessful = false,
                        isAllowUpdateBaResult.Message,
                        IsBA = isAllowBA,
                        BAStatus = isAllowBA ? "On" : "Off",
                        ConfirmMessage = isAllowBA ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                        Tooltip = isAllowBA ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip
                    });
                }
            }

            // update
            var updateResult = _customerService.UpdateCustomerBAStatus(
                licenseeSession.LicenseeId,
                custId,
                userSession.UserId,
                isAllowBA,
                request);
            if (updateResult.IsSuccessful)
            {
                return Json(new
                {
                    IsSuccessful = true,
                    updateResult.Message,
                    IsBA = !isAllowBA,
                    BAStatus = isAllowBA ? "On" : "Off",
                    ConfirmMessage = isAllowBA ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                    Tooltip = isAllowBA ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip,
                    OddsGroup = updateResult.ExtendedData
                });
            }

            return this.Json(new
            {
                IsSuccessful = false,
                updateResult.Message,
                IsBA = isAllowBA,
                BAStatus = isAllowBA ? "On" : "Off",
                ConfirmMessage = isAllowBA ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                Tooltip = isAllowBA ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip
            });
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
        [SuppressMessage(
         "StyleCop.CSharp.StylecopPlus.Rules",
         "SP2101:MethodMustNotContainMoreLinesThan",
         Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult UpdateCustomerCom(UpdatedCustomerComMetaModel metaModel)
        {
            var result = new ResultViewModel
            {
                Success = false,
                ErrorMessage = ViewResources.HttpError_Default
            };
            if (metaModel.Customers != null && metaModel.Customers.Count > 0)
            {
                var commList = GroupCommSetting.Commissions;
                var isvalid = commList.Any(i => i.Value == metaModel.SBComm) &&
                              commList.Any(i => i.Value == metaModel.ParlayComm) &&
                              commList.Any(i => i.Value == metaModel.RCComm) &&
                              commList.Any(i => i.Value == metaModel.NGComm) &&
                              commList.Any(i => i.Value == metaModel.LCComm) &&
                              commList.Any(i => i.Value == metaModel.CSComm);
                var licenseeSession = SessionService.LicenseeSession;
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
                    metaModel.Language = userSession.Language;
                    /* Get oldGroup Commission to compare with new group commission.
                     * Check data for Update group commission
                     * exclude groupId = -1, 0 (get all group or get none group)*/
                    if (metaModel.GroupId > 0)
                    {
                        var oldGoup = this._groupComService.GetById(metaModel.GroupId);
                        var invalidComm = oldGoup.NGComm == metaModel.NGComm && oldGoup.SBComm == metaModel.SBComm &&
                                           oldGoup.RCComm == metaModel.RCComm && oldGoup.LCComm == metaModel.LCComm &&
                                           oldGoup.CSComm == metaModel.CSComm;
                        if (invalidComm)
                        {
                            result.Success = false;
                            result.ErrorMessage = ViewResources.MessageCommNoChange;
                        }
                        else
                        {
                            this.ProcessUpdatingCustomerCom(metaModel, result);
                        }
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
        /// Update group commission.
        /// </summary>
        /// <param name="groupCommId">The group commission id.</param>
        /// <param name="sbcomm">The sport-book commission.</param>
        /// <param name="rcomm">The racing commission.</param>
        /// <param name="ngcomm">The number game  commission.</param>
        /// <param name="lccomm">The live casino commission.</param>
        /// <param name="rngcomm">The RNG casino commission.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public ActionResult UpdateGroupCom(int groupCommId, decimal sbcomm, decimal rcomm, decimal ngcomm, decimal lccomm, decimal rngcomm)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var result = new ResultViewModel
            {
                Success = false,
                ErrorMessage = ViewResources.Error
            };
            var request = new Request
            {
                Language = userSession.Language,
                Ip = Request.UserHostAddress
            };

            /*Check valid data submited.*/
            var commList = GroupCommSetting.Commissions;
            var isvalid = commList.Any(i => i.Value == sbcomm) && commList.Any(i => i.Value == rcomm) &&
                          commList.Any(i => i.Value == ngcomm) && commList.Any(i => i.Value == lccomm) &&
                          commList.Any(i => i.Value == rngcomm);
            if (isvalid)
            {
                var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
                var currentGroup = _groupComService.GetById(groupCommId);
                if (currentGroup != null)
                {
                    currentGroup.LicenseeId = licenseeSession.LicenseeId;
                    currentGroup.SBComm = sbcomm;
                    currentGroup.RCComm = rcomm;
                    currentGroup.NGComm = ngcomm;
                    currentGroup.LCComm = lccomm;
                    currentGroup.CSComm = rngcomm;
                    currentGroup.IsUpdateNG = productAccess.IsAllowNG;
                    currentGroup.IsUpdateRacing = productAccess.IsAllowRB;
                    currentGroup.IsUpdateLC = productAccess.IsAllowLC;
                    currentGroup.IsUpdateCS = productAccess.IsAllowCS;

                    _groupComService.UpdateGroup(currentGroup, request, userSession, out result);

                    result.Message = result.Success ? ViewResources.MessageSuccess : ViewResources.Error;
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
        public ActionResult RemoveFromGroupCom(string customerIds, bool isRemovePlayercomm)
        {
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
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var productAccess = _productService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                CustomerAPIRemoveFromGroupComName);
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            metaModel.Ip = Request.UserHostAddress;
            metaModel.Language = userSession.Language;
            var temp = _customerService.RemoveCustomersFromGroup(metaModel, productAccess.IsAllowCS);
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
        public ActionResult MoveToGroupCom(string customerIds, int groupComId)
        {
            var metaModel = new MoveToGroupComMetaModel
            {
                CustomerIds = customerIds,
                GroupComId = groupComId
            };
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = _productService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                CustomerAPIRemoveFromGroupComName);
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            metaModel.IsAllowedNG = productAccess.IsAllowNG;
            metaModel.IsAllowedRC = productAccess.IsAllowRB;
            metaModel.IsAllowedLC = productAccess.IsAllowLC;
            metaModel.IsAllowedRNG = productAccess.IsAllowCS;
            metaModel.Ip = Request.UserHostAddress;
            metaModel.Language = userSession.Language;
            var temp = _customerService.MoveCustomersToGroup(metaModel);
            var result = new ResultViewModel
            {
                Success = temp.IsSuccessful,
                Message = ViewResources.MessageSuccess,
                ErrorMessage = temp.Message,
                ErrorCode = temp.Code
            };

            if (result.Success)
            {
                var groupService = new GroupComService();
                var newGroup = groupService.GetById(metaModel.GroupComId);
                if (newGroup != null)
                {
                    result.ExtendedData = new
                    {
                        SBComm = NumberHelper.DecFormat(newGroup.SBComm * 100, 2),
                        RCComm = NumberHelper.DecFormat(newGroup.RCComm * 100, 2),
                        NGComm = NumberHelper.DecFormat(newGroup.NGComm * 100, 2),
                        LCComm = NumberHelper.DecFormat(newGroup.LCComm * 100, 2),
                        RNGComm = NumberHelper.DecFormat(newGroup.CSComm * 100, 2),
                        newGroup.GroupName,
                        newGroup.GroupColor,
                    };
                }
            }

            return Json(result);
        }

        [AjaxRequest]
        [HttpGet]
        public JsonResult SearchCustomer(string query)
        {
            query = query.Trim();

            var customerNames = _customerService.SearchCustomers(SessionService.UserSession.UserId, query, SessionService.GenerateAgentListXml());

            var searchResult = new
            {
                data = new[] { string.Empty },
                query = query,
                suggestions = customerNames
            };

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        #endregion Customer Comm API

        /// <summary>
        /// Process updating customer commission.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="result">The result view model.</param>
        private void ProcessUpdatingCustomerCom(UpdatedCustomerComMetaModel metaModel, ResultViewModel result)
        {
            metaModel.Language = SessionService.UserSession.Language;
            var temp = _customerService.UpdateCustomersCommission(metaModel, metaModel.IsAllowedCS);
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
                GroupName = ViewResources.None,
                GroupColor = "#ffffff",
            };
        }
    }
}