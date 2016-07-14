// -----------------------------------------------------------------------
// <copyright file="SystemParlayDetailMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;
    using Model.Core.MetaModel;

    /// <summary>
    /// SystemParlayDetailMetaModel class.
    /// </summary>
    public class SystemParlayDetailMetaModel : BaseMetaModel
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public long BetId { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
    }
}