using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public interface ICommonValueMetaModel
    {
        string BaseCurrency { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int CustId { get; set; }
    }
}
