// --------------------------------------------------------------------------
// <copyright file="LogController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Hosting;
    using System.Xml;
    using Base;
    using Base.Attributes;
    using DataAccess.Customer;
    using DataAccess.Customer.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.ViewModel.CustomerLog;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using RazorEngine;
    using RazorEngine.Templating;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Session.Enum;

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerLog)]
    public partial class LogController : BaseApiController
    {
        private static readonly int[] ActionIdListForBA = new[] { 2401, 2403, 2801 };

        private static readonly string[] ActionTypes = new[]
            {
                "Closed",
                "Suspended",
                "SpecialLevel",
                "GroupCommID",
                "GroupName",
                "GroupColor",
                "SBComm",
                "NGComm",
                "RNGComm"
            };

        private readonly ICustomerLogDataAccess _customerLogDataAccess;

        private readonly ILogInLogDataAccess _logInLogDataAccess;

        private readonly IAppSettings _appSettings;
        private readonly IClientViewHelper _clientViewHelper;
        
        public LogController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            ICustomerLogDataAccess customerLogDataAccess,
            ILogInLogDataAccess logInLogDataAccess,
            IAppSettings appSettings,
            IClientViewHelper clientViewHelper)
        : base(sessionService, productService, languageService)
        {
            _customerLogDataAccess = customerLogDataAccess;
            _logInLogDataAccess = logInLogDataAccess;
            _appSettings = appSettings;
            _clientViewHelper = clientViewHelper;
        }

        #region Utility

        #region Remove HTML tag

        /// <summary>
        /// Remove HTML tag inside tracking value.
        /// </summary>
        /// <param name="source">Tracking Value.</param>
        /// <returns>Tracking Value has been removed HTML Tag.</returns>
        private static string RemoveHtmlTags(string source)
        {
            var regex = new Regex("[/<>]");
            return regex.Replace(source, string.Empty);
        }

        /// <summary>
        /// Remove HTML Tag.
        /// </summary>
        /// <param name="datalist">A list of Customer Log.</param>
        /// <returns>A list of customer log has been removed HTML tag.</returns>
        private static IList<CustomerLog> RemoveHtmlTag(IList<CustomerLog> datalist)
        {
            foreach (var item in datalist)
            {
                var trackingCharCount = item.TrackingValue.Length;
                var trackingOldCharCount = item.TrackingValueOld.Length;

                // Remove HTML tag inside tracking value
                if (trackingCharCount > 0 && trackingOldCharCount > 0)
                {
                    var trackingValue = item.TrackingValue.Substring(1, trackingCharCount - 3);
                    var trackingValueOld = item.TrackingValueOld.Substring(1, trackingOldCharCount - 3);

                    // Add </> tag after remove all html tag
                    item.TrackingValue = "<" + RemoveHtmlTags(trackingValue) + "/>";
                    item.TrackingValueOld = "<" + RemoveHtmlTags(trackingValueOld) + "/>";
                }
            }

            return datalist;
        }

        #endregion Remove HTML tag

        #region Get XML attribute

        private static IDictionary<string, string> GetXmlAttr(string trackingValue, string trackingValueOld)
        {
            var dictonary = new Dictionary<string, string>();

            var doc = new XmlDocument();
            var docOld = new XmlDocument();
            if (!string.IsNullOrEmpty(trackingValue))
            {
                doc.LoadXml(trackingValue);
            }

            if (!string.IsNullOrEmpty(trackingValueOld))
            {
                docOld.LoadXml(trackingValueOld);
            }

            var root = doc.DocumentElement;
            var rootOld = docOld.DocumentElement;
            foreach (var attr in ActionTypes)
            {
                dictonary.Add(attr, GetAttributeValue(root, attr));
                dictonary.Add(attr + "Old", GetAttributeValue(rootOld, attr));
            }

            return dictonary;
        }

        private static string GetAttributeValue(XmlElement root, string attribute)
        {
            return root != null && root.HasAttribute(attribute) ?
                root.GetAttribute(attribute) :
                string.Empty;
        }

        #endregion Get XML attribute

        #region Parse dictonary to razor view

        /// <summary>
        /// Parse dictionary to razor view.
        /// </summary>
        /// <param name="actionId">Action id.</param>
        /// <param name="viewModel">View Model.</param>
        /// <returns> HTML string.</returns>
        private string GenerateDetailedValue(int actionId, ActionViewModel viewModel)
        {
            var viewOption = GetActionViewName(actionId);
            if (viewOption.IsNavigationView)
            {
                return string.Empty;
            }

            var templatePath = HostingEnvironment.MapPath(viewOption.ViewPath);
            var template = File.ReadAllText(templatePath);
            return Engine.Razor.RunCompile(template, viewOption.ViewPath, model: viewModel);
        }

        private ViewByOption GetActionViewName(int actionId)
        {
            var actionView = $"~/Views/Customer/CustomerLog/Action/{actionId}.cshtml";
            var userSession = SessionService.UserSession;

            var option = userSession != null && userSession.ViewType != ClientViewType.None ?
                userSession.ViewType : (ClientViewType)int.Parse(_appSettings.ClientViewType);

            return _clientViewHelper.GetViewPath(actionView, option);
        }

        #endregion Parse dictonary to razor view

        #endregion Utility

        #region Get detail data

        private IList<CustomerLog> GetDetails(IList<CustomerLog> dataList)
        {
            var resourcesMap = new Dictionary<string, string>
            {
                { "Closed", ViewResources.Closed },
                { "Suspended", ViewResources.Suspended },
                { "SpecialLevel", ViewResources.SpecialLevel },
                { "GroupId", ViewResources.GroupId },
                { "GroupName", ViewResources.GroupName },
                { "GroupColor", ViewResources.GroupColor },
                { "SBComm", ViewResources.SBComm },
                { "NGComm", ViewResources.NGComm },
                { "CSComm", ViewResources.CSComm },
                { "Yes", ViewResources.Yes },
                { "No", ViewResources.No },
                { "OldColor", ViewResources.OldColor }
            };

            foreach (var item in dataList)
            {
                // No need to read xml data for BA
                if (ActionIdListForBA.Contains(item.ActionId))
                {
                    item.Details = string.Empty;
                }
                else
                {
                    var dictionary = GetXmlAttr(item.TrackingValue, item.TrackingValueOld);
                    item.Details = GenerateDetailedValue(item.ActionId, new ActionViewModel { Data = dictionary, Resource = resourcesMap });
                }
            }

            return dataList;
        }

        #endregion Get detail data
    }
}