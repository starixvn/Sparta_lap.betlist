// -----------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Extensions;
    using Data;
    using DataAccess.TotalBet.Interfaces;
    using Fanex.Common.Extensions;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Half time/ Full time data access.
    /// </summary>
    public class HalfTimeFullTimeDataAccess : BaseDataAccess, IHalfTimeFullTimeDataAccess
    {
        /// <summary>
        /// Get HalfTime FullTime data list.
        /// </summary>
        /// <param name="metaModel">HT/FT MetaModel value.</param>
        /// <param name="totalData">HalfTimeFullTime value.</param>
        /// <returns>A list of HT/FT.</returns>
        public IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel, out HalfTimeFullTime totalData)
        {
            const string SpName = "Lrf_TotalBets_Sel16";
            var uspParams = new
            {
                LicenseeId = metaModel.LicenseeId,
                LicenseeType = metaModel.LicenseeType,
                CurrencyId = metaModel.CurrencyId,
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                TotalRows = metaModel.TotalRows,
                UserId = metaModel.UserId
            };
            IList<HalfTimeFullTime> data;
            using (var db = this.GetObjectDb(SpName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    data = SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                    reader.NextResult();
                    totalData = ReadTotalData(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                }
            }

            return data;
        }

        #region Utility

        /// <summary>
        /// Set data to list.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base Currency value.</param>
        /// <returns>A list of HT/FT.</returns>
        private static IList<HalfTimeFullTime> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<HalfTimeFullTime>();

            var totalHh = DataHelper.GetCurrencyColumnName("totalHH", isConvert, baseCurrency);
            var totalHd = DataHelper.GetCurrencyColumnName("totalHD", isConvert, baseCurrency);
            var totalHa = DataHelper.GetCurrencyColumnName("totalHA", isConvert, baseCurrency);
            var totalDh = DataHelper.GetCurrencyColumnName("totalDH", isConvert, baseCurrency);
            var totalDd = DataHelper.GetCurrencyColumnName("totalDD", isConvert, baseCurrency);
            var totalDa = DataHelper.GetCurrencyColumnName("totalDA", isConvert, baseCurrency);
            var totalAh = DataHelper.GetCurrencyColumnName("totalAH", isConvert, baseCurrency);
            var totalAd = DataHelper.GetCurrencyColumnName("totalAD", isConvert, baseCurrency);
            var totalAa = DataHelper.GetCurrencyColumnName("totalAA", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new HalfTimeFullTime
                {
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeID"),
                    AwayId = reader.ToInt("awayID"),
                    IsNeutral = reader.ToInt("isNeutral"),
                    HomeHome = reader.ToDecimal(totalHh),
                    HomeDraw = reader.ToDecimal(totalHd),
                    HomeAway = reader.ToDecimal(totalHa),
                    DrawHome = reader.ToDecimal(totalDh),
                    DrawDraw = reader.ToDecimal(totalDd),
                    DrawAway = reader.ToDecimal(totalDa),
                    AwayHome = reader.ToDecimal(totalAh),
                    AwayDraw = reader.ToDecimal(totalAd),
                    AwayAway = reader.ToDecimal(totalAa),
                };

                dataList.Add(item);
            }

            return dataList;
        }

        /// <summary>
        /// Read TotalData.
        /// </summary>
        /// <param name="reader">Data Reader class.</param>
        /// <param name="isConvert">Boolean isConvert.</param>
        /// <param name="baseCurrency">Base Currency.</param>
        /// <returns>Half Time Full Time.</returns>
        private static HalfTimeFullTime ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            if (reader.Read())
            {
                var totalHh = DataHelper.GetCurrencyColumnName("totalHH", isConvert, baseCurrency);
                var totalHd = DataHelper.GetCurrencyColumnName("totalHD", isConvert, baseCurrency);
                var totalHa = DataHelper.GetCurrencyColumnName("totalHA", isConvert, baseCurrency);
                var totalDh = DataHelper.GetCurrencyColumnName("totalDH", isConvert, baseCurrency);
                var totalDd = DataHelper.GetCurrencyColumnName("totalDD", isConvert, baseCurrency);
                var totalDa = DataHelper.GetCurrencyColumnName("totalDA", isConvert, baseCurrency);
                var totalAh = DataHelper.GetCurrencyColumnName("totalAH", isConvert, baseCurrency);
                var totalAd = DataHelper.GetCurrencyColumnName("totalAD", isConvert, baseCurrency);
                var totalAa = DataHelper.GetCurrencyColumnName("totalAA", isConvert, baseCurrency);

                return new HalfTimeFullTime
                {
                    HomeHome = reader.ToDecimal(totalHh),
                    HomeDraw = reader.ToDecimal(totalHd),
                    HomeAway = reader.ToDecimal(totalHa),
                    DrawHome = reader.ToDecimal(totalDh),
                    DrawDraw = reader.ToDecimal(totalDd),
                    DrawAway = reader.ToDecimal(totalDa),
                    AwayHome = reader.ToDecimal(totalAh),
                    AwayDraw = reader.ToDecimal(totalAd),
                    AwayAway = reader.ToDecimal(totalAa),
                };
            }

            return new HalfTimeFullTime();
        }

        #endregion Utility
    }
}