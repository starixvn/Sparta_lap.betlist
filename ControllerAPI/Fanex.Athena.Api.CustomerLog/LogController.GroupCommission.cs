// --------------------------------------------------------------------------
// <copyright file="LogController.GroupCommission.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    /// <summary>
    /// Log in log's API controller.
    /// </summary>
    public partial class LogController
    {
        /// <summary>
        /// Group commission's Log.
        /// </summary>
        /// <param name="year">Filter by year.</param>
        /// <param name="month">Filter by month.</param>
        /// <param name="fromday">Filter by from day.</param>
        /// <param name="today">Filter by today.</param>
        /// <returns>The list of rows.</returns>
        [HttpGet]
        public dynamic GroupCommission(int year, int month, int fromday, int today)
        {
            var metaModel = new CustomerLogMetaModel();

            metaModel.Year = year;
            metaModel.Month = month;
            metaModel.FromDay = fromday;
            metaModel.ToDay = today;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            var dataList = _customerLogService.GetGroupCommList(metaModel);
            var dataAfterRemoveHTMLTag = RemoveHtmlTag(dataList);

            var details = GetDetails(dataAfterRemoveHTMLTag);

            return new { rows = details };
        }
    }
}