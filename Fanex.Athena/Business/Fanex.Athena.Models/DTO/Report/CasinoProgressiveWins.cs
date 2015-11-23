// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWins.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System;
    using Fanex.Athena.Models.Cache;

    [Serializable]
    public class CasinoProgressiveWins
    {
        public int Index { get; set; }

        public DateTime WinlostDate { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public long RefNo { get; set; }

        public decimal WinAmount { get; set; }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public string TransDesc { get; set; }

        public int CurrencyId { get; set; }

        public int CustId { get; set; }
    }
}