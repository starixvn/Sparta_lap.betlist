// -----------------------------------------------------------------------
// <copyright file="AGCasinoMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;
    using Model.Core.MetaModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AGCasinoDetailMetaModel : BaseMetaModel
    {
        public long TransId { get; set; }

        public byte TransType { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
    }
}