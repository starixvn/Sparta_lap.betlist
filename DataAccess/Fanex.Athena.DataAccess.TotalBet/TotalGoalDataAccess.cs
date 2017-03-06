// -----------------------------------------------------------------------
// <copyright file="TotalGoalDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using DataAccess.TotalBet.Interfaces;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Common.Extensions;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class TotalGoalDataAccess : BaseDataAccess, ITotalGoalDataAccess
    {
        public IList<TotalGoal> GetTotalGoalDataList(TotalGoalMetaModel metaModel, out TotalGoal totalData)
        {
            const string UspName = "Lrf_TotalBets_Sel6";
            var uspParams = new
            {
                AgentList = metaModel.AgentListXml,
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                TotalRows = metaModel.TotalRows
            };
            IList<TotalGoal> data;
            using (var db = this.GetObjectDb(UspName))
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

        private static IList<TotalGoal> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<TotalGoal>();

            var total01Column = DataHelper.GetCurrencyColumnName("total01", isConvert, baseCurrency);
            var total23Column = DataHelper.GetCurrencyColumnName("total23", isConvert, baseCurrency);
            var total46Column = DataHelper.GetCurrencyColumnName("total46", isConvert, baseCurrency);
            var total07OverColumn = DataHelper.GetCurrencyColumnName("total7over", isConvert, baseCurrency);
            var totalFirst01Column = DataHelper.GetCurrencyColumnName("fhtotal01", isConvert, baseCurrency);
            var totalFirst23Column = DataHelper.GetCurrencyColumnName("fhtotal23", isConvert, baseCurrency);
            var total04OverColumn = DataHelper.GetCurrencyColumnName("fhtotal4over", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new TotalGoal
                {
                    //// LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeID"),
                    AwayId = reader.ToInt("awayID"),
                    IsNeutral = reader.ToInt("isNeutral"),
                    Total01 = reader.ToDecimal(total01Column),
                    Total23 = reader.ToDecimal(total23Column),
                    Total46 = reader.ToDecimal(total46Column),
                    Total7Over = reader.ToDecimal(total07OverColumn),
                    TotalFirstHalf01 = reader.ToNullableDecimal(totalFirst01Column),
                    TotalFirstHalf23 = reader.ToNullableDecimal(totalFirst23Column),
                    Total4Over = reader.ToNullableDecimal(total04OverColumn)
                };

                dataList.Add(item);
            }

            return dataList;
        }

        private static TotalGoal ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            if (reader.Read())
            {
                var total01Column = DataHelper.GetCurrencyColumnName("total01", isConvert, baseCurrency);
                var total23Column = DataHelper.GetCurrencyColumnName("total23", isConvert, baseCurrency);
                var total46Column = DataHelper.GetCurrencyColumnName("total46", isConvert, baseCurrency);
                var total07OverColumn = DataHelper.GetCurrencyColumnName("total7over", isConvert, baseCurrency);
                var totalFirst01Column = DataHelper.GetCurrencyColumnName("fhtotal01", isConvert, baseCurrency);
                var totalFirst23Column = DataHelper.GetCurrencyColumnName("fhtotal23", isConvert, baseCurrency);
                var total04OverColumn = DataHelper.GetCurrencyColumnName("fhtotal4over", isConvert, baseCurrency);

                return new TotalGoal
                {
                    Total01 = reader.ToDecimal(total01Column) + reader.ToDecimal(total23Column) + reader.ToDecimal(total46Column) + reader.ToDecimal(total07OverColumn),
                    TotalFirstHalf01 = reader.ToDecimal(totalFirst01Column) + reader.ToDecimal(totalFirst23Column) + reader.ToDecimal(total04OverColumn)
                };
            }

            return new TotalGoal();
        }
    }
}