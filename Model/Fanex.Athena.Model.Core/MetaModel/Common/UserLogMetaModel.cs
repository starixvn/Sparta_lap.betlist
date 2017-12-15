using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Model.Core.MetaModel.Common
{
    public class UserLogMetaModel
    {
        public int UserId { get; set; }

        public int CustID { get; set; }

        public string SPName { get; set; }

        public int ActionID { get; set; }

        public string TrackingValue1 { get; set; }

        public string TrackingValue2 { get; set; }
    }
}
