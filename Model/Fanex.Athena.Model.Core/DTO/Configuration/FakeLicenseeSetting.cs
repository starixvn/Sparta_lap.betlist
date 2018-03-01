using System;
using Fanex.Athena.Extension.Utility;

namespace Fanex.Athena.Model.Core.DTO.Configuration
{
    [Serializable]
    public class FakeLicenseeSetting
    {
        public string SiteName { get; set; }

        public bool DisableBetList { get; set; }

        public string MinDate { get; set; }

        public string MaxDate { get; set; }
    }
}