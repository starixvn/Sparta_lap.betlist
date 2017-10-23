namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class NotificationMetaModel : BaseReportMetaModel
    {
        public int Type { get; set; }

        public string LicenseeName { get; set; }

        public bool IsOnlyUnread { get; set; }

        public string Language { get; set; }

        public override bool IsValid => FromDate <= ToDate;
    }
}