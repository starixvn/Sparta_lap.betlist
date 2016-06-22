// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWins.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

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

        public string GameId { get; set; }
    }
}