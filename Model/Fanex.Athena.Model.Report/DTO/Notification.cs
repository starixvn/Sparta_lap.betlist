namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class Notification
    {
        public int MessageId { get; set; }

        public int MessageTypeId { get; set; }

        public DateTime DateTime { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}