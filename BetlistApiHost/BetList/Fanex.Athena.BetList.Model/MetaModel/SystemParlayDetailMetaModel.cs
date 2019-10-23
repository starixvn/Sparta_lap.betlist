// -----------------------------------------------------------------------
// <copyright file="SystemParlayDetailMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System;

    /// <summary>
    /// SystemParlayDetailMetaModel class.
    /// </summary>
    public class SystemParlayDetailMetaModel : BaseMetaModel
    {
        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public bool IsAllowedCustomerComm { get; set; }
    }
}