using System;

namespace Fanex.Athena.Model.Core.DTO.Common
{
    [Serializable]
    public class OneAuthenticationToken
    {
        public int UserSEQ { get; set; }

        public string VendorID { get; set; }

        public string UserID { get; set; }

        public string Language { get; set; }

        public string ActionLink { get; set; }

        public string SiteName { get; set; }
    }
}