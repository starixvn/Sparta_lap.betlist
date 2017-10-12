namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class NotificationReportController : ApiReportBaseController
    {
        [HttpPost]
        public dynamic Index(NotificationMetaModel metaModel)
        {
            if (metaModel.IsValid)
            {
                var mainData = new List<Notification>
                {
                    new Notification { Type = "abc" },
                    new Notification { Type = "dbc" }
                };

                return new
                {
                    Data = mainData
                };
            }

            return new
            {
                Data = new List<Notification>()
            };
        }
    }
}