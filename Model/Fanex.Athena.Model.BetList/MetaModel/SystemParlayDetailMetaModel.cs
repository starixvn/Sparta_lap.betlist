using System;
using Fanex.Athena.Model.Core.MetaModel;

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class SystemParlayDetailMetaModel : BaseMetaModel
    {
        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
    }
}