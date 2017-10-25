namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class ResultAmendment
    {
        public DateTime DateTime { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string DataFilePath { get; set; }
    }
}