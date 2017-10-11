// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using Core.Exception;
    using Data;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Logging;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class DailyRacingWinLossDataAccess : BaseDataAccess, IDailyRacingWinLossDataAccess
    {
        public IList<DailyRacingWinLoss> GetDataList(DailyRacingWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinlostRacing";

            var uspParams = new
            {
                AgentList = metaModel.AgentListXml,
                WinlostDate = metaModel.Date,
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var baseCurrency = metaModel.BaseCurrency;
                        return this.SetToList(reader, metaModel.LicenseeId, baseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<DailyRacingWinLoss> SetToList(IDataReader reader, int licenseeId, string baseCurrency)
        {
            var dataList = new List<DailyRacingWinLoss>();
            var leagueId = "LeagueId";
            var currencyName = "CurrencyName";
            var currency = "Currency";
            var raceColumn = "GroupMatch";
            var winToColumn = "WinTOMem" + baseCurrency;
            var placeToColumn = "PlaceTOMem" + baseCurrency;
            var winPlaceToColumn = "WinPlaceTOMem" + baseCurrency;
            var totalToColumn = "TotalTOMem" + baseCurrency;
            var dividendWonColumn = "DividendWonMem" + baseCurrency;
            var customerCommColumn = "PlayerCommInComeLicensee" + baseCurrency;
            var commIncomeColumn = "CommIncomeLicensee" + baseCurrency;
            var amtDueToRacingColumn = "AmountDueToRB" + baseCurrency;

            var winTo12BetColumn = "WinTOLicensee" + baseCurrency;
            var placeTo12BetColumn = "PlaceTOLicensee" + baseCurrency;
            var winPlaceTo12BetColumn = "WinPlaceTOLicensee" + baseCurrency;
            var totalTo12BetColumn = "TotalTOLicensee" + baseCurrency;
            var winLoss12BetColumn = "PTWLLicensee" + baseCurrency;

            while (reader.Read())
            {
                var item = new DailyRacingWinLoss
                {
                    // Common Info
                    Currency = reader.ToInt(currency),
                    LeagueId = reader.ToInt(leagueId),
                    CurrencyName = reader.ToString(currencyName),
                    RaceNo = reader.ToInt(raceColumn),

                    // Customer Info
                    WinTO = reader.ToDecimal(winToColumn),
                    PlaceTO = reader.ToDecimal(placeToColumn),
                    WPTO = reader.ToDecimal(winPlaceToColumn),
                    TotalTO = reader.ToDecimal(totalToColumn),
                    DividendWon = reader.ToDecimal(dividendWonColumn),
                    CustomerCommission = reader.ToDecimal(customerCommColumn),
                    CommIncome = reader.ToDecimal(commIncomeColumn),
                    AmountDueToRacing = reader.ToDecimal(amtDueToRacingColumn),

                    // Base MYR
                    WinTOMYR = reader.ToDecimal("WinTOMemMYR"),
                    PlaceTOMYR = reader.ToDecimal("PlaceTOMemMYR"),
                    WPTOMYR = reader.ToDecimal("WinPlaceTOMemMYR"),
                    TotalTOMYR = reader.ToDecimal("TotalTOMemMYR"),
                    DividendWonMYR = reader.ToDecimal("DividendWonMemMYR"),
                    CustomerCommissionMYR = reader.ToDecimal("PlayerCommInComeLicenseeMYR"),
                    CommIncomeMYR = reader.ToDecimal("CommIncomeLicenseeMYR"),
                    AmountDueToRacingMYR = reader.ToDecimal("AmountDueToRBMYR"),

                    // Base EUR
                    WinTOEUR = reader.ToDecimal("WinTOMemEUR"),
                    PlaceTOEUR = reader.ToDecimal("PlaceTOMemEUR"),
                    WPTOEUR = reader.ToDecimal("WinPlaceTOMemEUR"),
                    TotalTOEUR = reader.ToDecimal("TotalTOMemEUR"),
                    DividendWonEUR = reader.ToDecimal("DividendWonMemEUR"),
                    CustomerCommissionEUR = reader.ToDecimal("PlayerCommInComeLicenseeEUR"),
                    CommIncomeEUR = reader.ToDecimal("CommIncomeLicenseeEUR"),
                    AmountDueToRacingEUR = reader.ToDecimal("AmountDueToRBEUR"),

                    // Base USD, appear since we support base USD
                    WinTOUSD = reader.ToDecimal("WinTOMemUSD"),
                    PlaceTOUSD = reader.ToDecimal("PlaceTOMemUSD"),
                    WPTOUSD = reader.ToDecimal("WinPlaceTOMemUSD"),
                    TotalTOUSD = reader.ToDecimal("TotalTOMemUSD"),
                    DividendWonUSD = reader.ToDecimal("DividendWonMemUSD"),
                    CustomerCommissionUSD = reader.ToDecimal("PlayerCommInComeLicenseeUSD"),
                    CommIncomeUSD = reader.ToDecimal("CommIncomeLicenseeUSD"),
                    AmountDueToRacingUSD = reader.ToDecimal("AmountDueToRBUSD"),

                    // Base GBP
                    WinTOGBP = reader.ToDecimal("WinTOMemGBP"),
                    PlaceTOGBP = reader.ToDecimal("PlaceTOMemGBP"),
                    WPTOGBP = reader.ToDecimal("WinPlaceTOMemGBP"),
                    TotalTOGBP = reader.ToDecimal("TotalTOMemGBP"),
                    DividendWonGBP = reader.ToDecimal("DividendWonMemGBP"),
                    CustomerCommissionGBP = reader.ToDecimal("PlayerCommInComeLicenseeGBP"),
                    CommIncomeGBP = reader.ToDecimal("CommIncomeLicenseeGBP"),
                    AmountDueToRacingGBP = reader.ToDecimal("AmountDueToRBGBP"),

                    // Licensee Info
                    WinTO12Bet = reader.ToDecimal(winTo12BetColumn),
                    PlaceTO12Bet = reader.ToDecimal(placeTo12BetColumn),
                    WPTO12Bet = reader.ToDecimal(winPlaceTo12BetColumn),
                    TotalTO12Bet = reader.ToDecimal(totalTo12BetColumn),
                    WinLoss12Bet = reader.ToDecimal(winLoss12BetColumn),

                    // Base MYR
                    WinTO12BetMYR = reader.ToDecimal("WinTOLicenseeMYR"),
                    PlaceTO12BetMYR = reader.ToDecimal("PlaceTOLicenseeMYR"),
                    WPTO12BetMYR = reader.ToDecimal("WinPlaceTOLicenseeMYR"),
                    TotalTO12BetMYR = reader.ToDecimal("TotalTOLicenseeMYR"),
                    WinLoss12BetMYR = reader.ToDecimal("PTWLLicenseeMYR"),

                    // Base EUR
                    WinTO12BetEUR = reader.ToDecimal("WinTOLicenseeEUR"),
                    PlaceTO12BetEUR = reader.ToDecimal("PlaceTOLicenseeEUR"),
                    WPTO12BetEUR = reader.ToDecimal("WinPlaceTOLicenseeEUR"),
                    TotalTO12BetEUR = reader.ToDecimal("TotalTOLicenseeEUR"),
                    WinLoss12BetEUR = reader.ToDecimal("PTWLLicenseeEUR"),

                    // Base USD, appear since we support base USD
                    WinTO12BetUSD = reader.ToDecimal("WinTOLicenseeUSD"),
                    PlaceTO12BetUSD = reader.ToDecimal("PlaceTOLicenseeUSD"),
                    WPTO12BetUSD = reader.ToDecimal("WinPlaceTOLicenseeUSD"),
                    TotalTO12BetUSD = reader.ToDecimal("TotalTOLicenseeUSD"),
                    WinLoss12BetUSD = reader.ToDecimal("PTWLLicenseeUSD"),

                    // Base GBP
                    WinTO12BetGBP = reader.ToDecimal("WinTOLicenseeGBP"),
                    PlaceTO12BetGBP = reader.ToDecimal("PlaceTOLicenseeGBP"),
                    WPTO12BetGBP = reader.ToDecimal("WinPlaceTOLicenseeGBP"),
                    TotalTO12BetGBP = reader.ToDecimal("TotalTOLicenseeGBP"),
                    WinLoss12BetGBP = reader.ToDecimal("PTWLLicenseeGBP")
                };

                dataList.Add(item);
            }

            return dataList;
        }
    }
}