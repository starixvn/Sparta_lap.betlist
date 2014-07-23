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
            catch (SqlException sqlException)
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
                dailyRacingWinLoss.Wpto = reader.ToDecimal(winPlaceToColumn);
                dailyRacingWinLoss.TotalTO = reader.ToDecimal(totalToColumn);
                dailyRacingWinLoss.DividendWon = reader.ToDecimal(dividendWonColumn);
                dailyRacingWinLoss.CustomerCommission = reader.ToDecimal(customerCommColumn);
                dailyRacingWinLoss.CommIncome = reader.ToDecimal(commIncomeColumn);
                dailyRacingWinLoss.AmountDueToRacing = reader.ToDecimal(amtDueToRacingColumn);

                // Base MYR
                dailyRacingWinLoss.WinToMYR = reader.ToDecimal("WinTOMemMYR");
                dailyRacingWinLoss.PlaceToMYR = reader.ToDecimal("PlaceTOMemMYR");
                dailyRacingWinLoss.WptoMYR = reader.ToDecimal("WinPlaceTOMemMYR");
                dailyRacingWinLoss.TotalToMYR = reader.ToDecimal("TotalTOMemMYR");
                dailyRacingWinLoss.DividendWonMYR = reader.ToDecimal("DividendWonMemMYR");
                dailyRacingWinLoss.CustomerCommissionMYR = reader.ToDecimal("PlayerCommInComeLicenseeMYR");
                dailyRacingWinLoss.CommIncomeMYR = reader.ToDecimal("CommIncomeLicenseeMYR");
                dailyRacingWinLoss.AmountDueToRacingMYR = reader.ToDecimal("AmountDueToRBMYR");

                // Base EUR
                dailyRacingWinLoss.WinToEur = reader.ToDecimal("WinTOMemEUR");
                dailyRacingWinLoss.PlaceToEur = reader.ToDecimal("PlaceTOMemEUR");
                dailyRacingWinLoss.WptoEUR = reader.ToDecimal("WinPlaceTOMemEUR");
                dailyRacingWinLoss.TotalToEur = reader.ToDecimal("TotalTOMemEUR");
                dailyRacingWinLoss.DividendWonEUR = reader.ToDecimal("DividendWonMemEUR");
                dailyRacingWinLoss.CustomerCommissionEUR = reader.ToDecimal("PlayerCommInComeLicenseeEUR");
                dailyRacingWinLoss.CommIncomeEUR = reader.ToDecimal("CommIncomeLicenseeEUR");
                dailyRacingWinLoss.AmountDueToRacingEUR = reader.ToDecimal("AmountDueToRBEUR");

                // Base USD, appear since we support base USD
                dailyRacingWinLoss.WinToUSD = reader.ToDecimal("WinTOMemUSD");
                dailyRacingWinLoss.PlaceToUSD = reader.ToDecimal("PlaceTOMemUSD");
                dailyRacingWinLoss.WptoUSD = reader.ToDecimal("WinPlaceTOMemUSD");
                dailyRacingWinLoss.TotalToUSD = reader.ToDecimal("TotalTOMemUSD");
                dailyRacingWinLoss.DividendWonUSD = reader.ToDecimal("DividendWonMemUSD");
                dailyRacingWinLoss.CustomerCommissionUSD = reader.ToDecimal("PlayerCommInComeLicenseeUSD");
                dailyRacingWinLoss.CommIncomeUSD = reader.ToDecimal("CommIncomeLicenseeUSD");
                dailyRacingWinLoss.AmountDueToRacingUSD = reader.ToDecimal("AmountDueToRBUSD");

                // Licensee Info
                dailyRacingWinLoss.WinTO12Bet = reader.ToDecimal(winTo12BetColumn);
                dailyRacingWinLoss.PlaceTO12Bet = reader.ToDecimal(placeTo12BetColumn);
                dailyRacingWinLoss.Wpto12Bet = reader.ToDecimal(winPlaceTo12BetColumn);
                dailyRacingWinLoss.TotalTO12Bet = reader.ToDecimal(totalTo12BetColumn);
                dailyRacingWinLoss.WinLoss12Bet = reader.ToDecimal(winLoss12BetColumn);

                // Base MYR
                dailyRacingWinLoss.WinTO12BetMYR = reader.ToDecimal("WinTOLicenseeMYR");
                dailyRacingWinLoss.PlaceTO12BetMYR = reader.ToDecimal("PlaceTOLicenseeMYR");
                dailyRacingWinLoss.Wpto12BetMYR = reader.ToDecimal("WinPlaceTOLicenseeMYR");
                dailyRacingWinLoss.TotalTO12BetMYR = reader.ToDecimal("TotalTOLicenseeMYR");
                dailyRacingWinLoss.WinLoss12BetMYR = reader.ToDecimal("PTWLLicenseeMYR");

                // Base EUR
                dailyRacingWinLoss.WinTO12BetEUR = reader.ToDecimal("WinTOLicenseeEUR");
                dailyRacingWinLoss.PlaceTO12BetEUR = reader.ToDecimal("PlaceTOLicenseeEUR");
                dailyRacingWinLoss.Wpto12BetEUR = reader.ToDecimal("WinPlaceTOLicenseeEUR");
                dailyRacingWinLoss.TotalTO12BetEUR = reader.ToDecimal("TotalTOLicenseeEUR");
                dailyRacingWinLoss.WinLoss12BetEUR = reader.ToDecimal("PTWLLicenseeEUR");

                // Base USD, appear since we support base USD
                dailyRacingWinLoss.WinTO12BetUSD = reader.ToDecimal("WinTOLicenseeUSD");
                dailyRacingWinLoss.PlaceTO12BetUSD = reader.ToDecimal("PlaceTOLicenseeUSD");
                dailyRacingWinLoss.Wpto12BetUSD = reader.ToDecimal("WinPlaceTOLicenseeUSD");
                dailyRacingWinLoss.TotalTO12BetUSD = reader.ToDecimal("TotalTOLicenseeUSD");
                dailyRacingWinLoss.WinLoss12BetUSD = reader.ToDecimal("PTWLLicenseeUSD");

                dataList.Add(dailyRacingWinLoss);
            }

            return dataList;
        }
    }
}