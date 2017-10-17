namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class NotificationViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;

        public Dictionary<string, string> NotificationTypes { get; set; }
    }
}