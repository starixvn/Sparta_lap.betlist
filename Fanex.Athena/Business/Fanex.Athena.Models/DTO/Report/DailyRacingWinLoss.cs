// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    /// <summary>
    /// DailyRacingWinLoss class.
    /// </summary>
    public class DailyRacingWinLoss
    {
        #region Property common

        public int LeagueId { get; set; }

        public string CurrencyName { get; set; }

        public int Currency { get; set; }

        public int RaceNo { get; set; }

        #endregion Property common

        #region Customer Info

        #region depend on base currency

        public decimal WinTO { get; set; }

        public decimal PlaceTO { get; set; }

        public decimal Wpto { get; set; }

        public decimal TotalTO { get; set; }

        public decimal DividendWon { get; set; }

        public decimal CustomerCommission { get; set; }

        public decimal CommIncome { get; set; }

        public decimal AmountDueToRacing { get; set; }

        #endregion depend on base currency

        #region not depend on base currency

        public decimal WinToMYR { get; set; }

        public decimal PlaceToMYR { get; set; }

        public decimal WptoMYR { get; set; }

        public decimal TotalToMYR { get; set; }

        public decimal DividendWonMYR { get; set; }

        public decimal CustomerCommissionMYR { get; set; }

        public decimal CommIncomeMYR { get; set; }

        public decimal AmountDueToRacingMYR { get; set; }

        public decimal WinToEur { get; set; }

        public decimal PlaceToEur { get; set; }

        public decimal WptoEUR { get; set; }

        public decimal TotalToEur { get; set; }

        public decimal DividendWonEUR { get; set; }

        public decimal CustomerCommissionEUR { get; set; }

        public decimal CommIncomeEUR { get; set; }

        public decimal AmountDueToRacingEUR { get; set; }

        public decimal WinToUSD { get; set; }

        public decimal PlaceToUSD { get; set; }

        public decimal WptoUSD { get; set; }

        public decimal TotalToUSD { get; set; }

        public decimal DividendWonUSD { get; set; }

        public decimal CustomerCommissionUSD { get; set; }

        public decimal CommIncomeUSD { get; set; }

        public decimal AmountDueToRacingUSD { get; set; }

        #endregion not depend on base currency

        #endregion Customer Info

        #region 12Bet Licensee

        public decimal WinTO12Bet { get; set; }

        public decimal PlaceTO12Bet { get; set; }

        public decimal Wpto12Bet { get; set; }

        public decimal TotalTO12Bet { get; set; }

        public decimal WinLoss12Bet { get; set; } // PTWLLicensee

        public decimal WinTO12BetMYR { get; set; }

        public decimal PlaceTO12BetMYR { get; set; }

        public decimal Wpto12BetMYR { get; set; }

        public decimal TotalTO12BetMYR { get; set; }

        public decimal WinLoss12BetMYR { get; set; }

        public decimal WinTO12BetEUR { get; set; }

        public decimal PlaceTO12BetEUR { get; set; }

        public decimal Wpto12BetEUR { get; set; }

        public decimal TotalTO12BetEUR { get; set; }

        public decimal WinLoss12BetEUR { get; set; }

        public decimal WinTO12BetUSD { get; set; }

        public decimal PlaceTO12BetUSD { get; set; }

        public decimal Wpto12BetUSD { get; set; }

        public decimal TotalTO12BetUSD { get; set; }

        public decimal WinLoss12BetUSD { get; set; }

        #endregion 12Bet Licensee
    }
}