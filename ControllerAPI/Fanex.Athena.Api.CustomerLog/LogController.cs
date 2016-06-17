// --------------------------------------------------------------------------
// <copyright file="LogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web.Hosting;
    using System.Xml;
    using Base;
    using Base.Attributes;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.ViewModel.CustomerLog;
    using Fanex.Athena.Service.Customer;
    using Model.Core.DataDomain;
    using RazorEngine;
    using RazorEngine.Templating;
    using Resources;

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerLog)]
    public partial class LogController : BaseApiController
    {
        #region Field

        /// <summary>
        /// Customer log service .
        /// </summary>
        private readonly CustomerLogService _customerLogService;

        /// <summary>
        /// Log in log service .
        /// </summary>
        private readonly LogInLogService _logInLogService;

        /// <summary>
        /// Resources Map.
        /// </summary>
        private IDictionary<string, string> _resourcesMap;

        #endregion Field

        #region Constructor

        public LogController()
        {
            _customerLogService = new CustomerLogService();
            _logInLogService = new LogInLogService();
        }

        #endregion Constructor

        #region Utility

        #region Remove HTML tag

        /// <summary>
        /// Remove HTML tag inside tracking value.
        /// </summary>
        /// <param name="source">Tracking Value.</param>
        /// <returns>Tracking Value has been removed HTML Tag.</returns>
        private static string StripTags(string source)
        {
            source = source.Replace(@"/", string.Empty);
            source = source.Replace(@"<", string.Empty);
            source = source.Replace(@">", string.Empty);

            return source;
        }

        #endregion Remove HTML tag

        #region Parse dictonary to razor view

        /// <summary>
        /// Parse dictionary to razor view.
        /// </summary>
        /// <param name="actionId">Action id.</param>
        /// <param name="viewModel">View Model.</param>
        /// <returns> HTML string.</returns>
        private static string ParseDictonaryToRazorView(int actionId, ActionViewModel viewModel)
        {
            var templatePath = HostingEnvironment.MapPath(string.Format("~/Views/Customer/CustomerLog/Action/{0}.cshtml", actionId));
            var template = File.ReadAllText(templatePath);
            return Engine.Razor.RunCompile(template, actionId.ToString(), null, viewModel);
        }

        #endregion Parse dictonary to razor view

        #region Get XML attribute

        /// <summary>
        /// Get XML attribute.
        /// </summary>
        /// <param name="trackingValue">Tracking value.</param>
        /// <param name="trackingValueOld">Tracking value old.</param>
        /// <returns>XML attributes.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static Dictionary<string, string> GetXmlAttr(string trackingValue, string trackingValueOld)
        {
            var dictonary = new Dictionary<string, string>();
            var actionTypes = new List<string>
            {
                "Closed",
                "Suspended",
                "SpecialLevel",
                "GroupCommID",
                "GroupName",
                "GroupColor",
                "SBComm",
                "RacingComm",
                "NGComm",
                "LCComm",
                "RNGComm"
            };
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

            XmlElement root = doc.DocumentElement;
            XmlElement rootOld = docOld.DocumentElement;
            foreach (var attr in actionTypes)
            {
                if (root != null && root.HasAttribute(attr))
                {
                    var value = root.GetAttribute(attr);
                    dictonary.Add(attr, value);
                }
                else
                {
                    dictonary.Add(attr, string.Empty);
                }

                if (rootOld != null && rootOld.HasAttribute(attr))
                {
                    var value = rootOld.GetAttribute(attr);
                    dictonary.Add(attr + "Old", value);
                }
                else
                {
                    dictonary.Add(attr + "Old", string.Empty);
                }
            }

            return dictonary;
        }

        #endregion Get XML attribute

        #endregion Utility

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
                    item.TrackingValue = "<" + StripTags(trackingValue) + "/>";
                    item.TrackingValueOld = "<" + StripTags(trackingValueOld) + "/>";
                }
            }

            return datalist;
        }

        #region Get detail data

        /// <summary>
        /// Get details column.
        /// </summary>
        /// <param name="dataList">A list of customer log.</param>
        /// <returns>A list has been formatted.</returns>
        private IList<CustomerLog> GetDetails(IList<CustomerLog> dataList)
        {
            _resourcesMap = new Dictionary<string, string>
            {
                { "Closed", ViewResources.Closed },
                { "Suspended", ViewResources.Suspended },
                { "SpecialLevel", ViewResources.SpecialLevel },
                { "GroupId", ViewResources.GroupId },
                { "GroupName", ViewResources.GroupName },
                { "GroupColor", ViewResources.GroupColor },
                { "SBComm", ViewResources.SBComm },
                { "RBComm", ViewResources.RBComm },
                { "NGComm", ViewResources.NGComm },
                { "LCComm", ViewResources.LCComm },
                { "CSComm", ViewResources.CS + " " + ViewResources.Comm },
                { "Yes", ViewResources.Yes },
                { "No", ViewResources.No },
                { "OldColor", ViewResources.OldColor }
            };
            foreach (var item in dataList)
            {
                // No need to read xml data for BA
                if (item.ActionId == 2401 || item.ActionId == 2403 || item.ActionId == 2801)
                {
                    item.Details = string.Empty;
                    continue;
                }

                var dictonary = GetXmlAttr(item.TrackingValue, item.TrackingValueOld);

                item.Details = ParseDictonaryToRazorView(item.ActionId, new ActionViewModel { Data = dictonary, Resource = _resourcesMap });
            }

            return dataList;
        }

        #endregion Get detail data
    }
}