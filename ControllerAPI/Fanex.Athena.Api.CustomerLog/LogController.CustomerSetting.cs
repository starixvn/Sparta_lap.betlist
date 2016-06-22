// --------------------------------------------------------------------------
// <copyright file="LogController.CustomerSetting.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;
    using Model.Core.DataDomain;

    public partial class LogController
    {
        /// <summary>
        /// Get customer setting's log.
        /// </summary>
        /// <param name="username">User name (customer).</param>
        /// <param name="viewbyid">View by id (login user).</param>
        /// <param name="year">Filter by year.</param>
        /// <param name="month">Filter by month.</param>
        /// <param name="fromday">Filter by From-day.</param>
        /// <param name="today">Filter by To-day.</param>
        /// <returns>A list of rows.</returns>
        [HttpGet]
        public dynamic CustomerSetting(string username, int viewbyid, int year, int month, int fromday, int today)
        {
            var metaModel = new CustomerLogMetaModel
            {
                UserName = string.IsNullOrEmpty(username) ? string.Empty : username.Trim(),
                ViewById = viewbyid,
                Year = year,
                Month = month,
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
                FromDay = fromday,
                ToDay = today
            };

            if (metaModel.IsValid)
            {
                var dataList = _customerLogService.GetCustomerSettingList(metaModel);
                var dataAfterRemoveHTMLTag = RemoveHtmlTag(dataList);
                var details = GetDetails(dataAfterRemoveHTMLTag);
                return new { rows = details };
            }

            return new { rows = new List<string>() };
        }
    }
}