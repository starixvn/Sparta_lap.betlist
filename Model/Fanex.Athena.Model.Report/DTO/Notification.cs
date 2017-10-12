namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class Notification
    {
        public string Type { get; set; }

        public DateTime DateTime { get; set; }

        public string Content { get; set; }
    }
}