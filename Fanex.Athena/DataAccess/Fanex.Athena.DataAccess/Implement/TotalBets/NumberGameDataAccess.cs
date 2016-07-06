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
    using Core.Extensions;
    using Data;
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
        /// <param name="totalData">Number game value.</param>
        /// <returns>A list of number game.</returns>
        public IList<NumberGame> GetNumBerGameDataList(NumberGameMetaModel metaModel, out NumberGame totalData)
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

            IList<NumberGame> data;
            using (var db = new ObjectDb(SpName))
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
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>A list of Number game.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
                    Justification = "Reviewed. Suppression is OK here.")]
        private static IList<NumberGame> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<NumberGame>();

            // Warrior column name
            var warrior2NdColumn = DataHelper.GetCurrencyColumnName("Warrior_SecondBall", isConvert, baseCurrency);
            var warrior3RdColumn = DataHelper.GetCurrencyColumnName("Warrior_LastBall", isConvert, baseCurrency);

            // First ball column name
            var overFirstBallColumn = DataHelper.GetCurrencyColumnName("over_1stBall", isConvert, baseCurrency);
            var underFirstBallColumn = DataHelper.GetCurrencyColumnName("under_1stBall", isConvert, baseCurrency);
            var oddFirstBallColumn = DataHelper.GetCurrencyColumnName("odd_1stBall", isConvert, baseCurrency);
            var evenFirstBallColumn = DataHelper.GetCurrencyColumnName("even_1stBall", isConvert, baseCurrency);

            // Last ball column name
            var overLastBallColumn = DataHelper.GetCurrencyColumnName("over_lastBall", isConvert, baseCurrency);
            var underLastBallColumn = DataHelper.GetCurrencyColumnName("under_lastBall", isConvert, baseCurrency);
            var oddLastBallColumn = DataHelper.GetCurrencyColumnName("odd_lastBall", isConvert, baseCurrency);
            var evenLastBallColumn = DataHelper.GetCurrencyColumnName("even_lastBall", isConvert, baseCurrency);

            // Next ball column name
            var overNextBallColumn = DataHelper.GetCurrencyColumnName("over_next", isConvert, baseCurrency);
            var underNextBallColumn = DataHelper.GetCurrencyColumnName("under_next", isConvert, baseCurrency);
            var oddNextBallColumn = DataHelper.GetCurrencyColumnName("odd_next", isConvert, baseCurrency);
            var evenNextBallColumn = DataHelper.GetCurrencyColumnName("even_next", isConvert, baseCurrency);

            // High Low
            var highColumnColumn = DataHelper.GetCurrencyColumnName("high", isConvert, baseCurrency);
            var lowColumnColumn = DataHelper.GetCurrencyColumnName("low", isConvert, baseCurrency);

            // Next combo
            var nextComboOverOddColumn = DataHelper.GetCurrencyColumnName("NextCombo_OverOdd", isConvert, baseCurrency);
            var nextComboOverEvenColumn = DataHelper.GetCurrencyColumnName("NextCombo_OverEven", isConvert, baseCurrency);
            var nextComboUnderOddColumn = DataHelper.GetCurrencyColumnName("NextCombo_UnderOdd", isConvert, baseCurrency);
            var nextComboUnderEvenColumn = DataHelper.GetCurrencyColumnName("NextCombo_UnderEven", isConvert, baseCurrency);

            // Odd/Even full time
            var oddFullTimeColumn = DataHelper.GetCurrencyColumnName("odd_fullTime", isConvert, baseCurrency);
            var evenFullTimeColumn = DataHelper.GetCurrencyColumnName("even_fullTime", isConvert, baseCurrency);

            // Number wheel
            var numberWheelLiveColumn = DataHelper.GetCurrencyColumnName("NumberGroup_live", isConvert, baseCurrency);
            var numberWheelPreColumn = DataHelper.GetCurrencyColumnName("NumberGroup_pre", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new NumberGame
                {
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    MatchCode = reader.ToString("matchcode"),
                    Warrior2ND = reader.ToDecimal(warrior2NdColumn),
                    Warrior3RD = reader.ToDecimal(warrior3RdColumn),

                    OverUnderFirstBall = reader.ToDecimal(overFirstBallColumn) - reader.ToDecimal(underFirstBallColumn),
                    OddEvenFirstBall = reader.ToDecimal(oddFirstBallColumn) - reader.ToDecimal(evenFirstBallColumn),
                    OverUnderLastBall = reader.ToDecimal(overLastBallColumn) - reader.ToDecimal(underLastBallColumn),
                    OddEvenLastBall = reader.ToDecimal(oddLastBallColumn) - reader.ToDecimal(evenLastBallColumn),
                    OddEvenFullTime = reader.ToDecimal(oddFullTimeColumn) - reader.ToDecimal(evenFullTimeColumn),
                    OverUnderNext = reader.ToDecimal(overNextBallColumn) - reader.ToDecimal(underNextBallColumn),
                    OddEvenNext = reader.ToDecimal(oddNextBallColumn) - reader.ToDecimal(evenNextBallColumn),
                    HighLow = reader.ToDecimal(highColumnColumn) - reader.ToDecimal(lowColumnColumn),

                    RunningStakeOverUnderFirstBall = reader.ToDecimal(overFirstBallColumn) + reader.ToDecimal(underFirstBallColumn),
                    RunningStakeOddEvenFirstBall = reader.ToDecimal(oddFirstBallColumn) + reader.ToDecimal(evenFirstBallColumn),
                    RunningStakeOverUnderLastBall = reader.ToDecimal(overLastBallColumn) + reader.ToDecimal(underLastBallColumn),
                    RunningStakeOddEvenLastBall = reader.ToDecimal(oddLastBallColumn) + reader.ToDecimal(evenLastBallColumn),
                    RunningStakeOddEvenFullTime = reader.ToDecimal(oddFullTimeColumn) + reader.ToDecimal(evenFullTimeColumn),
                    RunningStakeOverUnderNext = reader.ToDecimal(overNextBallColumn) + reader.ToDecimal(underNextBallColumn),
                    RunningStakeOddEvenNext = reader.ToDecimal(oddNextBallColumn) + reader.ToDecimal(evenNextBallColumn),
                    RunningStakeHighLow = reader.ToDecimal(highColumnColumn) + reader.ToDecimal(lowColumnColumn),

                    NextComboOo = reader.ToDecimal(nextComboOverOddColumn),
                    NextComboOe = reader.ToDecimal(nextComboOverEvenColumn),
                    NextComboUo = reader.ToDecimal(nextComboUnderOddColumn),
                    NextComboUE = reader.ToDecimal(nextComboUnderEvenColumn),
                    NumberWheelLiveGame = reader.ToDecimal(numberWheelLiveColumn),
                    NumberWheelPreGame = reader.ToDecimal(numberWheelPreColumn),
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

        /// <summary>
        /// Set data to object.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>A Number game.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        private NumberGame ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            if (!reader.Read())
            {
                return new NumberGame();
            }

            // Warrior column name
            var warrior2NdColumn = DataHelper.GetCurrencyColumnName("Warrior_SecondBall", isConvert, baseCurrency);
            var warrior3RdColumn = DataHelper.GetCurrencyColumnName("Warrior_LastBall", isConvert, baseCurrency);

            // First ball column name
            var overFirstBallColumn = DataHelper.GetCurrencyColumnName("over_1stBall", isConvert, baseCurrency);
            var underFirstBallColumn = DataHelper.GetCurrencyColumnName("under_1stBall", isConvert, baseCurrency);
            var oddFirstBallColumn = DataHelper.GetCurrencyColumnName("odd_1stBall", isConvert, baseCurrency);
            var evenFirstBallColumn = DataHelper.GetCurrencyColumnName("even_1stBall", isConvert, baseCurrency);

            // Last ball column name
            var overLastBallColumn = DataHelper.GetCurrencyColumnName("over_lastBall", isConvert, baseCurrency);
            var underLastBallColumn = DataHelper.GetCurrencyColumnName("under_lastBall", isConvert, baseCurrency);
            var oddLastBallColumn = DataHelper.GetCurrencyColumnName("odd_lastBall", isConvert, baseCurrency);
            var evenLastBallColumn = DataHelper.GetCurrencyColumnName("even_lastBall", isConvert, baseCurrency);

            // Next ball column name
            var overNextBallColumn = DataHelper.GetCurrencyColumnName("over_next", isConvert, baseCurrency);
            var underNextBallColumn = DataHelper.GetCurrencyColumnName("under_next", isConvert, baseCurrency);
            var oddNextBallColumn = DataHelper.GetCurrencyColumnName("odd_next", isConvert, baseCurrency);
            var evenNextBallColumn = DataHelper.GetCurrencyColumnName("even_next", isConvert, baseCurrency);

            // High Low
            var highColumnColumn = DataHelper.GetCurrencyColumnName("high", isConvert, baseCurrency);
            var lowColumnColumn = DataHelper.GetCurrencyColumnName("low", isConvert, baseCurrency);

            // Next combo
            var nextComboOverOddColumn = DataHelper.GetCurrencyColumnName("NextCombo_OverOdd", isConvert, baseCurrency);
            var nextComboOverEvenColumn = DataHelper.GetCurrencyColumnName("NextCombo_OverEven", isConvert, baseCurrency);
            var nextComboUnderOddColumn = DataHelper.GetCurrencyColumnName("NextCombo_UnderOdd", isConvert, baseCurrency);
            var nextComboUnderEvenColumn = DataHelper.GetCurrencyColumnName("NextCombo_UnderEven", isConvert, baseCurrency);

            // Odd/Even full time
            var oddFullTimeColumn = DataHelper.GetCurrencyColumnName("odd_fullTime", isConvert, baseCurrency);
            var evenFullTimeColumn = DataHelper.GetCurrencyColumnName("even_fullTime", isConvert, baseCurrency);

            // Number wheel
            var numberWheelLiveColumn = DataHelper.GetCurrencyColumnName("NumberGroup_live", isConvert, baseCurrency);
            var numberWheelPreColumn = DataHelper.GetCurrencyColumnName("NumberGroup_pre", isConvert, baseCurrency);

            return new NumberGame
            {
                Warrior2ND = reader.ToDecimal(warrior2NdColumn),
                Warrior3RD = reader.ToDecimal(warrior3RdColumn),

                OverUnderFirstBall = reader.ToDecimal(overFirstBallColumn) - reader.ToDecimal(underFirstBallColumn),
                OddEvenFirstBall = reader.ToDecimal(oddFirstBallColumn) - reader.ToDecimal(evenFirstBallColumn),
                OverUnderLastBall = reader.ToDecimal(overLastBallColumn) - reader.ToDecimal(underLastBallColumn),
                OddEvenLastBall = reader.ToDecimal(oddLastBallColumn) - reader.ToDecimal(evenLastBallColumn),
                OddEvenFullTime = reader.ToDecimal(oddFullTimeColumn) - reader.ToDecimal(evenFullTimeColumn),
                OverUnderNext = reader.ToDecimal(overNextBallColumn) - reader.ToDecimal(underNextBallColumn),
                OddEvenNext = reader.ToDecimal(oddNextBallColumn) - reader.ToDecimal(evenNextBallColumn),
                HighLow = reader.ToDecimal(highColumnColumn) - reader.ToDecimal(lowColumnColumn),

                RunningStakeOverUnderFirstBall = reader.ToDecimal(overFirstBallColumn) + reader.ToDecimal(underFirstBallColumn),
                RunningStakeOddEvenFirstBall = reader.ToDecimal(oddFirstBallColumn) + reader.ToDecimal(evenFirstBallColumn),
                RunningStakeOverUnderLastBall = reader.ToDecimal(overLastBallColumn) + reader.ToDecimal(underLastBallColumn),
                RunningStakeOddEvenLastBall = reader.ToDecimal(oddLastBallColumn) + reader.ToDecimal(evenLastBallColumn),
                RunningStakeOddEvenFullTime = reader.ToDecimal(oddFullTimeColumn) + reader.ToDecimal(evenFullTimeColumn),
                RunningStakeOverUnderNext = reader.ToDecimal(overNextBallColumn) + reader.ToDecimal(underNextBallColumn),
                RunningStakeOddEvenNext = reader.ToDecimal(oddNextBallColumn) + reader.ToDecimal(evenNextBallColumn),
                RunningStakeHighLow = reader.ToDecimal(highColumnColumn) + reader.ToDecimal(lowColumnColumn),

                NextComboOo = reader.ToDecimal(nextComboOverOddColumn),
                NextComboOe = reader.ToDecimal(nextComboOverEvenColumn),
                NextComboUo = reader.ToDecimal(nextComboUnderOddColumn),
                NextComboUE = reader.ToDecimal(nextComboUnderEvenColumn),
                NumberWheelLiveGame = reader.ToDecimal(numberWheelLiveColumn),
                NumberWheelPreGame = reader.ToDecimal(numberWheelPreColumn)
            };
        }

        #endregion Utility
    }
}