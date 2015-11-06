// --------------------------------------------------------------------------
// <copyright file="LogController.LoginLog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Log in log's API controller.
    /// </summary>
    public partial class LogController
    {
        /// <summary>
        /// Get Log in log data list.
        /// </summary>
        /// <param name="username">Filter by user name.</param>
        /// <returns>Data list of user name.</returns>
        [HttpGet]
        [AjaxApiFunctionAuthorization(ParentUrl = CustomerLogUrl)]
        public dynamic LoginLog(string username)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            username = string.IsNullOrEmpty(username) ? string.Empty : username.Trim();
            if (!string.IsNullOrEmpty(username))
            {
                var dataList = _logInLogService.GetDataList(licenseeId, username);

                return new
                {
                    rows = dataList
                };
            }

            return new { rows = new List<string>() };
        }
    }
}