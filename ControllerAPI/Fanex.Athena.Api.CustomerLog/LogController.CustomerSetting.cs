// --------------------------------------------------------------------------
// <copyright file="LogController.CustomerSetting.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    public partial class LogController
    {
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
                ToDay = today,
                AgentListXml = SessionService.GenerateAgentListXml()
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