// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

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

        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private static DailyRacingWinLoss GetSumDailyRacingWinLoss(IList<DailyRacingWinLoss> rawData, string currencyName)
        {
            var result = new DailyRacingWinLoss
            {
                CurrencyName = currencyName
            };

            foreach (var item in rawData)
            {
                result.WinTO += item.WinTO;
                result.PlaceTO += item.PlaceTO;
                result.WPTO += item.WPTO;
                result.TotalTO += item.TotalTO;
                result.DividendWon += item.DividendWon;
                result.CustomerCommission += item.CustomerCommission;
                result.WinTO12Bet += item.WinTO12Bet;
                result.PlaceTO12Bet += item.PlaceTO12Bet;
                result.WPTO12Bet += item.WPTO12Bet;
                result.TotalTO12Bet += item.TotalTO12Bet;
                result.WinLoss12Bet += item.WinLoss12Bet;
                result.CommIncome += item.CommIncome;
                result.AmountDueToRacing += item.AmountDueToRacing;

                // MYR
                result.WinTOMYR += item.WinTOMYR;
                result.PlaceTOMYR += item.PlaceTOMYR;
                result.WPTOMYR += item.WPTOMYR;
                result.TotalTOMYR += item.TotalTOMYR;
                result.DividendWonMYR += item.DividendWonMYR;
                result.CustomerCommissionMYR += item.CustomerCommissionMYR;
                result.WinTO12BetMYR += item.WinTO12BetMYR;
                result.PlaceTO12BetMYR += item.PlaceTO12BetMYR;
                result.WPTO12BetMYR += item.WPTO12BetMYR;
                result.TotalTO12BetMYR += item.TotalTO12BetMYR;
                result.WinLoss12BetMYR += item.WinLoss12BetMYR;
                result.CommIncomeMYR += item.CommIncomeMYR;
                result.AmountDueToRacingMYR += item.AmountDueToRacingMYR;

                // EUR
                result.WinTOEUR += item.WinTOEUR;
                result.PlaceTOEUR += item.PlaceTOEUR;
                result.WPTOEUR += item.WPTOEUR;
                result.TotalTOEUR += item.TotalTOEUR;
                result.DividendWonEUR += item.DividendWonEUR;
                result.CustomerCommissionEUR += item.CustomerCommissionEUR;
                result.WinTO12BetEUR += item.WinTO12BetEUR;
                result.PlaceTO12BetEUR += item.PlaceTO12BetEUR;
                result.WPTO12BetEUR += item.WPTO12BetEUR;
                result.TotalTO12BetEUR += item.TotalTO12BetEUR;
                result.WinLoss12BetEUR += item.WinLoss12BetEUR;
                result.CommIncomeEUR += item.CommIncomeEUR;
                result.AmountDueToRacingEUR += item.AmountDueToRacingEUR;

                // USD
                result.WinTOUSD += item.WinTOUSD;
                result.PlaceTOUSD += item.PlaceTOUSD;
                result.WPTOUSD += item.WPTOUSD;
                result.TotalTOUSD += item.TotalTOUSD;
                result.DividendWonUSD += item.DividendWonUSD;
                result.CustomerCommissionUSD += item.CustomerCommissionUSD;
                result.WinTO12BetUSD += item.WinTO12BetUSD;
                result.PlaceTO12BetUSD += item.PlaceTO12BetUSD;
                result.WPTO12BetUSD += item.WPTO12BetUSD;
                result.TotalTO12BetUSD += item.TotalTO12BetUSD;
                result.WinLoss12BetUSD += item.WinLoss12BetUSD;
                result.CommIncomeUSD += item.CommIncomeUSD;
                result.AmountDueToRacingUSD += item.AmountDueToRacingUSD;

                // GBP
                result.WinTOGBP += item.WinTOGBP;
                result.PlaceTOGBP += item.PlaceTOGBP;
                result.WPTOGBP += item.WPTOGBP;
                result.TotalTOGBP += item.TotalTOGBP;
                result.DividendWonGBP += item.DividendWonGBP;
                result.CustomerCommissionGBP += item.CustomerCommissionGBP;
                result.WinTO12BetGBP += item.WinTO12BetGBP;
                result.PlaceTO12BetGBP += item.PlaceTO12BetGBP;
                result.WPTO12BetGBP += item.WPTO12BetGBP;
                result.TotalTO12BetGBP += item.TotalTO12BetGBP;
                result.WinLoss12BetGBP += item.WinLoss12BetGBP;
                result.CommIncomeGBP += item.CommIncomeGBP;
                result.AmountDueToRacingGBP += item.AmountDueToRacingGBP;
            }

            return result;
        }

        #endregion Private
    }
}