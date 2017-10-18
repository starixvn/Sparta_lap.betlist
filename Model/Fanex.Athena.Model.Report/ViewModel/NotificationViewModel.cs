namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;

    public class NotificationViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;

        public IDictionary<string, string> NotificationTypes { get; set; }
    }
}