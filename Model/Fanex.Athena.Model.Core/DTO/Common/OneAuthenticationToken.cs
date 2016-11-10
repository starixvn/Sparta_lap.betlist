using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Model.Core.DTO.Common
{
    public class OneAuthenticationToken
    {
        public string SiteName { set; get; }
        public string UserName { set; get; }
        public string Language { get; set; }
        public IList<string> FunctionKeys { get; set; }
        public string ActionLink { get; set; }
    }
}
