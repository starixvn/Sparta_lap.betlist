// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
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

        public decimal WPTO { get; set; }

        public decimal TotalTO { get; set; }

        public decimal DividendWon { get; set; }

        public decimal CustomerCommission { get; set; }

        public decimal CommIncome { get; set; }

        public decimal AmountDueToRacing { get; set; }

        #endregion depend on base currency

        #region not depend on base currency

        public decimal WinTOMYR { get; set; }

        public decimal PlaceTOMYR { get; set; }

        public decimal WPTOMYR { get; set; }

        public decimal TotalTOMYR { get; set; }

        public decimal DividendWonMYR { get; set; }

        public decimal CustomerCommissionMYR { get; set; }

        public decimal CommIncomeMYR { get; set; }

        public decimal AmountDueToRacingMYR { get; set; }

        // EUR.
        public decimal WinTOEUR { get; set; }

        public decimal PlaceTOEUR { get; set; }

        public decimal WPTOEUR { get; set; }

        public decimal TotalTOEUR { get; set; }

        public decimal DividendWonEUR { get; set; }

        public decimal CustomerCommissionEUR { get; set; }

        public decimal CommIncomeEUR { get; set; }

        public decimal AmountDueToRacingEUR { get; set; }

        // USD.
        public decimal WinTOUSD { get; set; }

        public decimal PlaceTOUSD { get; set; }

        public decimal WPTOUSD { get; set; }

        public decimal TotalTOUSD { get; set; }

        public decimal DividendWonUSD { get; set; }

        public decimal CustomerCommissionUSD { get; set; }

        public decimal CommIncomeUSD { get; set; }

        public decimal AmountDueToRacingUSD { get; set; }

        // GBP.
        public decimal WinTOGBP { get; set; }

        public decimal PlaceTOGBP { get; set; }

        public decimal WPTOGBP { get; set; }

        public decimal TotalTOGBP { get; set; }

        public decimal DividendWonGBP { get; set; }

        public decimal CustomerCommissionGBP { get; set; }

        public decimal CommIncomeGBP { get; set; }

        public decimal AmountDueToRacingGBP { get; set; }

        #endregion not depend on base currency

        #endregion Customer Info

        #region 12Bet Licensee

        public decimal WinTO12Bet { get; set; }

        public decimal PlaceTO12Bet { get; set; }

        public decimal WPTO12Bet { get; set; }

        public decimal TotalTO12Bet { get; set; }

        public decimal WinLoss12Bet { get; set; } // PTWLLicensee

        public decimal WinTO12BetMYR { get; set; }

        public decimal PlaceTO12BetMYR { get; set; }

        public decimal WPTO12BetMYR { get; set; }

        public decimal TotalTO12BetMYR { get; set; }

        public decimal WinLoss12BetMYR { get; set; }

        public decimal WinTO12BetEUR { get; set; }

        public decimal PlaceTO12BetEUR { get; set; }

        public decimal WPTO12BetEUR { get; set; }

        public decimal TotalTO12BetEUR { get; set; }

        public decimal WinLoss12BetEUR { get; set; }

        public decimal WinTO12BetUSD { get; set; }

        public decimal PlaceTO12BetUSD { get; set; }

        public decimal WPTO12BetUSD { get; set; }

        public decimal TotalTO12BetUSD { get; set; }

        public decimal WinLoss12BetUSD { get; set; }

        public decimal WinTO12BetGBP { get; set; }

        public decimal PlaceTO12BetGBP { get; set; }

        public decimal WPTO12BetGBP { get; set; }

        public decimal TotalTO12BetGBP { get; set; }

        public decimal WinLoss12BetGBP { get; set; }

        #endregion 12Bet Licensee
    }
}