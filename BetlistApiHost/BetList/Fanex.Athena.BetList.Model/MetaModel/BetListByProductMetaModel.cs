using System;

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class BetListByProductMetaModel : ICommonValueMetaModel
    {
        public string BaseCurrency { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int CustId { get; set; }

        public DateTime Date { get; set; }

        public string ProductCode { get; set; }
        public bool HasPlayerCom { get; set; }
        public int TransType { get; set; }
        public string AgentListXml { get; set; }
        public string Sort { get; set; } = "Transid ASC";
    }
}
