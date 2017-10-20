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
                    new Notification { Type = "abc", Subject = "Subject 1", Content = "this is content of subject 1" },
                    new Notification { Type = "dbc", Subject = "Subject 2", Content = null  },
                    new Notification { Type = "dbc", Subject = "Subject 3", Content = ""  }
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