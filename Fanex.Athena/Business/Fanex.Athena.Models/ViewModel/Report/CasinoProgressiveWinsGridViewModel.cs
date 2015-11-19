// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWins.cs" company="Nexcel">
// 
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
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
    }
}