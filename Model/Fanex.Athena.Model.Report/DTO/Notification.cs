namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class Notification
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime DateTime { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }
    }
}
