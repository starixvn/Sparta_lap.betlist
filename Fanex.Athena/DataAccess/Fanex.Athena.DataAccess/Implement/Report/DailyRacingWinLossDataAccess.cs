// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------
namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using Base;
    using Data;
    using Exception;
    using Fanex.Common.Extensions;
    using Interface.Report;
    using Logging;
    using Models.DTO.Report;
    using Models.MetaModel.Report;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyRacingWinLossDataAccess : BaseDataAccess, IDailyRacingWinLossDataAccess
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<DailyRacingWinLoss> GetDataList(DailyRacingWinLossMetaModel metaModel)
        {
            var uspName = "Lrf_Rpt_WinlostRacing";

            var uspParams = new
            {
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.LicenseeType,
                metaModel.CurrencyId,
                WinlostDate = metaModel.Date,
            };

            try
            {
                using (var db = new ObjectDb(uspName))
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
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, uspParams);
            }
        }

        /// <summary>
        /// Set to list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
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
                var dailyRacingWinLoss = new DailyRacingWinLoss();

                // Common Info
                dailyRacingWinLoss.Currency = reader.ToInt(currency);
                dailyRacingWinLoss.LeagueId = reader.ToInt(leagueId);
                dailyRacingWinLoss.CurrencyName = reader.ToString(currencyName);
                dailyRacingWinLoss.RaceNo = reader.ToInt(raceColumn);

                // Customer Info
                dailyRacingWinLoss.WinTO = reader.ToDecimal(winToColumn);
                dailyRacingWinLoss.PlaceTO = reader.ToDecimal(placeToColumn);
                dailyRacingWinLoss.WPTO = reader.ToDecimal(winPlaceToColumn);
                dailyRacingWinLoss.TotalTO = reader.ToDecimal(totalToColumn);
                dailyRacingWinLoss.DividendWon = reader.ToDecimal(dividendWonColumn);
                dailyRacingWinLoss.CustomerCommission = reader.ToDecimal(customerCommColumn);
                dailyRacingWinLoss.CommIncome = reader.ToDecimal(commIncomeColumn);
                dailyRacingWinLoss.AmountDueToRacing = reader.ToDecimal(amtDueToRacingColumn);

                // Base MYR
                dailyRacingWinLoss.WinTOMYR = reader.ToDecimal("WinTOMemMYR");
                dailyRacingWinLoss.PlaceTOMYR = reader.ToDecimal("PlaceTOMemMYR");
                dailyRacingWinLoss.WPTOMYR = reader.ToDecimal("WinPlaceTOMemMYR");
                dailyRacingWinLoss.TotalTOMYR = reader.ToDecimal("TotalTOMemMYR");
                dailyRacingWinLoss.DividendWonMYR = reader.ToDecimal("DividendWonMemMYR");
                dailyRacingWinLoss.CustomerCommissionMYR = reader.ToDecimal("PlayerCommInComeLicenseeMYR");
                dailyRacingWinLoss.CommIncomeMYR = reader.ToDecimal("CommIncomeLicenseeMYR");
                dailyRacingWinLoss.AmountDueToRacingMYR = reader.ToDecimal("AmountDueToRBMYR");

                // Base EUR
                dailyRacingWinLoss.WinTOEUR = reader.ToDecimal("WinTOMemEUR");
                dailyRacingWinLoss.PlaceTOEUR = reader.ToDecimal("PlaceTOMemEUR");
                dailyRacingWinLoss.WPTOEUR = reader.ToDecimal("WinPlaceTOMemEUR");
                dailyRacingWinLoss.TotalTOEUR = reader.ToDecimal("TotalTOMemEUR");
                dailyRacingWinLoss.DividendWonEUR = reader.ToDecimal("DividendWonMemEUR");
                dailyRacingWinLoss.CustomerCommissionEUR = reader.ToDecimal("PlayerCommInComeLicenseeEUR");
                dailyRacingWinLoss.CommIncomeEUR = reader.ToDecimal("CommIncomeLicenseeEUR");
                dailyRacingWinLoss.AmountDueToRacingEUR = reader.ToDecimal("AmountDueToRBEUR");

                // Base USD, appear since we support base USD
                dailyRacingWinLoss.WinTOUSD = reader.ToDecimal("WinTOMemUSD");
                dailyRacingWinLoss.PlaceTOUSD = reader.ToDecimal("PlaceTOMemUSD");
                dailyRacingWinLoss.WPTOUSD = reader.ToDecimal("WinPlaceTOMemUSD");
                dailyRacingWinLoss.TotalTOUSD = reader.ToDecimal("TotalTOMemUSD");
                dailyRacingWinLoss.DividendWonUSD = reader.ToDecimal("DividendWonMemUSD");
                dailyRacingWinLoss.CustomerCommissionUSD = reader.ToDecimal("PlayerCommInComeLicenseeUSD");
                dailyRacingWinLoss.CommIncomeUSD = reader.ToDecimal("CommIncomeLicenseeUSD");
                dailyRacingWinLoss.AmountDueToRacingUSD = reader.ToDecimal("AmountDueToRBUSD");

                // Base GBP
                dailyRacingWinLoss.WinTOGBP = reader.ToDecimal("WinTOMemGBP");
                dailyRacingWinLoss.PlaceTOGBP = reader.ToDecimal("PlaceTOMemGBP");
                dailyRacingWinLoss.WPTOGBP = reader.ToDecimal("WinPlaceTOMemGBP");
                dailyRacingWinLoss.TotalTOGBP = reader.ToDecimal("TotalTOMemGBP");
                dailyRacingWinLoss.DividendWonGBP = reader.ToDecimal("DividendWonMemGBP");
                dailyRacingWinLoss.CustomerCommissionGBP = reader.ToDecimal("PlayerCommInComeLicenseeGBP");
                dailyRacingWinLoss.CommIncomeGBP = reader.ToDecimal("CommIncomeLicenseeGBP");
                dailyRacingWinLoss.AmountDueToRacingGBP = reader.ToDecimal("AmountDueToRBGBP");

                // Licensee Info
                dailyRacingWinLoss.WinTO12Bet = reader.ToDecimal(winTo12BetColumn);
                dailyRacingWinLoss.PlaceTO12Bet = reader.ToDecimal(placeTo12BetColumn);
                dailyRacingWinLoss.WPTO12Bet = reader.ToDecimal(winPlaceTo12BetColumn);
                dailyRacingWinLoss.TotalTO12Bet = reader.ToDecimal(totalTo12BetColumn);
                dailyRacingWinLoss.WinLoss12Bet = reader.ToDecimal(winLoss12BetColumn);

                // Base MYR
                dailyRacingWinLoss.WinTO12BetMYR = reader.ToDecimal("WinTOLicenseeMYR");
                dailyRacingWinLoss.PlaceTO12BetMYR = reader.ToDecimal("PlaceTOLicenseeMYR");
                dailyRacingWinLoss.WPTO12BetMYR = reader.ToDecimal("WinPlaceTOLicenseeMYR");
                dailyRacingWinLoss.TotalTO12BetMYR = reader.ToDecimal("TotalTOLicenseeMYR");
                dailyRacingWinLoss.WinLoss12BetMYR = reader.ToDecimal("PTWLLicenseeMYR");

                // Base EUR
                dailyRacingWinLoss.WinTO12BetEUR = reader.ToDecimal("WinTOLicenseeEUR");
                dailyRacingWinLoss.PlaceTO12BetEUR = reader.ToDecimal("PlaceTOLicenseeEUR");
                dailyRacingWinLoss.WPTO12BetEUR = reader.ToDecimal("WinPlaceTOLicenseeEUR");
                dailyRacingWinLoss.TotalTO12BetEUR = reader.ToDecimal("TotalTOLicenseeEUR");
                dailyRacingWinLoss.WinLoss12BetEUR = reader.ToDecimal("PTWLLicenseeEUR");

                // Base USD, appear since we support base USD
                dailyRacingWinLoss.WinTO12BetUSD = reader.ToDecimal("WinTOLicenseeUSD");
                dailyRacingWinLoss.PlaceTO12BetUSD = reader.ToDecimal("PlaceTOLicenseeUSD");
                dailyRacingWinLoss.WPTO12BetUSD = reader.ToDecimal("WinPlaceTOLicenseeUSD");
                dailyRacingWinLoss.TotalTO12BetUSD = reader.ToDecimal("TotalTOLicenseeUSD");
                dailyRacingWinLoss.WinLoss12BetUSD = reader.ToDecimal("PTWLLicenseeUSD");

                // Base GBP
                dailyRacingWinLoss.WinTO12BetGBP = reader.ToDecimal("WinTOLicenseeGBP");
                dailyRacingWinLoss.PlaceTO12BetGBP = reader.ToDecimal("PlaceTOLicenseeGBP");
                dailyRacingWinLoss.WPTO12BetGBP = reader.ToDecimal("WinPlaceTOLicenseeGBP");
                dailyRacingWinLoss.TotalTO12BetGBP = reader.ToDecimal("TotalTOLicenseeGBP");
                dailyRacingWinLoss.WinLoss12BetGBP = reader.ToDecimal("PTWLLicenseeGBP");

                dataList.Add(dailyRacingWinLoss);
            }

            return dataList;
        }
    }
}