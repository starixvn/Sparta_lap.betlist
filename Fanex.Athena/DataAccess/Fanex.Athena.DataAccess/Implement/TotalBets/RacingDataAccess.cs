// -----------------------------------------------------------------------
// <copyright file="RacingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.TotalBets
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Data;
    using DataAccess.Interface.TotalBets;
    using Extension;
    using Fanex.Common;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Racing data access.
    /// </summary>
    public class RacingDataAccess : IRacingDataAccess
    {
        /// <summary>
        /// Get Racing data list.
        /// </summary>
        /// <param name="metaModel">Racing MetaModel value.</param>
        /// <returns>A list of RacingThe result value.</returns>
        public Racing GetRacingDataList(RacingMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_SelHR";
            var uspParams = new
            {
                LicenseeId = metaModel.LicenseeId,
                LicenseeType = metaModel.LicenseeType,
                CurrencyId = metaModel.CurrencyId,
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                UserId = metaModel.UserId
            };

            using (var db = new ObjectDb(SpName))
            {
                using (var ds = db.ExecuteDataSet(uspParams))
                {
                    return GetRacingData(ds, metaModel.IsConvert, metaModel.BaseCurrency);
                }
            }
        }

        #region Utility

        private static decimal GetRacingValue(DataRow dr, string com, string winLoss)
        {
            var comValue = Converter.ToDecimal(dr[com]);
            var winLossValue = Converter.ToDecimal(dr[winLoss]);

            return comValue + winLossValue;
        }

        /// <summary>
        /// Get racing data.
        /// </summary>
        /// <param name="ds">Data set value.</param>
        /// <param name="isConvert">Convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static Racing GetRacingData(DataSet ds, bool isConvert, string baseCurrency)
        {
            var matchTable = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            var totalTable = ds.Tables.Count > 1 ? ds.Tables[1] : new DataTable();

            var dataList = new List<RacingBase>();

            // Racing column
            var winTotal = DataHelper.GetCurrencyColumnName("Total_Win", isConvert, baseCurrency);
            var winRevenue = DataHelper.GetCurrencyColumnName("Total_Win_Comm", isConvert, baseCurrency);
            var winTotalWinLoss = DataHelper.GetCurrencyColumnName("Total_Win_WL", isConvert, baseCurrency);
            var placeTotal = DataHelper.GetCurrencyColumnName("Total_Place", isConvert, baseCurrency);
            var placeRevenue = DataHelper.GetCurrencyColumnName("Total_Place_Comm", isConvert, baseCurrency);
            var placeRevenueWinLoss = DataHelper.GetCurrencyColumnName("Total_Place_WL", isConvert, baseCurrency);
            var winPlaceTotal = DataHelper.GetCurrencyColumnName("Total_WinPlace", isConvert, baseCurrency);
            var winPlaceRevenue = DataHelper.GetCurrencyColumnName("Total_WinPlace_Comm", isConvert, baseCurrency);
            var winPlaceRevenueWinLoss = DataHelper.GetCurrencyColumnName("Total_WinPlace_WL", isConvert, baseCurrency);

            // Total column
            var sumWin = DataHelper.GetCurrencyColumnName("SumTotal_Win", isConvert, baseCurrency);
            var sumPlace = DataHelper.GetCurrencyColumnName("SumTotal_Place", isConvert, baseCurrency);
            var sumWinPlace = DataHelper.GetCurrencyColumnName("SumTotal_WinPlace", isConvert, baseCurrency);
            var sumWinComm = DataHelper.GetCurrencyColumnName("SumTotal_Win_Comm", isConvert, baseCurrency);
            var sumPlaceComm = DataHelper.GetCurrencyColumnName("SumTotal_Place_Comm", isConvert, baseCurrency);
            var sumWinPlaceComm = DataHelper.GetCurrencyColumnName("SumTotal_WinPlace_Comm", isConvert, baseCurrency);
            var sumWl = DataHelper.GetCurrencyColumnName("SumTotal_Win_WL", isConvert, baseCurrency);
            var sumPlaceWl = DataHelper.GetCurrencyColumnName("SumTotal_Place_WL", isConvert, baseCurrency);
            var sumWinPlaceWl = DataHelper.GetCurrencyColumnName("SumTotal_WinPlace_WL", isConvert, baseCurrency);

            // Get match list
            foreach (DataRow dr in matchTable.Rows)
            {
                var item = new RacingBase
                {
                    LeagueId = Converter.ToInt(dr["leagueid"]),
                    MatchId = Converter.ToInt(dr["matchid"]),
                    TeamId = Converter.ToInt(dr["teamid"]),
                    OddStatus = dr["oddsstatus"].ToString(),
                    KickOffTime = Converter.ToDateTime(dr["kickofftime"]),
                    HorseNo = Converter.ToInt(dr["horseno"]),
                    Race = Converter.ToInt(dr["race"]),
                    TotalWin = Converter.ToDecimal(dr[winTotal]),
                    RevenueWin = GetRacingValue(dr, winRevenue, winTotalWinLoss),
                    TotalPlace = Converter.ToDecimal(dr[placeTotal]),
                    RevenuePlace = GetRacingValue(dr, placeRevenue, placeRevenueWinLoss),
                    TotalWinPlace = Converter.ToDecimal(dr[winPlaceTotal]),
                    RevenueWinPlace = GetRacingValue(dr, winPlaceRevenue, winPlaceRevenueWinLoss)
                };

                dataList.Add(item);
            }

            var racingData = new Racing
                {
                    RacingDataList = dataList
                };

            // Get total
            foreach (DataRow dr in totalTable.Rows)
            {
                racingData.SumTotalWin = Converter.ToDecimal(dr[sumWin]);
                racingData.SumRevenueWin = GetRacingValue(dr, sumWinComm, sumWl);
                racingData.SumTotalPlace = Converter.ToDecimal(dr[sumPlace]);
                racingData.SumRevenuePlace = GetRacingValue(dr, sumPlaceComm, sumPlaceWl);
                racingData.SumTotalWinPlace = Converter.ToDecimal(dr[sumWinPlace]);
                racingData.SumRevenueWinPlace = GetRacingValue(dr, sumWinPlaceComm, sumWinPlaceWl);
            }

            return racingData;
        }

        #endregion Utility
    }
}