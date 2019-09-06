using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class BetListByProductMetaModel : ICommonValueMetaModel
    {
        public string BaseCurrency { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int CustId { get; set; }

        public DateTime WinLossDate { get; set; }

        public string ProductCode { get; set; }
        public IDictionary<int, string> UserCurrencyList { get; set; }
        public bool HasPlayerCom { get; set; }
    }
}
