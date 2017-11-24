namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class NotificationMetaModel
    {
        public int LicenseeId { get; set; }

        public string UserName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Type { get; set; }

        public string LicenseeName { get; set; }

        public bool IsOnlyUnread { get; set; }

        public string Language { get; set; }

        public bool IsValid => FromDate <= ToDate;
    }
}