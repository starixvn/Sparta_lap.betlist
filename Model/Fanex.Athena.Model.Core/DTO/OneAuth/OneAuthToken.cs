namespace Fanex.Athena.Model.Core.DTO.OneAuth
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class OneAuthToken
    {
        public int UserSEQ { get; set; }

        public string VendorID { get; set; }

        public string UserID { get; set; }

        public string Language { get; set; }

        public string ActionLink { get; set; }

        public string SiteName { get; set; }

        public bool IsAdminstrator { get; set; }

        public OneAuthCurrency[] Currency { get; set; }

        public OneAuthPermission[] Permissions { get; set; }
    }
}