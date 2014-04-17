// -----------------------------------------------------------------------
// <copyright file="CustomerApiController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using DNAide.Utils;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Customer;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Customer;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Customer;
    using Fanex.Athena.Service.Interface.Users;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Logging;

    #endregion Using

    /// <summary>
    /// Customer API.
    /// +Update BA.
    /// +Update level.
    /// +Update Status .
    /// </summary>
    public class CustomerApiController : ReadonlyBaseController
    {
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
                LogService.Error(exception.Message, exception);
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
        public ActionResult UpdateSpecialLevel(int custId, int level)
        {
            var result = new ResultViewModel
            {
                Success = false,
                Message = ViewResources.HttpError_Default,
                ErrorCode = 1,
                ErrorMessage = ViewResources.HttpError_Default
            };
            try
            {
                var dataResult = _customerService.UpdateSpecialLevel(
                    SessionService.UserSession.UserId,
                    custId,
                    level,
                    Request.UserHostAddress,
                    SessionService.UserSession.Language);
                result = new ResultViewModel
                {
                    Success = dataResult.IsSuccessful,
                    Message = dataResult.Message,
                    ErrorCode = dataResult.Code,
                    ErrorMessage = dataResult.Message
                };

                return Json(result);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
            }

            return Json(result);
        }

        #endregion Level & Status API

        #region BA API

        /// <summary>
        /// Update customer BA status.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="isAllowBa">Allowed BA if ISALLOWBA equals true.</param>        
        /// <returns>The action result.</returns>
        [SuppressMessage(
          "StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [AjaxRequest]
        public ActionResult UpdateBaStatus(int custId, bool isAllowBa)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            if (!productAccess.IsAllowBa)
            {
                return Json(new
                    {
                        IsSuccessful = false,
                        Message = ViewResources.NotAllowBA,
                        IsBA = isAllowBa,
                        BAStatus = isAllowBa ? "On" : "Off",
                        ConfirmMessage = isAllowBa ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                        Tooltip = isAllowBa ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip
                    });
            }

            var request = new Request
                {
                    Language = SessionService.UserSession.Language,

                    Ip = Request.UserHostAddress
                };

            var specialBaPermission = SessionService.UserSession.IsAllowedSpecialBA;
            if (!specialBaPermission)
            {
                bool isAllowUpdateBa;
                var isAllowUpdateBaResult = _licenseeService.IsAllowUpdateMemberBaStatus(request, licenseeSession.LicenseeId, out isAllowUpdateBa);
                if (!isAllowUpdateBa)
                {
                    return Json(new
                        {
                            IsSuccessful = false,
                            isAllowUpdateBaResult.Message,
                            IsBA = isAllowBa,
                            BAStatus = isAllowBa ? "On" : "Off",
                            ConfirmMessage = isAllowBa ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                            Tooltip = isAllowBa ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip
                        });
                }
            }

            // update
            var updateResult = _customerService.UpdateCustomerBaStatus(
                licenseeSession.LicenseeId,
                custId,
                SessionService.UserSession.UserId,
                isAllowBa,
                request);
            if (updateResult.IsSuccessful)
            {
                return Json(new
                    {
                        IsSuccessful = true,
                        updateResult.Message,
                        IsBA = !isAllowBa,
                        BAStatus = isAllowBa ? "On" : "Off",
                        ConfirmMessage = isAllowBa ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                        Tooltip = isAllowBa ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip
                    });
            }

            return this.Json(new
                {
                    IsSuccessful = false,
                    updateResult.Message,
                    IsBA = isAllowBa,
                    BAStatus = isAllowBa ? "On" : "Off",
                    ConfirmMessage = isAllowBa ? ViewResources.AlertTurnOffBAMsg : ViewResources.AlertTurnOnBAMsg,
                    Tooltip = isAllowBa ? ViewResources.TurnOffBaTooltip : ViewResources.TurnOnBaTooltip
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
        [SuppressMessage(
         "StyleCop.CSharp.Nexcel.NexcelCustomRules",
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
                var commList = GroupCommSetting.GetValues();
                var isvalid = commList.Any(i => i.Value == metaModel.SBComm) &&
                              commList.Any(i => i.Value == metaModel.RCComm) &&
                              commList.Any(i => i.Value == metaModel.NGComm) &&
                              commList.Any(i => i.Value == metaModel.LCComm) &&
                              commList.Any(i => i.Value == metaModel.CSComm);
                var licenseeSession = SessionService.LicenseeSession;
                var productAccess =
                    _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
                if (isvalid)
                {
                    metaModel.LicenseeId = licenseeSession.LicenseeId;
                    metaModel.UserId = this.SessionService.UserSession.UserId;
                    metaModel.Ip = this.Request.UserHostAddress;
                    metaModel.IsAllowedNG = productAccess.IsAllowNg;
                    metaModel.IsAllowedRC = productAccess.IsAllowRb;
                    metaModel.IsAllowedLC = productAccess.IsAllowLc;
                    metaModel.IsAllowedCS = productAccess.IsAllowCs;
                    metaModel.Language = this.SessionService.UserSession.Language;
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
        public ActionResult UpdateGroupCom(int groupCommId, decimal sbcomm, decimal rcomm, decimal ngcomm, decimal lccomm, decimal rngcomm)
        {
            var result = new ResultViewModel
                {
                    Success = false,
                    ErrorMessage = ViewResources.Error
                };
            var request = new Request
                {
                    Language = SessionService.UserSession.Language,
                    Ip = Request.UserHostAddress
                };

            /*Check valid data submited.*/
            var commList = GroupCommSetting.GetValues();
            var isvalid = commList.Any(i => i.Value == sbcomm) && commList.Any(i => i.Value == rcomm) &&
                          commList.Any(i => i.Value == ngcomm) && commList.Any(i => i.Value == lccomm) &&
                          commList.Any(i => i.Value == rngcomm);
            if (isvalid)
            {
                var licenseeSession = SessionService.LicenseeSession;
                var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
                var currentGroup = _groupComService.GetById(groupCommId);
                if (currentGroup != null)
                {
                    currentGroup.LicenseeId = licenseeSession.LicenseeId;
                    currentGroup.SBComm = sbcomm;
                    currentGroup.RCComm = rcomm;
                    currentGroup.NGComm = ngcomm;
                    currentGroup.LCComm = lccomm;
                    currentGroup.CSComm = rngcomm;
                    currentGroup.IsUpdateNG = productAccess.IsAllowNg;
                    currentGroup.IsUpdateRacing = productAccess.IsAllowRb;
                    currentGroup.IsUpdateLC = productAccess.IsAllowLc;
                    currentGroup.IsUpdateCS = productAccess.IsAllowCs;

                    _groupComService.UpdateGroup(currentGroup, request, SessionService.UserSession, out result);

                    result.Message = result.Success ? ViewResources.MessageSuccess : ViewResources.Error;
                }
            }

            return Json(result);
        }

        /// <summary>
        /// Remove customer from group commission.
        /// </summary>
        /// <param name="customers">Customer id list.</param>
        /// <param name="isRemovePlayercomm">Remove Player commission or not.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult RemoveFromGroupCom(IList<int> customers, bool isRemovePlayercomm)
        {
            var result = new ResultViewModel
                {
                    Success = false,
                    ErrorMessage = ViewResources.Error
                };

            var metaModel = new RemoveFromGroupComMetaModel()
            {
                Customers = customers,
                IsRemovePlayercomm = isRemovePlayercomm
            };
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreKeno);
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.Ip = Request.UserHostAddress;
            metaModel.Language = SessionService.UserSession.Language;
            var temp = _customerService.RemoveCustomersFromGroup(metaModel, productAccess.IsAllowCs);
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
        /// <param name="customers">Customer id list.</param>
        /// <param name="groupComId">Group commission id.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult MoveToGroupCom(IList<int> customers, int groupComId)
        {
            var result = new ResultViewModel
                {
                    Success = false,
                    ErrorMessage = ViewResources.Error
                };

            var metaModel = new MoveToGroupComMetaModel
            {
                Customers = customers,
                GroupComId = groupComId
            };
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreKeno);
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.IsAllowedNG = productAccess.IsAllowNg;
            metaModel.IsAllowedRC = productAccess.IsAllowRb;
            metaModel.IsAllowedLC = productAccess.IsAllowLc;
            metaModel.IsAllowedRNG = productAccess.IsAllowCs;
            metaModel.Ip = Request.UserHostAddress;
            metaModel.Language = SessionService.UserSession.Language;

            var temp = _customerService.MoveCustomersToGroup(metaModel);
            result.Success = temp.IsSuccessful;
            result.Message = ViewResources.MessageSuccess;
            result.ErrorMessage = temp.Message;
            result.ErrorCode = temp.Code;

            if (result.Success)
            {
                var groupService = new GroupComService();
                var newGroup = groupService.GetById(metaModel.GroupComId);
                if (newGroup != null)
                {
                    result.ExtendedData = new
                        {
                            SBComm = NumberFormat.DecFormat(newGroup.SBComm * 100, 2, false),
                            RCComm = NumberFormat.DecFormat(newGroup.RCComm * 100, 2, false),
                            NGComm = NumberFormat.DecFormat(newGroup.NGComm * 100, 2, false),
                            LCComm = NumberFormat.DecFormat(newGroup.LCComm * 100, 2, false),
                            RNGComm = NumberFormat.DecFormat(newGroup.CSComm * 100, 2, false),
                            newGroup.GroupName,
                            newGroup.GroupColor,
                        };
                }
            }

            return Json(result);
        }

        /// <summary>
        /// Search customers.
        /// </summary>
        /// <param name="licenseeType">The licensee type.</param>
        /// <param name="currencyId">The currency id.</param>
        /// <param name="query">The query string.</param>
        /// <returns>The action result.</returns>
        public ActionResult SearchCustomer(string licenseeType, int currencyId, string query)
        {
            query = query.Trim();
            var licenseeSession = SessionService.LicenseeSession;
            var result = _customerService.QuickSearch(SessionService.UserSession.UserId, query, licenseeSession.LicenseeId, licenseeType, currencyId);

            var suggestion = new StringCollection();
            foreach (DataRow row in result.Rows)
            {
                suggestion.Add(row[0].ToString());
            }

            var lstUsername = new
                {
                    data = new[] { string.Empty },
                    query = query,
                    suggestions = suggestion
                };

            return Json(lstUsername, JsonRequestBehavior.AllowGet);
        }

        #endregion Customer Comm API

        /// <summary>
        /// Process updating customer commission.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="result">The result view model.</param>
        [NonAction]
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
                SBComm = NumberFormat.DecFormat(metaModel.SBComm * 100, 2, false),
                RCComm = NumberFormat.DecFormat(metaModel.RCComm * 100, 2, false),
                NGComm = NumberFormat.DecFormat(metaModel.NGComm * 100, 2, false),
                LCComm = NumberFormat.DecFormat(metaModel.LCComm * 100, 2, false),
                RNGComm = NumberFormat.DecFormat(metaModel.CSComm * 100, 2, false),
                GroupName = ViewResources.None,
                GroupColor = "#ffffff",
            };
        }
    }
}