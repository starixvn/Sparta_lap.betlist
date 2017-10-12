namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class NotificationMetaModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsValid => FromDate <= ToDate;
    }
}