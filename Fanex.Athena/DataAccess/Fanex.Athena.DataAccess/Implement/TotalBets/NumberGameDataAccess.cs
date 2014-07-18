// -----------------------------------------------------------------------
// <copyright file="NumberGameDataAccess.cs" company="Nexcel Solution">
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
    using Extension;
    using Fanex.Athena.DataAccess.Interface.TotalBets;
    using Fanex.Common.Extensions;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Number game data access.
    /// </summary>
    public class NumberGameDataAccess : INumberGameDataAccess
    {
        /// <summary>
        /// Get number game data list.
        /// </summary>
        /// <param name="metaModel">Number game MetaModel value.</param>
        /// <returns>A list of number game.</returns>
        public IList<NumberGame> GetNumBerGameDataList(NumberGameMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel161";
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

            using (var db = new ObjectDb(SpName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    return SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                }
            }
        }

        #region Utility

        /// <summary>
        /// Get Total.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="first">First column value.</param>
        /// <param name="second">Second column value.</param>
        /// <returns>Total value.</returns>
        private static decimal GetTotal(IDataReader reader, string first, string second)
        {
            var firstValue = reader.ToDecimal(first);
            var secondValue = reader.ToDecimal(second);

            return firstValue - secondValue;
        }

        /// <summary>
        /// Set data to list.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>A list of Number game.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
                    Justification = "Reviewed. Suppression is OK here.")]
        private static IList<NumberGame> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<NumberGame>();

            // Warrior column name
            var warrior2Nd = DataHelper.GetCurrencyColumnName("Warrior_SecondBall", isConvert, baseCurrency);
            var warrior3Rd = DataHelper.GetCurrencyColumnName("Warrior_LastBall", isConvert, baseCurrency);

            // First ball column name
            var overFirstBall = DataHelper.GetCurrencyColumnName("over_1stBall", isConvert, baseCurrency);
            var underFirstBall = DataHelper.GetCurrencyColumnName("under_1stBall", isConvert, baseCurrency);
            var oddFirstBall = DataHelper.GetCurrencyColumnName("odd_1stBall", isConvert, baseCurrency);
            var evenFirstBall = DataHelper.GetCurrencyColumnName("even_1stBall", isConvert, baseCurrency);

            // Last ball column name
            var overLastBall = DataHelper.GetCurrencyColumnName("over_lastBall", isConvert, baseCurrency);
            var underLastBall = DataHelper.GetCurrencyColumnName("under_lastBall", isConvert, baseCurrency);
            var oddLastBall = DataHelper.GetCurrencyColumnName("odd_lastBall", isConvert, baseCurrency);
            var evenLastBall = DataHelper.GetCurrencyColumnName("even_lastBall", isConvert, baseCurrency);

            // Next ball column name
            var overNextBall = DataHelper.GetCurrencyColumnName("over_next", isConvert, baseCurrency);
            var underNextBall = DataHelper.GetCurrencyColumnName("under_next", isConvert, baseCurrency);
            var oddNextBall = DataHelper.GetCurrencyColumnName("odd_next", isConvert, baseCurrency);
            var evenNextBall = DataHelper.GetCurrencyColumnName("even_next", isConvert, baseCurrency);

            // High Low
            var highColumn = DataHelper.GetCurrencyColumnName("high", isConvert, baseCurrency);
            var lowColumn = DataHelper.GetCurrencyColumnName("low", isConvert, baseCurrency);

            // Next combo
            var nextComboOverOdd = DataHelper.GetCurrencyColumnName("NextCombo_OverOdd", isConvert, baseCurrency);
            var nextComboOverEven = DataHelper.GetCurrencyColumnName("NextCombo_OverEven", isConvert, baseCurrency);
            var nextComboUnderOdd = DataHelper.GetCurrencyColumnName("NextCombo_UnderOdd", isConvert, baseCurrency);
            var nextComboUnderEven = DataHelper.GetCurrencyColumnName("NextCombo_UnderEven", isConvert, baseCurrency);

            // Odd/Even full time
            var oddFullTime = DataHelper.GetCurrencyColumnName("odd_fullTime", isConvert, baseCurrency);
            var evenFullTime = DataHelper.GetCurrencyColumnName("even_fullTime", isConvert, baseCurrency);

            // Number wheel
            var numberWheelLive = DataHelper.GetCurrencyColumnName("NumberGroup_live", isConvert, baseCurrency);
            var numberWheelPre = DataHelper.GetCurrencyColumnName("NumberGroup_pre", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new NumberGame
                {
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    MatchCode = reader.ToString("matchcode"),
                    Warrior2Nd = reader.ToDecimal(warrior2Nd),
                    Warrior3Rd = reader.ToDecimal(warrior3Rd),
                    OverUnderFirstBall = GetTotal(reader, overFirstBall, underFirstBall),
                    OddEvenFirstBall = GetTotal(reader, oddFirstBall, evenFirstBall),
                    OverUnderLastBall = GetTotal(reader, overLastBall, underLastBall),
                    OddEvenLastBall = GetTotal(reader, oddLastBall, evenLastBall),
                    OddEvenFullTime = GetTotal(reader, oddFullTime, evenFullTime),
                    OverUnderNext = GetTotal(reader, overNextBall, underNextBall),
                    OddEvenNext = GetTotal(reader, oddNextBall, evenNextBall),
                    HighLow = GetTotal(reader, highColumn, lowColumn),
                    NextComboOo = reader.ToDecimal(nextComboOverOdd),
                    NextComboOe = reader.ToDecimal(nextComboOverEven),
                    NextComboUo = reader.ToDecimal(nextComboUnderOdd),
                    NextComboUe = reader.ToDecimal(nextComboUnderEven),
                    NumberWheelLiveGame = reader.ToDecimal(numberWheelLive),
                    NumberWheelPreGame = reader.ToDecimal(numberWheelPre),
                    BetCountWarrior = reader.ToInt("betcount_Warrior"),
                    BetCountOu1St = reader.ToInt("betcount_OU1stBall"),
                    BetCountOuLast = reader.ToInt("betcount_OULastBall"),
                    BetCountOe1St = reader.ToInt("betcount_OE1stBall"),
                    BetCountOeLast = reader.ToInt("betcount_OElastBall"),
                    BetCountOeFt = reader.ToInt("betcount_OEfullTime"),
                    BetCountOuNext = reader.ToInt("betcount_OUNext"),
                    BetCountOeNext = reader.ToInt("betcount_OENext"),
                    BetCountHighLow = reader.ToInt("betcount_highlow"),
                    BetCountNextCombo = reader.ToInt("betcount_NextCombo"),
                    BetCountNumberWheelLive = reader.ToInt("betcount_NumberGroup_live"),
                    BetCountNumberWheelPre = reader.ToInt("betcount_NumberGroup_pre"),
                };

                dataList.Add(item);
            }

            return dataList;
        }

        #endregion Utility
    }
}