// --------------------------------------------------------------------------
// <copyright file="LogController.GroupCommission.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.CustomerLog
{
    using System.Web.Http;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    public partial class LogController
    {
        [HttpGet]
        public dynamic GroupCommission(int year, int month, int fromday, int today)
        {
            var metaModel = new CustomerLogMetaModel
            {
                Year = year,
                Month = month,
                FromDay = fromday,
                ToDay = today,
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
                SiteName = SessionService.LicenseeSession.LicenseeName
            };

            var dataList = _customerLogDataAccess.GetGroupCommList(metaModel);
            var dataAfterRemoveHTMLTag = RemoveHtmlTag(dataList);

            var details = GetDetails(dataAfterRemoveHTMLTag);

            return new { rows = details };
        }
    }
}