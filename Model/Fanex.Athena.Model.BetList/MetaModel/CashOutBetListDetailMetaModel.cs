// -----------------------------------------------------------------------
// <copyright file="CashOutBetListDetailMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class CashOutBetListDetailMetaModel
    {
        public string BaseCurrrency { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public int BetType { get; set; }

        public long OriginalTransId { get; set; }

        public string AgentListXml { get; set; }
    }
}