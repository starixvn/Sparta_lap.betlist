using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Model.Core.DTO.Common
{
    [Serializable]
    public class LicenseeSiteName
    {
        public int LicenseeID { get; set; }
        public string SiteName { get; set; }
    }
}
