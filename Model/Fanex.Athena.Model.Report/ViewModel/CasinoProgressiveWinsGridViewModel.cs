// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWins.cs" company="">
//
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;

    public class CasinoProgressiveWinsGridViewModel
    {
        public int Index { get; set; }

        public DateTime WinlostDate { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public long RefNo { get; set; }

        public string Choice { get; set; }

        public decimal WinAmount { get; set; }

        public int CustId { get; set; }

        public int BetType { get; set; }

        public string GameId { get; set; }
    }
}