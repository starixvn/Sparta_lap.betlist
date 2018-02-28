using System;
using Fanex.Athena.Extension.Utility;

namespace Fanex.Athena.Model.Core.DTO.Configuration
{
    [Serializable]
    public class ReportSetting
    {
        public string ReportKey { get; set; }

        public string MinDateString { get; set; }

        public string MaxDateString { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public bool IsDisableBetList { get; set; }
    }
}