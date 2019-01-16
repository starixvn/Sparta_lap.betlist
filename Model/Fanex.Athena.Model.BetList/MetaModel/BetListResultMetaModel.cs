using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Model.BetList.MetaModel
{
    [Serializable]
    public class BetListResultMetaModel
    {
        public IDictionary<int, string> CurrencyList { get; set; }
        
        public BetListMetaModel MetaModel { get; set; }
        public string UspKey { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
}
