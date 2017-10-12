namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;

    public class NotificationViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;
    }
}