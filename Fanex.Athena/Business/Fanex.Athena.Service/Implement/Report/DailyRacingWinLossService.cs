// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class DailyRacingWinLossService : IDailyRacingWinLossService
    {
        #region Fields

        private readonly IDailyRacingWinLossDataAccess _dailyRacingWinLossDataAccess;

        #endregion Fields

        #region Constructor

        public DailyRacingWinLossService()
        {
            _dailyRacingWinLossDataAccess = new DailyRacingWinLossDataAccess();
        }

        #endregion Constructor

        #region Public

        public IList<DailyRacingWinLoss> GetDataList(DailyRacingWinLossMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _dailyRacingWinLossDataAccess.GetDataList(metaModel)
                       : new List<DailyRacingWinLoss>();
        }

        /// <summary>
        /// Get daily racing win-loss data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>Daily racing win-loss data.</returns>
        public DailyRacingWinLossData GetDailyRacingWinLossData(DailyRacingWinLossMetaModel metaModel)
        {
            var rawData = GetDataList(metaModel);

            var contentData = rawData.GroupBy(r => string.Format("{0}-{1}", r.LeagueId, r.Currency), r => r)
                                     .ToDictionary(g => g.Key, g => g.ToList());

            var contentTotalData = rawData.GroupBy(r => string.Format("{0}-{1}", r.LeagueId, r.Currency), r => r)
                                          .ToDictionary(g => g.Key, g => GetSumDailyRacingWinLoss(g.ToList(), g.Select(a => a.CurrencyName).FirstOrDefault()));

            var currencyTotalData = rawData.GroupBy(r => r.CurrencyName, r => r)
                                           .ToDictionary(g => g.Key, g => GetSumDailyRacingWinLoss(g.ToList(), g.Key));

            var grandTotalData = GetSumDailyRacingWinLoss(rawData, string.Empty);

            return new DailyRacingWinLossData
                {
                    ContentData = contentData,
                    ContentTotalData = contentTotalData,
                    CurrencyTotalData = currencyTotalData,
                    GrandTotalData = grandTotalData
                };
        }

        #endregion Public

        #region Private

        /// <summary>
        /// Get summary daily racing win-loss.
        /// </summary>
        /// <param name="rawData">The raw data .</param>
        /// <param name="currencyName">The currency name.</param>
        /// <returns>The daily racing win-loss.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private static DailyRacingWinLoss GetSumDailyRacingWinLoss(IList<DailyRacingWinLoss> rawData, string currencyName)
        {
            return new DailyRacingWinLoss
                {
                    CurrencyName = currencyName,
                    WinTO = rawData.Sum(a => a.WinTO),
                    PlaceTO = rawData.Sum(a => a.PlaceTO),
                    WPTO = rawData.Sum(a => a.WPTO),
                    TotalTO = rawData.Sum(a => a.TotalTO),
                    DividendWon = rawData.Sum(a => a.DividendWon),
                    CustomerCommission = rawData.Sum(a => a.CustomerCommission),
                    WinTO12Bet = rawData.Sum(a => a.WinTO12Bet),
                    PlaceTO12Bet = rawData.Sum(a => a.PlaceTO12Bet),
                    WPTO12Bet = rawData.Sum(a => a.WPTO12Bet),
                    TotalTO12Bet = rawData.Sum(a => a.TotalTO12Bet),
                    WinLoss12Bet = rawData.Sum(a => a.WinLoss12Bet),
                    CommIncome = rawData.Sum(a => a.CommIncome),
                    AmountDueToRacing = rawData.Sum(a => a.AmountDueToRacing),

                    // MYR
                    WinTOMYR = rawData.Sum(a => a.WinTOMYR),
                    PlaceTOMYR = rawData.Sum(a => a.PlaceTOMYR),
                    WPTOMYR = rawData.Sum(a => a.WPTOMYR),
                    TotalTOMYR = rawData.Sum(a => a.TotalTOMYR),
                    DividendWonMYR = rawData.Sum(a => a.DividendWonMYR),
                    CustomerCommissionMYR = rawData.Sum(a => a.CustomerCommissionMYR),
                    WinTO12BetMYR = rawData.Sum(a => a.WinTO12BetMYR),
                    PlaceTO12BetMYR = rawData.Sum(a => a.PlaceTO12BetMYR),
                    WPTO12BetMYR = rawData.Sum(a => a.WPTO12BetMYR),
                    TotalTO12BetMYR = rawData.Sum(a => a.TotalTO12BetMYR),
                    WinLoss12BetMYR = rawData.Sum(a => a.WinLoss12BetMYR),
                    CommIncomeMYR = rawData.Sum(a => a.CommIncomeMYR),
                    AmountDueToRacingMYR = rawData.Sum(a => a.AmountDueToRacingMYR),

                    // EUR
                    WinTOEUR = rawData.Sum(a => a.WinTOEUR),
                    PlaceTOEUR = rawData.Sum(a => a.PlaceTOEUR),
                    WPTOEUR = rawData.Sum(a => a.WPTOEUR),
                    TotalTOEUR = rawData.Sum(a => a.TotalTOEUR),
                    DividendWonEUR = rawData.Sum(a => a.DividendWonEUR),
                    CustomerCommissionEUR = rawData.Sum(a => a.CustomerCommissionEUR),
                    WinTO12BetEUR = rawData.Sum(a => a.WinTO12BetEUR),
                    PlaceTO12BetEUR = rawData.Sum(a => a.PlaceTO12BetEUR),
                    WPTO12BetEUR = rawData.Sum(a => a.WPTO12BetEUR),
                    TotalTO12BetEUR = rawData.Sum(a => a.TotalTO12BetEUR),
                    WinLoss12BetEUR = rawData.Sum(a => a.WinLoss12BetEUR),
                    CommIncomeEUR = rawData.Sum(a => a.CommIncomeEUR),
                    AmountDueToRacingEUR = rawData.Sum(a => a.AmountDueToRacingEUR),

                    // USD
                    WinTOUSD = rawData.Sum(a => a.WinTOUSD),
                    PlaceTOUSD = rawData.Sum(a => a.PlaceTOUSD),
                    WPTOUSD = rawData.Sum(a => a.WPTOUSD),
                    TotalTOUSD = rawData.Sum(a => a.TotalTOUSD),
                    DividendWonUSD = rawData.Sum(a => a.DividendWonUSD),
                    CustomerCommissionUSD = rawData.Sum(a => a.CustomerCommissionUSD),
                    WinTO12BetUSD = rawData.Sum(a => a.WinTO12BetUSD),
                    PlaceTO12BetUSD = rawData.Sum(a => a.PlaceTO12BetUSD),
                    WPTO12BetUSD = rawData.Sum(a => a.WPTO12BetUSD),
                    TotalTO12BetUSD = rawData.Sum(a => a.TotalTO12BetUSD),
                    WinLoss12BetUSD = rawData.Sum(a => a.WinLoss12BetUSD),
                    CommIncomeUSD = rawData.Sum(a => a.CommIncomeUSD),
                    AmountDueToRacingUSD = rawData.Sum(a => a.AmountDueToRacingUSD)
                };
        }

        #endregion Private
    }
}