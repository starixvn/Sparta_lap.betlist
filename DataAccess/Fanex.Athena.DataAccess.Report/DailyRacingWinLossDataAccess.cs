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
            var uspName = "Lrf_Rpt_WinlostRacing";

            var uspParams = new
            {
                AgentList = metaModel.AgentListXml,
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
                var item = new DailyRacingWinLoss();

                // Common Info
                item.Currency = reader.ToInt(currency);
                item.LeagueId = reader.ToInt(leagueId);
                item.CurrencyName = reader.ToString(currencyName);
                item.RaceNo = reader.ToInt(raceColumn);

                // Customer Info
                item.WinTO = reader.ToDecimal(winToColumn);
                item.PlaceTO = reader.ToDecimal(placeToColumn);
                item.WPTO = reader.ToDecimal(winPlaceToColumn);
                item.TotalTO = reader.ToDecimal(totalToColumn);
                item.DividendWon = reader.ToDecimal(dividendWonColumn);
                item.CustomerCommission = reader.ToDecimal(customerCommColumn);
                item.CommIncome = reader.ToDecimal(commIncomeColumn);
                item.AmountDueToRacing = reader.ToDecimal(amtDueToRacingColumn);

                // Base MYR
                item.WinTOMYR = reader.ToDecimal("WinTOMemMYR");
                item.PlaceTOMYR = reader.ToDecimal("PlaceTOMemMYR");
                item.WPTOMYR = reader.ToDecimal("WinPlaceTOMemMYR");
                item.TotalTOMYR = reader.ToDecimal("TotalTOMemMYR");
                item.DividendWonMYR = reader.ToDecimal("DividendWonMemMYR");
                item.CustomerCommissionMYR = reader.ToDecimal("PlayerCommInComeLicenseeMYR");
                item.CommIncomeMYR = reader.ToDecimal("CommIncomeLicenseeMYR");
                item.AmountDueToRacingMYR = reader.ToDecimal("AmountDueToRBMYR");

                // Base EUR
                item.WinTOEUR = reader.ToDecimal("WinTOMemEUR");
                item.PlaceTOEUR = reader.ToDecimal("PlaceTOMemEUR");
                item.WPTOEUR = reader.ToDecimal("WinPlaceTOMemEUR");
                item.TotalTOEUR = reader.ToDecimal("TotalTOMemEUR");
                item.DividendWonEUR = reader.ToDecimal("DividendWonMemEUR");
                item.CustomerCommissionEUR = reader.ToDecimal("PlayerCommInComeLicenseeEUR");
                item.CommIncomeEUR = reader.ToDecimal("CommIncomeLicenseeEUR");
                item.AmountDueToRacingEUR = reader.ToDecimal("AmountDueToRBEUR");

                // Base USD, appear since we support base USD
                item.WinTOUSD = reader.ToDecimal("WinTOMemUSD");
                item.PlaceTOUSD = reader.ToDecimal("PlaceTOMemUSD");
                item.WPTOUSD = reader.ToDecimal("WinPlaceTOMemUSD");
                item.TotalTOUSD = reader.ToDecimal("TotalTOMemUSD");
                item.DividendWonUSD = reader.ToDecimal("DividendWonMemUSD");
                item.CustomerCommissionUSD = reader.ToDecimal("PlayerCommInComeLicenseeUSD");
                item.CommIncomeUSD = reader.ToDecimal("CommIncomeLicenseeUSD");
                item.AmountDueToRacingUSD = reader.ToDecimal("AmountDueToRBUSD");

                // Base GBP
                item.WinTOGBP = reader.ToDecimal("WinTOMemGBP");
                item.PlaceTOGBP = reader.ToDecimal("PlaceTOMemGBP");
                item.WPTOGBP = reader.ToDecimal("WinPlaceTOMemGBP");
                item.TotalTOGBP = reader.ToDecimal("TotalTOMemGBP");
                item.DividendWonGBP = reader.ToDecimal("DividendWonMemGBP");
                item.CustomerCommissionGBP = reader.ToDecimal("PlayerCommInComeLicenseeGBP");
                item.CommIncomeGBP = reader.ToDecimal("CommIncomeLicenseeGBP");
                item.AmountDueToRacingGBP = reader.ToDecimal("AmountDueToRBGBP");

                // Licensee Info
                item.WinTO12Bet = reader.ToDecimal(winTo12BetColumn);
                item.PlaceTO12Bet = reader.ToDecimal(placeTo12BetColumn);
                item.WPTO12Bet = reader.ToDecimal(winPlaceTo12BetColumn);
                item.TotalTO12Bet = reader.ToDecimal(totalTo12BetColumn);
                item.WinLoss12Bet = reader.ToDecimal(winLoss12BetColumn);

                // Base MYR
                item.WinTO12BetMYR = reader.ToDecimal("WinTOLicenseeMYR");
                item.PlaceTO12BetMYR = reader.ToDecimal("PlaceTOLicenseeMYR");
                item.WPTO12BetMYR = reader.ToDecimal("WinPlaceTOLicenseeMYR");
                item.TotalTO12BetMYR = reader.ToDecimal("TotalTOLicenseeMYR");
                item.WinLoss12BetMYR = reader.ToDecimal("PTWLLicenseeMYR");

                // Base EUR
                item.WinTO12BetEUR = reader.ToDecimal("WinTOLicenseeEUR");
                item.PlaceTO12BetEUR = reader.ToDecimal("PlaceTOLicenseeEUR");
                item.WPTO12BetEUR = reader.ToDecimal("WinPlaceTOLicenseeEUR");
                item.TotalTO12BetEUR = reader.ToDecimal("TotalTOLicenseeEUR");
                item.WinLoss12BetEUR = reader.ToDecimal("PTWLLicenseeEUR");

                // Base USD, appear since we support base USD
                item.WinTO12BetUSD = reader.ToDecimal("WinTOLicenseeUSD");
                item.PlaceTO12BetUSD = reader.ToDecimal("PlaceTOLicenseeUSD");
                item.WPTO12BetUSD = reader.ToDecimal("WinPlaceTOLicenseeUSD");
                item.TotalTO12BetUSD = reader.ToDecimal("TotalTOLicenseeUSD");
                item.WinLoss12BetUSD = reader.ToDecimal("PTWLLicenseeUSD");

                // Base GBP
                item.WinTO12BetGBP = reader.ToDecimal("WinTOLicenseeGBP");
                item.PlaceTO12BetGBP = reader.ToDecimal("PlaceTOLicenseeGBP");
                item.WPTO12BetGBP = reader.ToDecimal("WinPlaceTOLicenseeGBP");
                item.TotalTO12BetGBP = reader.ToDecimal("TotalTOLicenseeGBP");
                item.WinLoss12BetGBP = reader.ToDecimal("PTWLLicenseeGBP");

                dataList.Add(item);
            }

            return dataList;
        }
    }
}