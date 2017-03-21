// --------------------------------------------------------------------------
// <copyright file="LogController.LoginLog.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Collections.Generic;
    using System.Web.Http;

    public partial class LogController
    {
        [HttpGet]
        public dynamic LoginLog(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return new { rows = new List<string>() };
            }

            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            var agentListXml = SessionService.GenerateAgentListXml();
            var dataList = _logInLogService.GetDataList(licenseeId, username.Trim(), agentListXml);

            return new
            {
                rows = dataList
            };
        }
    }
}