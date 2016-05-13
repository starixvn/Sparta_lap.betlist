// -----------------------------------------------------------------------
// <copyright file="MicroGamingRngDetailMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.BetList
{
    using System;
    using Model.Core.MetaModel;

    /// <summary>
    /// MicroGamingRngDetailMetaModel class.
    /// </summary>
    public class MicroGamingRngDetailMetaModel : BaseMetaModel
    {
        public long TransId { get; set; }

        public byte TransType { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
    }
}